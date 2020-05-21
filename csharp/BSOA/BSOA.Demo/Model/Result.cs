using BSOA.Model;
using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Result'
    /// </summary>
    public partial class Result : IRow
    {
        private ResultTable _table;
        private int _index;

        internal Result(ResultTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Result(ResultTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Result(SarifLogBsoa database) : this(database.Result)
        { }

        public Result() : this(SarifLogBsoa.Current)
        { }

        public Microsoft.CodeAnalysis.Sarif.BaselineState BaselineState
        {
            get => (Microsoft.CodeAnalysis.Sarif.BaselineState)_table.BaselineState[_index];
            set => _table.BaselineState[_index] = (int)value;
        }

        public string RuleId
        {
            get => _table.RuleId[_index];
            set => _table.RuleId[_index] = value;
        }

        public int RuleIndex
        {
            get => _table.RuleIndex[_index];
            set => _table.RuleIndex[_index] = value;
        }

        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        public IList<Location> Locations
        {
            get => _table.Database.Location.List(_table.Locations[_index]);
        }

        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ResultTable)table;
            _index = index;
        }
        #endregion
    }
}
