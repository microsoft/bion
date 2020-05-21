using BSOA.Model;
using System;
using System.Collections.Generic;

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

        public IList<Result> Results
        {
            get => _table.Database.Result.List(_table.Results[_index]);
        }

        public IList<Artifact> Artifacts
        {
            get => _table.Database.Artifact.List(_table.Artifacts[_index]);
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
