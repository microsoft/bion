using BSOA.Diagnostics;

using Microsoft.CodeAnalysis.Sarif;

using System;

namespace BSOA.Demo.Comparison
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = (args.Length > 0 ? args[0] : @"C:\Download\Demo\V2\Inputs\CodeAsData.sarif");

            SarifLog log = Measure.LoadPerformance<SarifLog>(SarifLog.Load, filePath, "SARIF JSON to Normal object model", iterations: 1);

            long lineTotal = LineTotal(log);
            Console.WriteLine($"Line Sum: {lineTotal:n0}");
        }

        private static long LineTotal(SarifLog log)
        {
            long lineTotal = 0;

            foreach (Run run in log.Runs)
            {
                foreach (Result result in run.Results)
                {
                    foreach (Location location in result.Locations)
                    {
                        lineTotal += location?.PhysicalLocation?.Region?.StartLine ?? 0;
                    }
                }
            }

            return lineTotal;
        }
    }
}
