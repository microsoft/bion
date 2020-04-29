using BSOA.Model;

namespace BSOA.Demo.Model
{
    public class SarifLogBsoa : Database
    {
        public ArtifactTable Artifact { get; }
        public ArtifactContentTable ArtifactContent { get; }
        public ArtifactLocationTable ArtifactLocation { get; }
        public LocationTable Location { get; }
        public LogicalLocationTable LogicalLocation { get; }
        public MessageTable Message { get; }
        public PhysicalLocationTable PhysicalLocation { get; }
        public RegionTable Region { get; }
        public ResultTable Result { get; }
        public RunTable Run { get; }

        public SarifLogBsoa() : base()
        {
            Artifact = AddTable(nameof(Artifact), new ArtifactTable(this));
            ArtifactContent = AddTable(nameof(ArtifactContent), new ArtifactContentTable(this));
            ArtifactLocation = AddTable(nameof(ArtifactLocation), new ArtifactLocationTable(this));
            LogicalLocation = AddTable(nameof(LogicalLocation), new LogicalLocationTable(this));
            Location = AddTable(nameof(Location), new LocationTable(this));
            Message = AddTable(nameof(Message), new MessageTable(this));
            PhysicalLocation = AddTable(nameof(PhysicalLocation), new PhysicalLocationTable(this));
            Region = AddTable(nameof(Region), new RegionTable(this));
            Result = AddTable(nameof(Result), new ResultTable(this));
            Run = AddTable(nameof(Run), new RunTable(this));
        }

        public override string ToString()
        {
            return $"{Location.Count:n0} {nameof(Location)}s";
        }
    }
}
