using BSOA.Demo.Conversion;
using BSOA.Demo.Model;
using Microsoft.CodeAnalysis.Sarif;
using System.Collections.Generic;

namespace BSOA.Demo
{
    public class SarifLogFiltered
    {
        public List<Microsoft.CodeAnalysis.Sarif.Location> Locations { get; set; }

        public SarifLogFiltered()
        {
            Locations = new List<Microsoft.CodeAnalysis.Sarif.Location>();
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
            foreach (Microsoft.CodeAnalysis.Sarif.Location region in Locations)
            {
                LocationConverter.Convert(region, log);
            }

            return log;
        }

        public bool Equals(SarifLogBsoa log)
        {
            if (Locations.Count != log.Location.Count)
            {
                return false;
            }

            for (int i = 0; i < Locations.Count; ++i)
            {
                Microsoft.CodeAnalysis.Sarif.Location left = Locations[i];
                Model.Location right = log.Location[i];

                if (!LocationConverter.Compare(left, right))
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            return $"{Locations.Count:n0} {nameof(Locations)}";
        }
    }
}
