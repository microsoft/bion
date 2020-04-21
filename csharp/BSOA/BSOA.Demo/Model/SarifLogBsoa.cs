using BSOA.Model;

namespace BSOA.Demo.Model
{
    public class SarifLogBsoa : Database
    {
        public RegionTable Regions { get; }

        public SarifLogBsoa() : base()
        {
            Regions = AddTable(nameof(Regions), new RegionTable(this));
        }
    }
}
