using BSOA.Demo.Converters;
using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class LocationConverter
    {
        public static bool Compare(Microsoft.CodeAnalysis.Sarif.Location expected, Model.Location actual)
        {
            if (expected == null) { return actual == null; }

            if (expected.Id != actual.Id) { return false; }
            if (!PhysicalLocationConverter.Compare(expected.PhysicalLocation, actual.PhysicalLocation)) { return false; }
            if (!MessageConverter.Compare(expected.Message, actual.Message)) { return false; }
            if (!expected.LogicalLocations.CompareList(actual.LogicalLocations, LogicalLocationConverter.Compare)) { return false; }

            return true;
        }
    }
}
