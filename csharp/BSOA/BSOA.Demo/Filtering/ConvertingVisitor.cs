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

        public override Run VisitRun(Run node)
        {
            Result.Runs.Add(node.DeepClone());
            return base.VisitRun(node);
        }

        public override Result VisitResult(Result node)
        {
            node.PartialFingerprints = null;

            foreach(string name in node.PropertyNames)
            {
                node.RemoveProperty(name);
            }

            return base.VisitResult(node);
        }
    }
}
