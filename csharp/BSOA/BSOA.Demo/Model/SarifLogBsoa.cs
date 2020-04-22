using BSOA.Model;

namespace BSOA.Demo.Model
{
    public class SarifLogBsoa : Database
    {
        public RegionTable Region { get; }
        public ArtifactContentTable ArtifactContent { get; }

        public SarifLogBsoa() : base()
        {
            Region = AddTable(nameof(Region), new RegionTable(this));
            ArtifactContent = AddTable(nameof(ArtifactContent), new ArtifactContentTable(this));
        }
    }
}
