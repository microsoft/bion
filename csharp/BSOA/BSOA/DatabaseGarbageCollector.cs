using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BSOA
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
        private Dictionary<string, TableCollector> _tableCollectors;
        private string _rootTableName;

        public DatabaseCollector(Database database)
        {
            _tableCollectors = new Dictionary<string, TableCollector>();
            _rootTableName = database.RootTableName;

            // 1. Build a Collector for each Table
            foreach (var table in database.Tables)
            {
                _tableCollectors[table.Key] = new TableCollector(table.Value);
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

    public class TableCollector : ICollector
    {
        // Table this Collector is assigned to (so it can Swap and Remove to clean unused rows)
        private ITable _table;

        // List of columns from this table to other tables (to walk reachable graph)
        private List<ICollector> _refsFromTable;

        // List of columns from other tables to this table (to remap indices of Swapped rows)
        private List<IRefColumn> _refsToTable;

        // During collection, tracking rows in the table reachable from the root object
        private bool[] _isRowReachable;

        public TableCollector(ITable table)
        {
            _table = table;
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

                // Swap the *values* to the end of the values array
                for (int i = 0; i < remapped.Length; ++i)
                {
                    values.Swap(remapFrom + i, remapped[i]);
                }

                // TODO: Update object model instances whose rows have been swapped
                // TODO: Copy object model instances which were orphaned to a temporary database

                // Remove the unused values that are now at the end of the array
                values.RemoveFromEnd(remapped.Length);

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