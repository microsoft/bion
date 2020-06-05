using BSOA.Demo.Conversion;
using BSOA.Demo.Model;
using Microsoft.CodeAnalysis.Sarif;
using System.Collections.Generic;
using System.Linq;

namespace BSOA.Demo
{
    public class SarifLogFiltered
    {
        public List<Microsoft.CodeAnalysis.Sarif.Run> Runs { get; set; }

        public SarifLogFiltered()
        {
            Runs = new List<Microsoft.CodeAnalysis.Sarif.Run>();
        }

        public static SarifLogFiltered FromSarif(Microsoft.CodeAnalysis.Sarif.SarifLog log)
        {
            ConvertingVisitor visitor = new ConvertingVisitor();
            visitor.VisitSarifLog(log);
            return visitor.Result;
        }

        public bool Equals(Model.SarifLog log)
        {
            if (Runs.Count != log.Runs.Count)
            {
                return false;
            }

            for (int i = 0; i < Runs.Count; ++i)
            {
                Microsoft.CodeAnalysis.Sarif.Run left = Runs[i];
                Model.Run right = log.Runs[i];

                if (!RunConverter.Compare(left, right))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return $"{Runs.Count:n0} {nameof(Runs)}; {Runs.Sum((run) => run.Results.Count):n0} Results";
        }
    }
}
