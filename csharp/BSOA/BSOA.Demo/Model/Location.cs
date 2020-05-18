using BSOA.Column;
using BSOA.Model;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    public readonly struct Location
    {
        internal readonly LocationTable _table;
        internal readonly int _index;

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
            get => new NumberListConverter<LogicalLocation, LogicalLocationTable>(_table.LogicalLocations[_index], _table.Database.LogicalLocation, (table, index) => new LogicalLocation(table, index), (table, item) => item._index);
            set => new NumberListConverter<LogicalLocation, LogicalLocationTable>(_table.LogicalLocations[_index], _table.Database.LogicalLocation, (table, index) => new LogicalLocation(table, index), (table, item) => item._index).SetTo(value);
        }

        public Message Message
        {
            get => _table.Database.Message[_table.Message[_index]];
            set => _table.Message[_index] = value._index;
        }

        public IList<Region> Annotations
        {
            get => new NumberListConverter<Region, RegionTable>(_table.Annotations[_index], _table.Database.Region, (table, index) => new Region(table, index), (table, item) => item._index);
            set => new NumberListConverter<Region, RegionTable>(_table.Annotations[_index], _table.Database.Region, (table, index) => new Region(table, index), (table, item) => item._index).SetTo(value);
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
