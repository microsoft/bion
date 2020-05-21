using BSOA.Model;
using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Artifact'
    /// </summary>
    public partial class Artifact : IRow
    {
        private ArtifactTable _table;
        private int _index;

        internal Artifact(ArtifactTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Artifact(ArtifactTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Artifact(SarifLogBsoa database) : this(database.Artifact)
        { }

        public Artifact() : this(SarifLogBsoa.Current)
        { }

        public Message Description
        {
            get => _table.Database.Message.Get(_table.Description[_index]);
            set => _table.Description[_index] = _table.Database.Message.LocalIndex(value);
        }

        public ArtifactLocation Location
        {
            get => _table.Database.ArtifactLocation.Get(_table.Location[_index]);
            set => _table.Location[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        public int Offset
        {
            get => _table.Offset[_index];
            set => _table.Offset[_index] = value;
        }

        public int Length
        {
            get => _table.Length[_index];
            set => _table.Length[_index] = value;
        }

        public string MimeType
        {
            get => _table.MimeType[_index];
            set => _table.MimeType[_index] = value;
        }

        public ArtifactContent Contents
        {
            get => _table.Database.ArtifactContent.Get(_table.Contents[_index]);
            set => _table.Contents[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        public string Encoding
        {
            get => _table.Encoding[_index];
            set => _table.Encoding[_index] = value;
        }

        public string SourceLanguage
        {
            get => _table.SourceLanguage[_index];
            set => _table.SourceLanguage[_index] = value;
        }

        public DateTime LastModifiedTimeUtc
        {
            get => _table.LastModifiedTimeUtc[_index];
            set => _table.LastModifiedTimeUtc[_index] = value;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ArtifactTable)table;
            _index = index;
        }
        #endregion
    }
}
