using BSOA.Demo.Converters;
using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class LocationConverter
    {
        public static Model.Location Convert(Microsoft.CodeAnalysis.Sarif.Location source, SarifLogBsoa toDatabase)
        {
            Model.Location result = new Model.Location(toDatabase);

            result.Id = source.Id;

            if (source.PhysicalLocation != null)
            {
                result.PhysicalLocation = PhysicalLocationConverter.Convert(source.PhysicalLocation, toDatabase);
            }

            if (source.Message != null)
            {
                result.Message = MessageConverter.Convert(source.Message, toDatabase);
            }

            result.LogicalLocations.ConvertList(source.LogicalLocations, toDatabase, LogicalLocationConverter.Convert);

            return result;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Location expected, Model.Location actual)
        {
            if (expected == null) { return actual.IsNull; }

            if (expected.Id != actual.Id) { return false; }
            if (!PhysicalLocationConverter.Compare(expected.PhysicalLocation, actual.PhysicalLocation)) { return false; }
            if (!MessageConverter.Compare(expected.Message, actual.Message)) { return false; }
            if (!expected.LogicalLocations.CompareList(actual.LogicalLocations, LogicalLocationConverter.Compare)) { return false; }

            return true;
        }
    }
}
