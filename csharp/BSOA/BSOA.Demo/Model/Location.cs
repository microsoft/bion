using BSOA.Column;
using BSOA.Model;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    public struct Location
    {
        internal LocationTable _table;
        internal int _index;

        public Location(LocationTable table, int index)
        {
            _table = table;
            _index = index;
        }

        public Location(LocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Location(SarifLogBsoa database) : this(database.Location)
        { }

        public bool IsNull => (_table == null || _index < 0);

        public int Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        public PhysicalLocation PhysicalLocation
        {
            get => _table.Database.PhysicalLocation[_table.PhysicalLocation[_index]];
            set => _table.PhysicalLocation[_index] = value._index;
        }

        public IList<LogicalLocation> LogicalLocations
        {
            get => new MutableSliceWrapper<LogicalLocation, LogicalLocationTable>(_table.LogicalLocations[_index], _table.Database.LogicalLocation, (table, index) => new LogicalLocation(table, index), (item) => item._index);
            set => new MutableSliceWrapper<LogicalLocation, LogicalLocationTable>(_table.LogicalLocations[_index], _table.Database.LogicalLocation, (table, index) => new LogicalLocation(table, index), (item) => item._index).SetTo(value);
        }

        public Message Message
        {
            get => _table.Database.Message[_table.Message[_index]];
            set => _table.Message[_index] = value._index;
        }

        public IList<Region> Annotations
        {
            get => new MutableSliceWrapper<Region, RegionTable>(_table.Annotations[_index], _table.Database.Region, (table, index) => new Region(table, index), (item) => item._index);
            set => new MutableSliceWrapper<Region, RegionTable>(_table.Annotations[_index], _table.Database.Region, (table, index) => new Region(table, index), (item) => item._index).SetTo(value);
        }
    }

    public class LocationTable : Table<Location>
    {
        internal SarifLogBsoa Database;

        internal NumberColumn<int> Id;
        internal RefColumn PhysicalLocation;
        internal RefListColumn LogicalLocations;
        internal RefColumn Message;
        internal RefListColumn Annotations;

        // RefListColumn Relationships
        // Properties

        public LocationTable(SarifLogBsoa database) : base()
        {
            this.Database = database;

            this.Id = AddColumn(nameof(Id), new NumberColumn<int>(-1));
            this.PhysicalLocation = AddColumn(nameof(PhysicalLocation), new RefColumn(nameof(database.PhysicalLocation)));
            this.LogicalLocations = AddColumn(nameof(LogicalLocations), new RefListColumn(nameof(database.LogicalLocation)));
            this.Message = AddColumn(nameof(Message), new RefColumn(nameof(database.Message)));
            this.Annotations = AddColumn(nameof(Annotations), new RefListColumn(nameof(database.Region)));
        }

        public override Location this[int index] => new Location(this, index);
    }

}
