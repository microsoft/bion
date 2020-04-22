using BSOA.Demo.Conversion;
using BSOA.Demo.Model;
using Microsoft.CodeAnalysis.Sarif;
using System.Collections.Generic;

namespace BSOA.Demo
{
    public class SarifLogFiltered
    {
        public List<Microsoft.CodeAnalysis.Sarif.Region> Regions { get; set; }

        public SarifLogFiltered()
        {
            Regions = new List<Microsoft.CodeAnalysis.Sarif.Region>();
        }

        public static SarifLogFiltered FromSarif(SarifLog log)
        {
            ConvertingVisitor visitor = new ConvertingVisitor();
            visitor.VisitSarifLog(log);
            return visitor.Result;
        }

        public SarifLogBsoa ToBsoa()
        {
            SarifLogBsoa log = new SarifLogBsoa();

            // Convert to Bsoa RegionTable
            foreach (Microsoft.CodeAnalysis.Sarif.Region region in Regions)
            {
                RegionConverter.Convert(region, log);
            }

            return log;
        }

        public bool Equals(SarifLogBsoa log)
        {
            if (Regions.Count != log.Region.Count)
            {
                return false;
            }

            for (int i = 0; i < Regions.Count; ++i)
            {
                Microsoft.CodeAnalysis.Sarif.Region left = Regions[i];
                Model.Region right = log.Region[i];

                if (!RegionConverter.Compare(left, right))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return $"{Regions.Count:n0} Regions";
        }
    }
}
