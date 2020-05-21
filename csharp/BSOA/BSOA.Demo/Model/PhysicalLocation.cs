using System;
using System.Collections.Generic;
using System.ComponentModel;

using BSOA.Model;

using Newtonsoft.Json;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'PhysicalLocation'
    /// </summary>
    public partial class PhysicalLocation : IRow
    {
        private PhysicalLocationTable _table;
        private int _index;

        internal PhysicalLocation(PhysicalLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public PhysicalLocation(PhysicalLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public PhysicalLocation(SarifLogBsoa database) : this(database.PhysicalLocation)
        { }

        public PhysicalLocation() : this(SarifLogBsoa.Current)
        { }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ArtifactLocation ArtifactLocation
        {
            get => _table.Database.ArtifactLocation.Get(_table.ArtifactLocation[_index]);
            set => _table.ArtifactLocation[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Region Region
        {
            get => _table.Database.Region.Get(_table.Region[_index]);
            set => _table.Region[_index] = _table.Database.Region.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Region ContextRegion
        {
            get => _table.Database.Region.Get(_table.ContextRegion[_index]);
            set => _table.ContextRegion[_index] = _table.Database.Region.LocalIndex(value);
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (PhysicalLocationTable)table;
            _index = index;
        }
        #endregion
    }
}
