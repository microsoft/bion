using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'PhysicalLocation' entity.
    /// </summary>
    public partial class PhysicalLocationTable : Table<PhysicalLocation>
    {
        internal SarifLogBsoa Database;

        internal RefColumn ArtifactLocation;
        internal RefColumn Region;
        internal RefColumn ContextRegion;

        public PhysicalLocationTable(SarifLogBsoa database) : base()
        {
            Database = database;

            ArtifactLocation = AddColumn(nameof(ArtifactLocation), new RefColumn(nameof(SarifLogBsoa.ArtifactLocation)));
            Region = AddColumn(nameof(Region), new RefColumn(nameof(SarifLogBsoa.Region)));
            ContextRegion = AddColumn(nameof(ContextRegion), new RefColumn(nameof(SarifLogBsoa.Region)));
        }

        public override PhysicalLocation Get(int index)
        {
            return (index == -1 ? null : new PhysicalLocation(this, index));
        }
    }
}
