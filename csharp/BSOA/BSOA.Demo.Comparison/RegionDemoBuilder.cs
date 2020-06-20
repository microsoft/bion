using BSOA.Json;

using Microsoft.CodeAnalysis.Sarif;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Comparison
{
    public static class RegionDemoBuilder
    {
        public static void Build(string inputPath, string outputPath)
        {
            SarifLog log = SarifLog.Load(inputPath);
            
            // Extract Regions from log
            RegionVisitor visitor = new RegionVisitor();
            visitor.VisitSarifLog(log);

            // Put on object with root "Regions" property
            TinyLog tiny = new TinyLog();
            tiny.Regions = visitor.Regions;

            Console.WriteLine($"  -> LineTotal: {LineTotal(tiny):n0}");

            // Write as JSON
            AsJson.Save(outputPath, tiny);
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

    internal class TinyLog
    {
        public List<Region> Regions { get; set; }
    }

    internal class RegionVisitor : SarifRewritingVisitor
    {
        public List<Region> Regions { get; set; }

        public RegionVisitor()
        {
            Regions = new List<Region>();
        }

        public override Region VisitRegion(Region node)
        {
            Regions.Add(node);
            return node;
        }
    }
}