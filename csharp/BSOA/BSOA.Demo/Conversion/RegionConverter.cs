using BSOA.Demo.Model;
using Microsoft.CodeAnalysis.Sarif;

namespace BSOA.Demo.Conversion
{
    public class RegionConverter
    {
        public static Region4 Convert(Region region, RegionTable toTable)
        {
            Region4 result = toTable.Add();

            result.StartLine = region.StartLine;
            result.StartColumn = region.StartColumn;
            result.EndLine = region.EndLine;
            result.EndColumn = region.EndColumn;
            result.ByteOffset = region.ByteOffset;
            result.ByteLength = region.ByteLength;
            result.CharOffset = region.CharOffset;
            result.CharLength = region.CharLength;

            return result;
        }

        public static bool Compare(Region expected, Region4 actual)
        {
            if (expected.StartLine != actual.StartLine) { return false; }
            if (expected.StartColumn != actual.StartColumn) { return false; }
            if (expected.EndLine != actual.EndLine) { return false; }
            if (expected.EndColumn != actual.EndColumn) { return false; }
            if (expected.ByteOffset != actual.ByteOffset) { return false; }
            if (expected.ByteLength != actual.ByteLength) { return false; }
            if (expected.CharOffset != actual.CharOffset) { return false; }
            if (expected.CharLength != actual.CharLength) { return false; }

            return true;
        }
    }
}
