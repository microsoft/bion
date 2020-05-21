using System;
using System.Collections.Generic;
using System.ComponentModel;

using BSOA.Model;

using Newtonsoft.Json;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'ToolComponent'
    /// </summary>
    public partial class ToolComponent : IRow
    {
        private ToolComponentTable _table;
        private int _index;

        internal ToolComponent(ToolComponentTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ToolComponent(ToolComponentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public ToolComponent(SarifLogBsoa database) : this(database.ToolComponent)
        { }

        public ToolComponent() : this(SarifLogBsoa.Current)
        { }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ToolComponentTable)table;
            _index = index;
        }
        #endregion
    }
}
