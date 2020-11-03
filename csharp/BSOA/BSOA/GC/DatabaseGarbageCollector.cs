using BSOA.Column;
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
    ///  - Remove all rows not reachable from the root, so they aren't serialized out.
    ///  - Update all Ref and RefList columns so that they still point to the same logical row.
    ///  - Update all OM instances in memory so that they still point to the same logical row.
    ///  - Copy data for OM instances which aren't reachable to a temporary database, so the OM object looks the same.
    /// </summary>
    internal class DatabaseCollector
    {
        public IDatabase Database { get; }

        private Func<IDatabase> _tempBuilder;
        private IDatabase _tempDatabase;
        public IDatabase TempDatabase => _tempDatabase ??= _tempBuilder();

        private Dictionary<string, TableCollector> _tableCollectors;
        private string _rootTableName;

        public DatabaseCollector(IDatabase database)
        {
            Database = database;
            _tempBuilder = () => (IDatabase)Activator.CreateInstance(database.GetType());

            _tableCollectors = new Dictionary<string, TableCollector>();
            _rootTableName = database.RootTableName;

            // 1. Build a Collector for each Table
            foreach (var table in database.Tables)
            {
                _tableCollectors[table.Key] = new TableCollector(this, table.Value, table.Key);
            }

            // 2. Hook up ref columns between the source and target table collectors
            foreach (var table in database.Tables)
            {
                string tableName = table.Key;
                TableCollector sourceCollector = _tableCollectors[tableName];

                foreach (var column in table.Value.Columns.Values)
                {
                    IRefColumn refColumn = column as IRefColumn;
                    if (refColumn != null)
                    {
                        sourceCollector.AddRefColumn(refColumn, _tableCollectors[refColumn.ReferencedTableName]);
                    }
                }
            }
        }

        public bool Collect()
        {
            foreach (TableCollector collector in _tableCollectors.Values)
            {
                collector.PrepareToCollect();
            }

            // Walk reachable rows (add root, which will recursively add everything reachable)
            _tableCollectors[_rootTableName].AddRow(0);

            // Identify and clean up any unused rows from each table
            bool dataRemoved = false;
            foreach (TableCollector collector in _tableCollectors.Values)
            {
                dataRemoved |= collector.Collect();
            }

            return dataRemoved;
        }
    }

    /// <summary>
    ///  GarbageCollection is built on ICollectors, which 'mark' rows which are
    ///  reachable from the database root.
    /// </summary>
    internal interface ICollector
    {
        void AddRow(int index);
    }

    internal class TableCollector : ICollector
    {
        private DatabaseCollector _databaseCollector;

        // Table this Collector is assigned to (so it can Swap and Remove to clean unused rows)
        private ITable _table;
        private string _tableName;

        // List of columns from this table to other tables (to walk reachable graph)
        private List<ICollector> _refsFromTable;

        // List of columns from other tables to this table (to remap indices of Swapped rows)
        private List<IRefColumn> _refsToTable;

        // During collection, tracking rows in the table reachable from the root object
        private bool[] _isRowReachable;

        public TableCollector(DatabaseCollector databaseCollector, ITable table, string tableName)
        {
            _databaseCollector = databaseCollector;
            _table = table;
            _tableName = tableName;
        }

        public void AddRefColumn(IRefColumn column, TableCollector target)
        {
            if (_refsFromTable == null) { _refsFromTable = new List<ICollector>(); }
            if (target._refsToTable == null) { target._refsToTable = new List<IRefColumn>(); }

            // Add column to 'RefsTo' in the target (for remapping indices)
            target._refsToTable.Add(column);

            // Add column to 'RefsFrom' in the source (for walking reachable indices)
            if (column is RefColumn)
            {
                _refsFromTable.Add(new RefColumnCollector((RefColumn)column, target));
            }
            else if (column is RefListColumn)
            {
                _refsFromTable.Add(new RefListColumnCollector((RefListColumn)column, target));
            }
            else
            {
                throw new NotImplementedException($"IRefColumn of type {column.GetType().Name} not supported in TableCollector.Add()");
            }
        }

        public void PrepareToCollect()
        {
            _isRowReachable = new bool[_table.Count];
        }

        public void AddRow(int index)
        {
            if (_isRowReachable[index] == false)
            {
                _isRowReachable[index] = true;

                if (_refsFromTable != null)
                {
                    foreach (ICollector collector in _refsFromTable)
                    {
                        collector.AddRow(index);
                    }
                }
            }
        }

        public bool Collect()
        {
            IColumn values = _table;
            IRemapper<int> remapper = IntRemapper.Instance;

            // TODO: Deduplicate with GarbageCollector.Collect().

            // If there are unused values, ...
            List<int> unusedValues = new List<int>();
            for (int i = 0; i < _isRowReachable.Length; ++i)
            {
                // PERF RISK: Need faster way to find unused values; pivot to BitVector and add vectorized mechanism?
                if (!_isRowReachable[i]) { unusedValues.Add(i); }
            }

            _isRowReachable = null;
            int[] remapped = unusedValues.ToArray();

            if (remapped.Length > 0)
            {
                int remapFrom = (values.Count - remapped.Length);

                Revise(_table, remapFrom, remapped);

                // Swap the *values* to the end of the values array
                for (int i = 0; i < remapped.Length; ++i)
                {
                    values.Swap(remapFrom + i, remapped[i]);
                }

                // Remove the unused values that are now at the end of the array
                values.RemoveFromEnd(remapped.Length);

                // Ensure count of 'latest' table is *also* updated to be correct
                _databaseCollector.Database.Tables[_tableName].RemoveFromEnd(remapped.Length);

                // Trim values afterward to clean up any newly unused space
                values.Trim();

                // Remap indices from all tables which point to this one to use the updated indices
                if (_refsToTable != null)
                {
                    foreach (INumberColumn<int> refToTable in _refsToTable)
                    {
                        refToTable.ForEach((slice) => remapper.RemapAbove(slice, remapFrom, remapped));
                    }
                }
            }

            // Return whether anything was remapped
            return remapped.Length > 0;
        }

        public void Revise(ITable current, int remapFrom, int[] remapped)
        {
            // ISSUE: Removed rows will be cloned multiple times; once for the row in the table, and again as each reference is recursively cloned.

            // Construct a new Table tied to the existing database and *unchanged* columns
            IDatabase database = _databaseCollector.Database;
            Dictionary<string, IColumn> latestColumns = new Dictionary<string, IColumn>(current.Columns);
            ITable latest = (ITable)Activator.CreateInstance(current.GetType(), database, latestColumns);

            // Replace Table instance in Database with new one
            database.Tables[_tableName] = latest;
            database.GetOrBuildTables();

            // Get the temporary copy of this table
            ITable temp = _databaseCollector.TempDatabase.Tables[_tableName];

            // Build a RowUpdater to redirect object model instances to the temp or latest table copies
            RowUpdater updater = new RowUpdater(latest, temp);

            // For each removed item...
            for (int i = 0; i < remapped.Length; ++i)
            {
                // Copy the item to the temp table
                int tempTableIndex = temp.Count;
                temp.CopyItem(tempTableIndex, current, remapped[i]);

                // Tell the updater that the item-to-remove is in the temp table
                updater.AddMapping(remapped[i], tempTableIndex, movedToTemp: true);

                // Tell the updater that the item swapped in for the removed item was swapped
                updater.AddMapping(remapFrom + i, remapped[i], movedToTemp: false);
            }

            // Wrap each column on the current table with an UpdatingColumn
            List<string> columnNames = current.Columns.Keys.ToList();
            foreach (string columnName in columnNames)
            {
                current.Columns[columnName] = WrapColumn(current.Columns[columnName], temp.Columns[columnName], updater);
            }

            current.GetOrBuildColumns();
        }

        private static IColumn WrapColumn(IColumn inner, IColumn temp, RowUpdater updater)
        {
            Type innerType = inner.Type;
            return (IColumn)(typeof(UpdatingColumn<>).MakeGenericType(innerType).GetConstructor(new[] { typeof(IColumn), typeof(IColumn), typeof(RowUpdater) }).Invoke(new object[] { inner, temp, updater }));
        }
    }

    internal struct RefColumnCollector : ICollector
    {
        public RefColumn Column { get; }
        public TableCollector Collector { get; }

        public RefColumnCollector(RefColumn column, TableCollector collector)
        {
            Column = column;
            Collector = collector;
        }

        public void AddRow(int index)
        {
            int targetIndex = Column[index];
            if (targetIndex >= 0)
            {
                Collector.AddRow(targetIndex);
            }
        }
    }

    internal struct RefListColumnCollector : ICollector
    {
        public RefListColumn Column { get; }
        public TableCollector Collector { get; }

        public RefListColumnCollector(RefListColumn column, TableCollector collector)
        {
            Column = column;
            Collector = collector;
        }

        public void AddRow(int index)
        {
            foreach (int targetIndex in Column.Values[index])
            {
                Collector.AddRow(targetIndex);
            }
        }
    }
}