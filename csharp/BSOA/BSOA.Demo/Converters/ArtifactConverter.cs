using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class ArtifactConverter
    {
        public static Model.Artifact Convert(Microsoft.CodeAnalysis.Sarif.Artifact source, SarifLogBsoa toDatabase)
        {
            Model.Artifact result = new Model.Artifact(toDatabase.Artifact);

            if (source.Description != null)
            {
                result.Description = MessageConverter.Convert(source.Description, toDatabase);
            }

            if (source.Location != null)
            {
                result.Location = ArtifactLocationConverter.Convert(source.Location, toDatabase);
            }

            result.ParentIndex = source.ParentIndex;
            result.Offset = source.Offset;
            result.Length = source.Length;
            result.MimeType = source.MimeType;

            if (source.Contents != null)
            {
                result.Contents = ArtifactContentConverter.Convert(source.Contents, toDatabase);
            }

            result.Encoding = source.Encoding;
            result.SourceLanguage = source.SourceLanguage;
            result.LastModifiedTimeUtc = source.LastModifiedTimeUtc;

            return result;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Artifact expected, Model.Artifact actual)
        {
            if (expected == null) { return actual == null; }

            if (!MessageConverter.Compare(expected.Description, actual.Description)) { return false; }
            if (!ArtifactLocationConverter.Compare(expected.Location, actual.Location)) { return false; }

            if (expected.ParentIndex != actual.ParentIndex) { return false; }
            if (expected.Offset != actual.Offset) { return false; }
            if (expected.Length != actual.Length) { return false; }
            if (expected.MimeType != actual.MimeType) { return false; }

            if (!ArtifactContentConverter.Compare(expected.Contents, actual.Contents)) { return false; }

            if (expected.Encoding != actual.Encoding) { return false; }
            if (expected.SourceLanguage != actual.SourceLanguage) { return false; }
            if (expected.LastModifiedTimeUtc != actual.LastModifiedTimeUtc) { return false; }

            return true;
        }
    }
}
