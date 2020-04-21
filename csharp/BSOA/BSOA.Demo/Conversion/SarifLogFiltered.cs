using BSOA.Demo.Conversion;
using BSOA.Demo.Model;
using Microsoft.CodeAnalysis.Sarif;
using System.Collections.Generic;

namespace BSOA.Demo
{
    public class SarifLogFiltered
    {
        public List<Region> Regions { get; set; }

        public SarifLogFiltered()
        {
            Regions = new List<Region>();
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
            foreach (Region region in Regions)
            {
                RegionConverter.Convert(region, log.Regions);
            }

            return log;
        }

        public bool Equals(SarifLogBsoa log)
        {
            if (Regions.Count != log.Regions.Count)
            {
                return false;
            }

            for (int i = 0; i < Regions.Count; ++i)
            {
                Region left = Regions[i];
                Region4 right = log.Regions[i];

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
