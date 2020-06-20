using BSOA.Diagnostics;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using System;
using System.IO;

namespace BSOA.Demo.Comparison
{
    class Program
    {
        static void Main(string[] args)
        {
            SarifLog log;
            string mode = (args.Length > 0 ? args[0].ToLowerInvariant() : "load");
            string filePath = (args.Length > 1 ? args[1] : @"C:\Download\Demo\V2\Inputs\CodeAsData.sarif");

            switch(mode)
            {
                case "load":
                    log = Measure.LoadPerformance<SarifLog>(SarifLog.Load, filePath, "SARIF JSON to Normal object model", iterations: 8);
                    Console.WriteLine($"Line Sum: {LineTotal(log):n0}");
                    break;

                case "build":
                    RegionDemoBuilder.Build(SarifLog.Load(filePath), @"C:\Download\Demo\V2\Inputs\Regions.json");
                    break;

                case "loadandsave":
                    string outputPath = (args.Length > 2 ? args[2] : Path.Combine(Path.GetDirectoryName(filePath), "..", Path.GetFileName(filePath)));
                    log = SarifLog.Load(filePath);

                    //log.Save(outputPath);

                    using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(outputPath)))
                    {
                        writer.Formatting = Formatting.Indented;
                        JsonSerializer.Create().Serialize(writer, log);
                    }

                    break;

                default:
                    Console.WriteLine($"Unknown mode '{mode}'. Usage: BSOA.Demo.Comparison <load/build> <filePath>");
                    break;
            }
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
