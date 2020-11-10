// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using BSOA.Extensions;
using BSOA.Model;

namespace BSOA.GC
{
    /// <summary>
    ///  DatabaseCollector is responsible for garbage collection in BSOA database tables.
    ///  When BSOA object model objects are created, the data for them is really stored in a new row in an internal table.
    ///  The .NET garbage collector cleans up the object, but BSOA must clean up unused rows.
    ///  
    ///  This collector must:
    ///  - Remove all rows not reachable from the root, so they aren't serialized out with the database.
    ///  - Update all Ref and RefList columns for swapped rows so that they still point to the same data.
    ///
    ///  - Copy data for rows which aren't reachable to a temporary database, so orphaned OM objects will still know their data.
    ///  - Update OM instances in memory to refer to the right row (maybe same row, maybe swapped row, maybe row now in temp table).
    /// </summary>
    internal class DatabaseCollector
    {
        public bool MaintainObjectModel { get; }
        public IDatabase Database { get; }

        private Dictionary<string, TableCollector> _tableCollectors;

        // Create a Temp Database (just in time) if needed, to copy unreachable items to
        private IDatabase _tempDatabase;
        public IDatabase TempDatabase => _tempDatabase ??= ConstructorBuilder.GetConstructor<Func<IDatabase>>(Database.GetType())();

        public DatabaseCollector(IDatabase database)
        {
            MaintainObjectModel = true;
            Database = database;

            // 1. Build a Collector for each Table
            _tableCollectors = new Dictionary<string, TableCollector>();
            foreach (var table in database.Tables)
            {
                _tableCollectors[table.Key] = new TableCollector(this, table.Value, table.Key);
            }

            // 2. Hook up ref columns between the source and target table collectors
            foreach (var table in database.Tables)
            {
                string tableName = table.Key;
                TableCollector sourceCollector = _tableCollectors[tableName];

                foreach (var column in table.Value.Columns)
                {
                    IRefColumn refColumn = column.Value as IRefColumn;
                    if (refColumn != null)
                    {
                        sourceCollector.AddRefColumn(column.Key, refColumn, _tableCollectors[refColumn.ReferencedTableName]);
                    }
                }
            }
        }

        public bool Collect()
        {
            long tableRowTotal = Database.Tables.Values.Sum((table) => table.Count);

            // 3. Walk reachable rows (add root, which will recursively add everything reachable)
            _tableCollectors.Values.ForEach((collector) => collector.ResetAddedRows());
            long reachableTotal = _tableCollectors[Database.RootTableName].Traverse(0);

            // If nothing or everything is reachable, no splitting is needed. Stop.
            if (reachableTotal == 1 || reachableTotal == tableRowTotal)
            {
                return false;
            }

            _tableCollectors.Values.ForEach((collector) => collector.SaveReachableRows());

            // 4. Walk *unreachable* rows, assign temp DB row indices to each row-to-remove, and copy the rows to Temp DB
            if (MaintainObjectModel)
            {
                _tableCollectors.Values.ForEach((collector) => collector.ResetAddedRows());
                _tableCollectors.Values.ForEach((collector) => collector.WalkUnreachableGraph());
                _tableCollectors.Values.ForEach((collector) => collector.SaveUnreachableRows());

                _tableCollectors.Values.ForEach((collector) => collector.BuildRowToTempRowMap());
                _tableCollectors.Values.ForEach((collector) => collector.CopyUnreachableGraphToTemp());
            }

            // 5. Swap and Remove to clean up all non-reachable rows from 'main' database
            // 6. Turn existing table instances into "traps" to update object model instances to the (now current) table and row.
            _tableCollectors.Values.ForEach((collector) => collector.RemoveNonReachableRows());

            return true;
        }
    }

    internal class TableCollector : IGraphTraverser
    {
        private DatabaseCollector _databaseCollector;

        // Table this Collector is assigned to (so it can Swap and Remove to clean unused rows)
        private ITable _table;
        private string _tableName;
        private int _initialCount;

        // List of columns from this table to other tables (to walk reachable graph)
        private List<ColumnCollector> _refsFromTable;

        // List of columns from other tables to this table (to remap indices of Swapped rows)
        private List<ColumnCollector> _refsToTable;

        // Tracks rows included in the current recursive walk
        private bool[] _addedRows;
        private int _addedCount;

        // Tracks rows which are reachable from the root element
        private bool[] _reachableRows;
        private int _reachableCount;

        // Tracks all rows referenced by any unreachable row (may overlap with reachable rows)
        private bool[] _unreachableRows;
        private int _unreachableCount;

        // Keep the temp table (to which unreachables are copied) and mappings from current row index to temp row index and back
        private ITable _tempTable;
        private int[] _tempIndexToRowIndex;
        private int[] _rowIndexToTempIndex;

        public TableCollector(DatabaseCollector databaseCollector, ITable table, string tableName)
        {
            _databaseCollector = databaseCollector;
            _table = table;
            _tableName = tableName;

            _initialCount = table.Count;
        }

        public void AddRefColumn(string columnName, IRefColumn column, TableCollector target)
        {
            if (_refsFromTable == null) { _refsFromTable = new List<ColumnCollector>(); }
            if (target._refsToTable == null) { target._refsToTable = new List<ColumnCollector>(); }

            ColumnCollector collector = new ColumnCollector(columnName, column, target);

            // Add column to 'RefsTo' in the target (for remapping indices)
            target._refsToTable.Add(collector);

            // Add column to 'RefsFrom' in the source (for walking reachable indices)
            _refsFromTable.Add(collector);
        }

        public void ResetAddedRows()
        {
            _addedRows = new bool[_initialCount];
            _addedCount = 0;
        }

        public long Traverse(int index)
        {
            long sum = 0;

            if (_addedRows[index] == false)
            {
                _addedRows[index] = true;
                _addedCount++;
                sum++;

                if (_refsFromTable != null)
                {
                    foreach (IGraphTraverser collector in _refsFromTable)
                    {
                        sum += collector.Traverse(index);
                    }
                }
            }

            return sum;
        }

        public void SaveReachableRows()
        {
            _reachableRows = _addedRows;
            _reachableCount = _addedCount;

            _addedRows = null;
            _addedCount = 0;
        }

        public long WalkUnreachableGraph()
        {
            if (_reachableCount == _initialCount) { return 0; }

            // Traverse all unreachable rows recusively, finding everything they reference.
            // This will include all unreachable rows, but also anything reachable but also referenced by something unreachable.
            long sum = 0;
            for (int i = 0; i < _reachableRows.Length; ++i)
            {
                if (_reachableRows[i] == false)
                {
                    sum += Traverse(i);
                }
            }

            return sum;
        }

        public void SaveUnreachableRows()
        {
            _unreachableRows = _addedRows;
            _unreachableCount = _addedCount;

            _addedRows = null;
            _addedCount = 0;
        }

        public void BuildRowToTempRowMap()
        {
            // Assign a new temp row index to every row in the unreachable graph.
            _tempIndexToRowIndex = new int[_unreachableCount];
            _rowIndexToTempIndex = new int[_unreachableRows.Length];

            int tempCount = 0;
            for (int i = 0; i < _unreachableRows.Length; ++i)
            {
                int tempIndex = -1;

                if (_unreachableRows[i])
                {
                    tempIndex = tempCount;
                    tempCount++;

                    _tempIndexToRowIndex[tempIndex] = i;
                }

                _rowIndexToTempIndex[i] = tempIndex;
            }
        }

        public void CopyUnreachableGraphToTemp()
        {
            if (_unreachableCount == 0) { return; }

            _tempTable = _databaseCollector.TempDatabase.Tables[_tableName];

            // Copy every row in the unreachable graph to the temp table *non-recursively*
            foreach (string columnName in _table.Columns.Keys)
            {
                IColumn source = _table.Columns[columnName];
                IColumn temp = _tempTable.Columns[columnName];

                for (int tempIndex = 0; tempIndex < _tempIndexToRowIndex.Length; ++tempIndex)
                {
                    int sourceIndex = _tempIndexToRowIndex[tempIndex];
                    temp.CopyItem(tempIndex, source, sourceIndex);
                }
            }

            // Update every Ref and RefList in the temp copy of *this* table to use the re-assigned temp indices from the corresponding referenced temp table
            if (_refsFromTable != null)
            {
                foreach (var refCollector in _refsFromTable)
                {
                    IRefColumn tempTableColumn = (IRefColumn)_tempTable.Columns[refCollector.ColumnName];
                    TableCollector referencedTableCollector = refCollector.ReferencedTableCollector;
                    tempTableColumn.ForEach((slice) => IntRemapper.Instance.Remap(slice, referencedTableCollector._rowIndexToTempIndex));
                }
            }

            // Ensure temp table count correct
            _tempTable.SetCount(_tempIndexToRowIndex.Length);
        }

        public bool RemoveNonReachableRows()
        {
            if (_reachableCount == _initialCount) { return false; }

            if (!_databaseCollector.MaintainObjectModel)
            {
                // Clean up all non-reachable rows
                GarbageCollector.Collect(_table, _refsToTable.Select((c) => c.Column), _reachableRows);
            }
            else
            {
                IDatabase database = _databaseCollector.Database;

                // Construct 'successor' Table with the same columns
                Func<IDatabase, Dictionary<string, IColumn>, ITable> tableBuilder = ConstructorBuilder.GetConstructor<Func<IDatabase, Dictionary<string, IColumn>, ITable>>(_table.GetType());
                ITable successor = tableBuilder(database, new Dictionary<string, IColumn>(_table.Columns));

                // Update the database to refer to the successor table (in new object model instances returned)
                database.Tables[_tableName] = successor;
                database.GetOrBuildTables();

                // Find the 'temp' copy of this table which unreachable rows were copied to
                ITable temp = _databaseCollector.TempDatabase.Tables[_tableName];

                // Build a RowUpdater to redirect object model instances to the successor or temp tables
                RowUpdater updater = new RowUpdater(successor, temp);

                // Clean up all non-reachable rows, and fill out the updater with where to redirect them
                GarbageCollector.Collect(_table, _refsToTable.Select((c) => c.Column), _reachableRows, updater, _rowIndexToTempIndex);

                // Tell the previous table to redirect object model objects
                _table.Updater = updater;

                // Tell the successor table about the new row count (Collect removes happened after it was created)
                successor.SetCount(_table.Count);
            }

            return true;
        }
    }

    /// <summary>
    ///  ColumnCollector adapts an IRefColumn into an IGraphTraverser by also
    ///  maintaining a reference to the collector from the referenced table,
    ///  so that traversals cross tables.
    /// </summary>
    internal struct ColumnCollector : IGraphTraverser
    {
        public string ColumnName { get; }
        public IRefColumn Column { get; }
        public TableCollector ReferencedTableCollector { get; }

        public ColumnCollector(string columnName, IRefColumn column, TableCollector referencedTableCollector)
        {
            ColumnName = columnName;
            Column = column;
            ReferencedTableCollector = referencedTableCollector;
        }

        public long Traverse(int index)
        {
            return Column.Traverse(index, ReferencedTableCollector);
        }
    }
}