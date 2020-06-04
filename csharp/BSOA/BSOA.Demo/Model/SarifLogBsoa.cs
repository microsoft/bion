using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Database
    /// </summary>
    public partial class SarifLogBsoa : Database
    {
        internal static SarifLogBsoa Current { get; private set; }
        
        internal ArtifactTable Artifact { get; }
        internal ArtifactContentTable ArtifactContent { get; }
        internal ArtifactLocationTable ArtifactLocation { get; }
        internal LocationTable Location { get; }
        internal LogicalLocationTable LogicalLocation { get; }
        internal MessageTable Message { get; }
        internal PhysicalLocationTable PhysicalLocation { get; }
        internal RegionTable Region { get; }
        internal ResultTable Result { get; }
        internal RunTable Run { get; }
        internal ToolTable Tool { get; }
        internal ToolComponentTable ToolComponent { get; }
        internal RootTable Root { get; }

        public SarifLogBsoa()
        {
            Current = this;

            Artifact = AddTable(nameof(Artifact), new ArtifactTable(this));
            ArtifactContent = AddTable(nameof(ArtifactContent), new ArtifactContentTable(this));
            ArtifactLocation = AddTable(nameof(ArtifactLocation), new ArtifactLocationTable(this));
            Location = AddTable(nameof(Location), new LocationTable(this));
            LogicalLocation = AddTable(nameof(LogicalLocation), new LogicalLocationTable(this));
            Message = AddTable(nameof(Message), new MessageTable(this));
            PhysicalLocation = AddTable(nameof(PhysicalLocation), new PhysicalLocationTable(this));
            Region = AddTable(nameof(Region), new RegionTable(this));
            Result = AddTable(nameof(Result), new ResultTable(this));
            Run = AddTable(nameof(Run), new RunTable(this));
            Tool = AddTable(nameof(Tool), new ToolTable(this));
            ToolComponent = AddTable(nameof(ToolComponent), new ToolComponentTable(this));
            Root = AddTable(nameof(Root), new RootTable(this));
        }
    }
}
