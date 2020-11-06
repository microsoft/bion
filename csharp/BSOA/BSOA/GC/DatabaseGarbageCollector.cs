using BSOA.Collections;
using BSOA.Column;
using BSOA.Extensions;
using BSOA.Model;

using System;
using System.Collections.Generic;
using System.Linq;

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
            long reachableTotal = _tableCollectors[Database.RootTableName].AddRow(0);

            // If nothing or everything is reachable, no splitting is needed. Stop.
            if (reachableTotal == 1 || reachableTotal == tableRowTotal)
            {
                return false;
            }

            _tableCollectors.Values.ForEach((collector) => collector.IdentifyUnreachableRows());

            // 4. Walk *unreachable* rows, assign temp DB row indices to each row-to-remove, and copy the rows to Temp DB
            if (MaintainObjectModel)
            {
                _tableCollectors.Values.ForEach((collector) => collector.ResetAddedRows());
                _tableCollectors.Values.ForEach((collector) => collector.WalkUnreachableGraph());
                _tableCollectors.Values.ForEach((collector) => collector.BuildRowToTempRowMap());
                _tableCollectors.Values.ForEach((collector) => collector.CopyUnreachableGraphToTemp());
            }

            // 5. Swap and Remove to clean up all unreachable rows from 'main' database
            bool dataRemoved = false;
            _tableCollectors.Values.ForEach((collector) => dataRemoved |= collector.RemoveUnreachableRows());

            // 6. Turn existing table instances into "traps" to update object model instances to the (now current) table and row.
            if (MaintainObjectModel)
            {
                _tableCollectors.Values.ForEach((collector) => collector.SetObjectModelUpdateTrap());
            }

            return dataRemoved;
        }
    }

    internal class TableCollector
    {
        private DatabaseCollector _databaseCollector;

        // Table this Collector is assigned to (so it can Swap and Remove to clean unused rows)
        private ITable _table;
        private string _tableName;

        // List of columns from this table to other tables (to walk reachable graph)
        private List<ICollector> _refsFromTable;

        // List of columns from other tables to this table (to remap indices of Swapped rows)
        private List<IRefColumn> _refsToTable;

        // Added rows tracks rows reachable from the root (first walk) and everything to copy to temp (second walk)
        private bool[] _addedRows;

        // The set of rows which were unreachable from the root (and must be removed before write)
        private int[] _unreachableRows;

        // Keep the temp table (to which unreachables are copied) and mappings from current row index to temp row index and back
        private ITable _tempTable;
        private List<int> _tempIndexToRowIndex;
        private int[] _rowIndexToTempIndex;

        public TableCollector(DatabaseCollector databaseCollector, ITable table, string tableName)
        {
            _databaseCollector = databaseCollector;
            _table = table;
            _tableName = tableName;
        }

        public void AddRefColumn(string columnName, IRefColumn column, TableCollector target)
        {
            if (_refsFromTable == null) { _refsFromTable = new List<ICollector>(); }
            if (target._refsToTable == null) { target._refsToTable = new List<IRefColumn>(); }

            // Add column to 'RefsTo' in the target (for remapping indices)
            target._refsToTable.Add(column);

            // Add column to 'RefsFrom' in the source (for walking reachable indices)
            if (column is RefColumn)
            {
                _refsFromTable.Add(new RefColumnCollector(columnName, (RefColumn)column, target));
            }
            else if (column is RefListColumn)
            {
                _refsFromTable.Add(new RefListColumnCollector(columnName, (RefListColumn)column, target));
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void ResetAddedRows()
        {
            _addedRows = new bool[_table.Count];
        }

        public long AddRow(int index)
        {
            long sum = 0;

            if (_addedRows[index] == false)
            {
                _addedRows[index] = true;
                sum++;

                if (_refsFromTable != null)
                {
                    foreach (ICollector collector in _refsFromTable)
                    {
                        sum += collector.AddRow(index);
                    }
                }
            }

            return sum;
        }

        public void IdentifyUnreachableRows()
        {
            // Build an array of all row indices which were unreachable from the root.
            // These rows will be removed from the database.

            List<int> unreachableRows = new List<int>();
            for (int i = 0; i < _addedRows.Length; ++i)
            {
                // PERF RISK: Need faster way to find unused values; pivot to BitVector and add vectorized mechanism?
                if (!_addedRows[i]) { unreachableRows.Add(i); }
            }

            if (unreachableRows.Count > 0)
            {
                _unreachableRows = unreachableRows.ToArray();
            }
        }

        public void WalkUnreachableGraph()
        {
            if (_unreachableRows == null) { return; }

            // Traverse all unreachable rows recusively, finding everything they reference.
            // This will include all unreachable rows, but also anything reachable but also referenced by something unreachable.
            foreach (int rowIndex in _unreachableRows)
            {
                AddRow(rowIndex);
            }
        }

        public void BuildRowToTempRowMap()
        {
            // Assign a new temp row index to every row in the unreachable graph.
            _tempIndexToRowIndex = new List<int>();
            _rowIndexToTempIndex = new int[_addedRows.Length];

            int tempCount = 0;
            for (int i = 0; i < _addedRows.Length; ++i)
            {
                int tempIndex = -1;

                if (_addedRows[i])
                {
                    tempIndex = tempCount;
                    tempCount++;

                    _tempIndexToRowIndex.Add(i);
                }

                _rowIndexToTempIndex[i] = tempIndex;
            }
        }

        public void CopyUnreachableGraphToTemp()
        {
            if (_tempIndexToRowIndex == null || _tempIndexToRowIndex.Count == 0) { return; }

            _tempTable = _databaseCollector.TempDatabase.Tables[_tableName];

            // Copy every row in the unreachable graph to the temp table *non-recursively*
            foreach (string columnName in _table.Columns.Keys)
            {
                IColumn source = _table.Columns[columnName];
                IColumn temp = _tempTable.Columns[columnName];

                for (int tempIndex = 0; tempIndex < _tempIndexToRowIndex.Count; ++tempIndex)
                {
                    int sourceIndex = _tempIndexToRowIndex[tempIndex];
                    temp.CopyItem(tempIndex, source, sourceIndex);
                }
            }

            // Update every Ref and RefList in the temp copy of this table to use the re-assigned temp indices from each referenced table
            if (_refsFromTable != null)
            {
                foreach (var refCollector in _refsFromTable)
                {
                    IRefColumn temp = (IRefColumn)_tempTable.Columns[refCollector.ColumnName];
                    temp.ForEach(refCollector.Collector.FixReferences);
                }
            }

            // Ensure temp table count correct
            _tempTable.SetCount(_tempIndexToRowIndex.Count);
        }

        private void FixReferences(ArraySlice<int> slice)
        {
            int[] array = slice.Array;
            int end = slice.Index + slice.Count;
            for (int i = slice.Index; i < end; ++i)
            {
                int index = array[i];
                if (index >= 0)
                {
                    array[i] = _rowIndexToTempIndex[index];
                }
            }
        }

        public bool RemoveUnreachableRows()
        {
            if (_unreachableRows == null) { return false; }

            // TODO: Deduplicate with GarbageCollector.Collect()
            IColumn current = _table;
            IRemapper<int> remapper = IntRemapper.Instance;

            int remapFrom = (current.Count - _unreachableRows.Length);

            // Swap the *values* to the end of the values array
            for (int i = 0; i < _unreachableRows.Length; ++i)
            {
                current.Swap(remapFrom + i, _unreachableRows[i]);
            }

            // Remove the unused values that are now at the end of the array
            current.RemoveFromEnd(_unreachableRows.Length);

            // Trim values afterward to clean up any newly unused space
            current.Trim();

            // Remap indices from all tables which point to this one to use the updated indices
            if (_refsToTable != null)
            {
                foreach (INumberColumn<int> refToTable in _refsToTable)
                {
                    refToTable.ForEach((slice) => remapper.RemapAbove(slice, remapFrom, _unreachableRows));
                }
            }

            return true;
        }

        public void SetObjectModelUpdateTrap()
        {
            if (_unreachableRows == null) { return; }

            IDatabase database = _databaseCollector.Database;
            ITable current = _table;
            int remapFrom = _table.Count;
            int[] remapped = _unreachableRows;

            // Construct a new 'latest' Table with the real, updated columns
            Func<IDatabase, Dictionary<string, IColumn>, ITable> tableBuilder = ConstructorBuilder.GetConstructor<Func<IDatabase, Dictionary<string, IColumn>, ITable>>(current.GetType());
            ITable latest = tableBuilder(database, new Dictionary<string, IColumn>(current.Columns));

            // Update the database to see the 'latest' table
            database.Tables[_tableName] = latest;
            database.GetOrBuildTables();

            // Find the 'temp' copy of this table which unreachable rows were copied to
            ITable temp = _databaseCollector.TempDatabase.Tables[_tableName];

            // Build a RowUpdater to redirect object model instances to the temp or latest table copies
            RowUpdater updater = new RowUpdater(latest, temp);

            // For each removed item...
            for (int i = 0; i < remapped.Length; ++i)
            {
                int removedRowIndex = remapped[i];
                int swappedRowIndex = remapFrom + i;
                int removedRowTempTableIndex = _rowIndexToTempIndex[remapped[i]];

                // Tell the updater that the item-to-remove is in the temp table
                updater.AddMapping(removedRowIndex, removedRowTempTableIndex, movedToTemp: true);

                // Tell the updater that the item swapped in for the removed item was swapped
                updater.AddMapping(swappedRowIndex, removedRowIndex, movedToTemp: false);
            }

            current.Updater = updater;
        }
    }

    /// <summary>
    ///  Garbage Collection is built on ICollectors, which track the references from table to table.
    ///  These allow recursively walking a graph of objects to identify reachable or unreachable rows.
    /// </summary>
    internal interface ICollector
    {
        string ColumnName { get; }
        TableCollector Collector { get; }
        long AddRow(int index);
    }

    internal struct RefColumnCollector : ICollector
    {
        public string ColumnName { get; }
        public RefColumn Column { get; }
        public TableCollector Collector { get; }

        public RefColumnCollector(string columnName, RefColumn column, TableCollector collector)
        {
            ColumnName = columnName;
            Column = column;
            Collector = collector;
        }

        public long AddRow(int index)
        {
            int targetIndex = Column[index];
            if (targetIndex >= 0)
            {
                return Collector.AddRow(targetIndex);
            }

            return 0;
        }
    }

    internal struct RefListColumnCollector : ICollector
    {
        public string ColumnName { get; }
        public RefListColumn Column { get; }
        public TableCollector Collector { get; }

        public RefListColumnCollector(string columnName, RefListColumn column, TableCollector collector)
        {
            ColumnName = columnName;
            Column = column;
            Collector = collector;
        }

        public long AddRow(int index)
        {
            long sum = 0;

            foreach (int targetIndex in Column.Values[index])
            {
                sum += Collector.AddRow(targetIndex);
            }

            return sum;
        }
    }
}