using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class RegionConverter
    {
        public static Model.Region Convert(Microsoft.CodeAnalysis.Sarif.Region region, SarifLogBsoa toDatabase)
        {
            Model.Region result = new Model.Region(toDatabase);

            result.StartLine = region.StartLine;
            result.StartColumn = region.StartColumn;
            result.EndLine = region.EndLine;
            result.EndColumn = region.EndColumn;
            result.ByteOffset = region.ByteOffset;
            result.ByteLength = region.ByteLength;
            result.CharOffset = region.CharOffset;
            result.CharLength = region.CharLength;

            if (region.Snippet != null)
            {
                result.Snippet = ArtifactContentConverter.Convert(region.Snippet, toDatabase);
            }

            return result;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Region expected, Model.Region actual)
        {
            if (expected.StartLine != actual.StartLine) { return false; }
            if (expected.StartColumn != actual.StartColumn) { return false; }
            if (expected.EndLine != actual.EndLine) { return false; }
            if (expected.EndColumn != actual.EndColumn) { return false; }
            if (expected.ByteOffset != actual.ByteOffset) { return false; }
            if (expected.ByteLength != actual.ByteLength) { return false; }
            if (expected.CharOffset != actual.CharOffset) { return false; }
            if (expected.CharLength != actual.CharLength) { return false; }

            if (!ArtifactContentConverter.Compare(expected.Snippet, actual.Snippet)) { return false; }

            return true;
        }
    }
}
