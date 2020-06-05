using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'PhysicalLocation'
    /// </summary>
    internal partial class PhysicalLocationTable : Table<PhysicalLocation>
    {
        internal SarifLogDatabase Database;

        internal RefColumn ArtifactLocation;
        internal RefColumn Region;
        internal RefColumn ContextRegion;

        internal PhysicalLocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            ArtifactLocation = AddColumn(nameof(ArtifactLocation), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Region = AddColumn(nameof(Region), new RefColumn(nameof(SarifLogDatabase.Region)));
            ContextRegion = AddColumn(nameof(ContextRegion), new RefColumn(nameof(SarifLogDatabase.Region)));
        }

        public override PhysicalLocation Get(int index)
        {
            return (index == -1 ? null : new PhysicalLocation(this, index));
        }
    }
}
