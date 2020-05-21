using System;
using System.Collections.Generic;
using System.ComponentModel;

using BSOA.Model;

using Newtonsoft.Json;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Run'
    /// </summary>
    public partial class Run : IRow
    {
        private RunTable _table;
        private int _index;

        internal Run(RunTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Run(RunTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Run(SarifLogBsoa database) : this(database.Run)
        { }

        public Run() : this(SarifLogBsoa.Current)
        { }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Tool Tool
        {
            get => _table.Database.Tool.Get(_table.Tool[_index]);
            set => _table.Tool[_index] = _table.Database.Tool.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Artifact> Artifacts
        {
            get => _table.Database.Artifact.List(_table.Artifacts[_index]);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Result> Results
        {
            get => _table.Database.Result.List(_table.Results[_index]);
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (RunTable)table;
            _index = index;
        }
        #endregion
    }
}
