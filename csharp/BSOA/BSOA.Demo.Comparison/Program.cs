using BSOA.Diagnostics;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Reflection;

namespace BSOA.Demo.Comparison
{
    class Program
    {
        static void Main(string[] args)
        {
            string mode = (args.Length > 0 ? args[0].ToLowerInvariant() : "load");
            string filePath = (args.Length > 1 ? args[1] : @"C:\Download\Demo\V2\Inputs\CodeAsData.sarif");
            string outputPath;
            SarifLog log;

            Assembly sdk = typeof(SarifLog).Assembly;
            string loadDescription = $"Loading {Path.GetFileName(filePath)} into {sdk.GetName().Name} v{sdk.GetName().Version}...";

            switch (mode)
            {
                case "load":
                    log = Measure.LoadPerformance(SarifLog.Load, filePath, loadDescription, iterations: 5);
                    Console.WriteLine($"Line Sum: {LineTotal(log):n0}");
                    break;

                
                case "loadandsave":
                    outputPath = (args.Length > 2 ? args[2] : Path.Combine(Path.GetDirectoryName(filePath), "..\\Out", Path.GetFileName(filePath)));
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                    
                    log = Measure.LoadPerformance(SarifLog.Load, filePath, loadDescription, iterations: 1);
                    Measure.Time($"Saving to {outputPath}", () => log.Save(outputPath));

                    break;

                case "indent":
                    outputPath = (args.Length > 2 ? args[2] : Path.Combine(Path.GetDirectoryName(filePath), "..\\Indented", Path.GetFileName(filePath)));
                    Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

                    log = Measure.LoadPerformance(SarifLog.Load, filePath, loadDescription, iterations: 1);

                    using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(outputPath)))
                    {
                        writer.Formatting = Formatting.Indented;
                        JsonSerializer.Create().Serialize(writer, log);
                    }

                    break;

                case "regionbuild":
                    outputPath = (args.Length > 2 ? args[2] : Path.Combine(Path.GetDirectoryName(filePath), "Regions.json"));

                    Console.WriteLine($"Building Region demo from {filePath} to {outputPath}");
                    RegionDemoBuilder.Build(filePath, outputPath);
                    
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
