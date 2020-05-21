using System;
using System.Collections.Generic;
using System.ComponentModel;

using BSOA.Model;

using Newtonsoft.Json;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Tool'
    /// </summary>
    public partial class Tool : IRow
    {
        private ToolTable _table;
        private int _index;

        internal Tool(ToolTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Tool(ToolTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Tool(SarifLogBsoa database) : this(database.Tool)
        { }

        public Tool() : this(SarifLogBsoa.Current)
        { }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ToolComponent Driver
        {
            get => _table.Database.ToolComponent.Get(_table.Driver[_index]);
            set => _table.Driver[_index] = _table.Database.ToolComponent.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<ToolComponent> Extensionss
        {
            get => _table.Database.ToolComponent.List(_table.Extensionss[_index]);
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ToolTable)table;
            _index = index;
        }
        #endregion
    }
}
