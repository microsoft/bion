using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class ArtifactLocationConverter
    {
        public static bool Compare(Microsoft.CodeAnalysis.Sarif.ArtifactLocation expected, Model.ArtifactLocation actual)
        {
            if (expected == null) { return actual == null; }

            if (expected.Uri != actual.Uri) { return false; }
            if (expected.UriBaseId != actual.UriBaseId) { return false; }
            if (expected.Index != actual.Index) { return false; }
            if (!MessageConverter.Compare(expected.Description, actual.Description)) { return false; }

            return true;
        }
    }
}
