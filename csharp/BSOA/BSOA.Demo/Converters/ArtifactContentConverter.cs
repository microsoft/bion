using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class ArtifactContentConverter
    {
        public static bool Compare(Microsoft.CodeAnalysis.Sarif.ArtifactContent expected, Model.ArtifactContent actual)
        {
            if (expected == null) { return actual == null; }

            if (expected.Text != actual.Text) { return false; }
            if (expected.Binary != actual.Binary) { return false; }

            return true;
        }
    }
}
