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

        public override Region VisitRegion(Region region)
        {
            Result.Regions.Add(Scope(region.DeepClone()));

            return base.VisitRegion(region);
        }

        private Region Scope(Region r)
        {
            r.Message = null;
            r.Snippet = null;
            r.SourceLanguage = null;

            r.Tags.Clear();

            foreach (string propertyName in r.PropertyNames)
            {
                r.RemoveProperty(propertyName);
            }

            return r;
        }
    }
}
