using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class ArtifactLocationConverter
    {
        public static Model.ArtifactLocation Convert(Microsoft.CodeAnalysis.Sarif.ArtifactLocation source, SarifLogBsoa toDatabase)
        {
            Model.ArtifactLocation result = new Model.ArtifactLocation(toDatabase);

            result.Uri = source.Uri;
            result.UriBaseId = source.UriBaseId;
            result.Index = source.Index;
            
            if(source.Description != null)
            {
                result.Description = MessageConverter.Convert(source.Description, toDatabase);
            }

            // Properties
            
            return result;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.ArtifactLocation expected, Model.ArtifactLocation actual)
        {
            if (expected == null) { return actual.IsNull; }

            if (expected.Uri != actual.Uri) { return false; }
            if (expected.UriBaseId != actual.UriBaseId) { return false; }
            if (expected.Index != actual.Index) { return false; }
            if (!MessageConverter.Compare(expected.Description, actual.Description)) { return false; }

            return true;
        }
    }
}
