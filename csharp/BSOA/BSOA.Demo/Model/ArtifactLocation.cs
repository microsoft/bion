using System;
using System.Collections.Generic;
using System.ComponentModel;

using BSOA.Model;

using Newtonsoft.Json;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'ArtifactLocation'
    /// </summary>
    public partial class ArtifactLocation : IRow
    {
        private ArtifactLocationTable _table;
        private int _index;

        internal ArtifactLocation(ArtifactLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ArtifactLocation(ArtifactLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public ArtifactLocation(SarifLogBsoa database) : this(database.ArtifactLocation)
        { }

        public ArtifactLocation() : this(SarifLogBsoa.Current)
        { }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Uri Uri
        {
            get => _table.Uri[_index];
            set => _table.Uri[_index] = value;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string UriBaseId
        {
            get => _table.UriBaseId[_index];
            set => _table.UriBaseId[_index] = value;
        }

        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Message Description
        {
            get => _table.Database.Message.Get(_table.Description[_index]);
            set => _table.Description[_index] = _table.Database.Message.LocalIndex(value);
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ArtifactLocationTable)table;
            _index = index;
        }
        #endregion
    }
}
