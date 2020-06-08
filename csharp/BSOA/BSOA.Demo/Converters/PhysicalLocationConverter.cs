using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class PhysicalLocationConverter
    {
        public static bool Compare(Microsoft.CodeAnalysis.Sarif.PhysicalLocation expected, Model.PhysicalLocation actual)
        {
            if (expected == null) { return actual == null; }

            if (!ArtifactLocationConverter.Compare(expected.ArtifactLocation, actual.ArtifactLocation)) { return false; }
            if (!RegionConverter.Compare(expected.Region, actual.Region)) { return false; }
            if (!RegionConverter.Compare(expected.ContextRegion, actual.ContextRegion)) { return false; }

            return true;
        }
    }
}
