using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class ArtifactContentConverter
    {
        public static Model.ArtifactContent Convert(Microsoft.CodeAnalysis.Sarif.ArtifactContent source, SarifLogBsoa toDatabase)
        {
            Model.ArtifactContent result = new Model.ArtifactContent(toDatabase.ArtifactContent);

            result.Text = source.Text;
            result.Binary = source.Binary;

            return result;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.ArtifactContent expected, Model.ArtifactContent actual)
        {
            if (expected == null) { return actual == null; }

            if (expected.Text != actual.Text) { return false; }
            if (expected.Binary != actual.Binary) { return false; }

            return true;
        }
    }
}
