using Microsoft.CodeAnalysis.Sarif;

namespace BSOA.Demo
{
    internal class ConvertingVisitor : SarifRewritingVisitor
    {
        public SarifLogFiltered Result { get; set; }

        public ConvertingVisitor()
        {
            Result = new SarifLogFiltered();
        }

        public override Location VisitLocation(Location node)
        {
            Result.Locations.Add(node.DeepClone());
            return base.VisitLocation(node);
        }
    }
}
