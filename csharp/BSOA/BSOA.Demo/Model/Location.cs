using BSOA.Model;
using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Location'
    /// </summary>
    public partial class Location : IRow
    {
        private LocationTable _table;
        private int _index;

        internal Location(LocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Location(LocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Location(SarifLogBsoa database) : this(database.Location)
        { }

        public Location() : this(SarifLogBsoa.Current)
        { }

        public int Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        public PhysicalLocation PhysicalLocation
        {
            get => _table.Database.PhysicalLocation.Get(_table.PhysicalLocation[_index]);
            set => _table.PhysicalLocation[_index] = _table.Database.PhysicalLocation.LocalIndex(value);
        }

        public IList<LogicalLocation> LogicalLocations
        {
            get => _table.Database.LogicalLocation.List(_table.LogicalLocations[_index]);
        }

        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        public IList<Region> Annotations
        {
            get => _table.Database.Region.List(_table.Annotations[_index]);
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (LocationTable)table;
            _index = index;
        }
        #endregion
    }
}
