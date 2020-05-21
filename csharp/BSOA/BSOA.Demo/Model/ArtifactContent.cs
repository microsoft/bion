using BSOA.Model;
using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'ArtifactContent'
    /// </summary>
    public partial class ArtifactContent : IRow
    {
        private ArtifactContentTable _table;
        private int _index;

        internal ArtifactContent(ArtifactContentTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ArtifactContent(ArtifactContentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public ArtifactContent(SarifLogBsoa database) : this(database.ArtifactContent)
        { }

        public ArtifactContent() : this(SarifLogBsoa.Current)
        { }

        public string Text
        {
            get => _table.Text[_index];
            set => _table.Text[_index] = value;
        }

        public string Binary
        {
            get => _table.Binary[_index];
            set => _table.Binary[_index] = value;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ArtifactContentTable)table;
            _index = index;
        }
        #endregion
    }
}
