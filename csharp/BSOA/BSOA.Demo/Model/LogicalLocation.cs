using BSOA.Model;
using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'LogicalLocation'
    /// </summary>
    public partial class LogicalLocation : IRow
    {
        private LogicalLocationTable _table;
        private int _index;

        internal LogicalLocation(LogicalLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public LogicalLocation(LogicalLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public LogicalLocation(SarifLogBsoa database) : this(database.LogicalLocation)
        { }

        public LogicalLocation() : this(SarifLogBsoa.Current)
        { }

        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        public string FullyQualifiedName
        {
            get => _table.FullyQualifiedName[_index];
            set => _table.FullyQualifiedName[_index] = value;
        }

        public string DecoratedName
        {
            get => _table.DecoratedName[_index];
            set => _table.DecoratedName[_index] = value;
        }

        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        public string Kind
        {
            get => _table.Kind[_index];
            set => _table.Kind[_index] = value;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (LogicalLocationTable)table;
            _index = index;
        }
        #endregion
    }
}
