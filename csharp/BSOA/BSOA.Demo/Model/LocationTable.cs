using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Location' entity.
    /// </summary>
    public partial class LocationTable : Table<Location>
    {
        internal SarifLogBsoa Database;

        internal IColumn<int> Id;
        internal RefColumn PhysicalLocation;
        internal RefListColumn LogicalLocations;
        internal RefColumn Message;
        internal RefListColumn Annotations;

        public LocationTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<int>(-1));
            PhysicalLocation = AddColumn(nameof(PhysicalLocation), new RefColumn(nameof(SarifLogBsoa.PhysicalLocation)));
            LogicalLocations = AddColumn(nameof(LogicalLocations), new RefListColumn(nameof(SarifLogBsoa.LogicalLocation)));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogBsoa.Message)));
            Annotations = AddColumn(nameof(Annotations), new RefListColumn(nameof(SarifLogBsoa.Region)));
        }

        public override Location Get(int index)
        {
            return (index == -1 ? null : new Location(this, index));
        }
    }
}
