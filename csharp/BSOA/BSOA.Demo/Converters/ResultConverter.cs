using BSOA.Demo.Converters;
using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class ResultConverter
    {
        public static Model.Result Convert(Microsoft.CodeAnalysis.Sarif.Result source, SarifLogBsoa toDatabase)
        {
            Model.Result result = new Model.Result(toDatabase.Result);

            result.RuleId = source.RuleId;
            result.RuleIndex = source.RuleIndex;

            if (source.Message != null)
            {
                result.Message = MessageConverter.Convert(source.Message, toDatabase);
            }

            result.Locations.ConvertList(source.Locations, toDatabase, (item, db) => LocationConverter.Convert(item, db));
            result.Guid = source.Guid;

            return result;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Result expected, Model.Result actual)
        {
            if (expected == null) { return actual.IsNull; }

            if (expected.RuleId != actual.RuleId) { return false; }
            if (expected.RuleIndex != actual.RuleIndex) { return false; }

            if (!MessageConverter.Compare(expected.Message, actual.Message)) { return false; }
            if (!expected.Locations.CompareList(actual.Locations, LocationConverter.Compare)) { return false; }

            if (expected.Guid != actual.Guid) { return false; }

            return true;
        }
    }
}
