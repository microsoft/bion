using BSOA.Demo.Model;
using Microsoft.CodeAnalysis.Sarif;

namespace BSOA.Demo.Conversion
{
    public class RegionConverter
    {
        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Region expected, Model.Region actual)
        {
            if(expected == null) { return actual == null; }

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
