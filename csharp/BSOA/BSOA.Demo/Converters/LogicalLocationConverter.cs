using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class LogicalLocationConverter
    {
        public static bool Compare(Microsoft.CodeAnalysis.Sarif.LogicalLocation expected, Model.LogicalLocation actual)
        {
            if (expected == null) { return actual == null; }

            if (expected.DecoratedName != actual.DecoratedName) { return false; }
            if (expected.FullyQualifiedName != actual.FullyQualifiedName) { return false; }
            if (expected.Index != actual.Index) { return false; }
            if (expected.Kind != actual.Kind) { return false; }
            if (expected.Name != actual.Name) { return false; }

            return true;
        }
    }
}
