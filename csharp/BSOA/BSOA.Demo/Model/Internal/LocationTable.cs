using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Location'
    /// </summary>
    internal partial class LocationTable : Table<Location>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> Id;
        internal RefColumn PhysicalLocation;
        internal RefListColumn LogicalLocations;
        internal RefColumn Message;
        internal RefListColumn Annotations;

        internal LocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<int>(-1));
            PhysicalLocation = AddColumn(nameof(PhysicalLocation), new RefColumn(nameof(SarifLogDatabase.PhysicalLocation)));
            LogicalLocations = AddColumn(nameof(LogicalLocations), new RefListColumn(nameof(SarifLogDatabase.LogicalLocation)));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            Annotations = AddColumn(nameof(Annotations), new RefListColumn(nameof(SarifLogDatabase.Region)));
        }

        public override Location Get(int index)
        {
            return (index == -1 ? null : new Location(this, index));
        }
    }
}
