using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class PhysicalLocationConverter
    {
        public static Model.PhysicalLocation Convert(Microsoft.CodeAnalysis.Sarif.PhysicalLocation source, SarifLogBsoa toDatabase)
        {
            Model.PhysicalLocation result = new Model.PhysicalLocation(toDatabase);

            if (source.ArtifactLocation != null)
            {
                result.ArtifactLocation = ArtifactLocationConverter.Convert(source.ArtifactLocation, toDatabase);
            }

            if (source.Region != null)
            {
                result.Region = RegionConverter.Convert(source.Region, toDatabase);
            }

            if (source.ContextRegion != null)
            {
                result.ContextRegion = RegionConverter.Convert(source.ContextRegion, toDatabase);
            }

            // Address, Properties

            return result;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.PhysicalLocation expected, Model.PhysicalLocation actual)
        {
            if (expected == null) { return actual.IsNull; }

            if (!ArtifactLocationConverter.Compare(expected.ArtifactLocation, actual.ArtifactLocation)) { return false; }
            if (!RegionConverter.Compare(expected.Region, actual.Region)) { return false; }
            if (!RegionConverter.Compare(expected.ContextRegion, actual.ContextRegion)) { return false; }

            return true;
        }
    }
}
