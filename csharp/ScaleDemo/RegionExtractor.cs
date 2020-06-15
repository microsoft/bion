// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

using Bion.Json;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

namespace ScaleDemo
{
    internal class RegionAccumulator : SarifRewritingVisitor
    {
        public List<Region> AllRegions = new List<Region>();

        public override Region VisitRegion(Region node)
        {
            AllRegions.Add(node);
            return base.VisitRegion(node);
        }
    }

    public static class RegionExtractor
    {
        public static void Extract(string folderPath, string sarifFileName, int targetCount)
        {
            Console.WriteLine($"Extracting Regions from '{sarifFileName}'...");

            // Load Sarif log and find all Regions
            RegionAccumulator regionAccumulator = new RegionAccumulator();
            regionAccumulator.Visit(SarifLog.Load(Path.Combine(folderPath, sarifFileName)));

            Console.WriteLine($"Found {regionAccumulator.AllRegions.Count:n0} regions.");
            Console.WriteLine($"Writing converted forms with {targetCount:n0} region instances...");

            // Generate a list of the target size from it
            List<Region> listAtScale = new List<Region>(targetCount);
            for (int i = 0; i < targetCount; ++i)
            {
                listAtScale.Add(regionAccumulator.AllRegions[i % regionAccumulator.AllRegions.Count]);
            }

            // Write as minified JSON
            JsonSerializer serializer = new JsonSerializer();
            using (StreamWriter sw = File.CreateText(Path.Combine(folderPath, "Regions.json")))
            using (JsonTextWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, listAtScale);
            }

            // Write as BION
            using (Stream stream = File.Create(Path.Combine(folderPath, "Regions.bion")))
            using (JsonWriter writer = new BionDataWriter(stream))
            {
                serializer.Serialize(writer, listAtScale);
            }

            // Write as BIN
            BlockSerializer elfieSerializer = new BlockSerializer();
            RegionTable table = elfieSerializer.Convert(listAtScale);

            using (Stream stream = File.Create(Path.Combine(folderPath, "Regions.bin")))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                elfieSerializer.Serialize(writer, table);
            }

            Console.WriteLine("Done.");
            Console.WriteLine();
        }
    }
}
