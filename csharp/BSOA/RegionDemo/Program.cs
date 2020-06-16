using BSOA.Demo.Model;
using BSOA.Diagnostics;
using BSOA.Json;

using System;
using System.IO;

namespace RegionDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = (args.Length > 0 ? args[0] : @"C:\Download\Demo\V2\Inputs\Regions.json");
            string outputPath = (args.Length > 1 ? args[1] : Path.Combine(Path.GetDirectoryName(filePath), "..", Path.ChangeExtension(Path.GetFileName(filePath), ".bsoa")));

            TinyLog log = Measure.LoadPerformance<TinyLog>(AsJson.Load<TinyLog>, filePath, "Load Regions via Newtonsoft", iterations: 10);
            Console.WriteLine($" -> LineTotal: {LineTotal(log):n0}");

            Measure.Time("Save as BSOA", () => log.WriteBsoa(outputPath), iterations: 5);

            Measure.Time("Save as JSON", () => AsJson.Save(Path.ChangeExtension(outputPath, ".json"), log));
        }

        private static long LineTotal(TinyLog log)
        {
            long lineTotal = 0;

            foreach (Region region in log.Regions)
            {
                lineTotal += region.StartLine;
            }

            return lineTotal;
        }
    }
}
