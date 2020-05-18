using BSOA.Demo.Model;

namespace BSOA.Demo.Conversion
{
    public class LogicalLocationConverter
    {
        public static Model.LogicalLocation Convert(Microsoft.CodeAnalysis.Sarif.LogicalLocation source, SarifLogBsoa toDatabase)
        {
            Model.LogicalLocation result = new Model.LogicalLocation(toDatabase);

            result.DecoratedName = source.DecoratedName;
            result.FullyQualifiedName = source.FullyQualifiedName;
            result.Index = source.Index;
            result.Kind = source.Kind;
            result.Name = source.Name;
            result.ParentIndex = source.ParentIndex;

            return result;
        }

        public static bool Compare(Microsoft.CodeAnalysis.Sarif.LogicalLocation expected, Model.LogicalLocation actual)
        {
            if (expected == null) { return actual.IsNull; }

            if (expected.DecoratedName != actual.DecoratedName) { return false; }
            if (expected.FullyQualifiedName != actual.FullyQualifiedName) { return false; }
            if (expected.Index != actual.Index) { return false; }
            if (expected.Kind != actual.Kind) { return false; }
            if (expected.Name != actual.Name) { return false; }

            return true;
        }
    }
}
