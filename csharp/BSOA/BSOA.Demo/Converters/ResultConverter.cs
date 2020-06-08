using BSOA.Demo.Converters;
using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class ResultConverter
    {
        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Result expected, Model.Result actual)
        {
            if (expected == null) { return actual == null; }

            if (expected.BaselineState != actual.BaselineState) { return false; }
            if (expected.RuleId != actual.RuleId) { return false; }
            if (expected.RuleIndex != actual.RuleIndex) { return false; }

            if (!MessageConverter.Compare(expected.Message, actual.Message)) { return false; }
            if (!expected.Locations.CompareList(actual.Locations, LocationConverter.Compare)) { return false; }

            if (expected.Guid != actual.Guid) { return false; }

            return true;
        }
    }
}
