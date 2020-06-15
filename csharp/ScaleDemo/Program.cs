// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using Bion;
using Bion.Json;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

using ScaleDemo.Serializers;

namespace ScaleDemo
{
    class Program
    {
        private const double Megabyte = 1024 * 1024;

        private static JsonSerializer _jsonSerializer = new JsonSerializer();
        private static JsonCustomDeserializer _jsonCustomDeserializer = new JsonCustomDeserializer();
        private static BionDirectDeserializer _bionDirectDeserializer = new BionDirectDeserializer();
        private static BlockSerializer _elfieSerializer = new BlockSerializer();

        static void Main(string[] args)
        {
            string folderPath = (args.Length > 0 ? args[0] : @"C:\Download\Demo");
            string jsonPath = Path.Combine(folderPath, "Regions.json");
            string bionPath = Path.Combine(folderPath, "Regions.bion");
            string binPath = Path.Combine(folderPath, "Regions.bin");

            if (!File.Exists(jsonPath))
            {
                RegionExtractor.Extract(folderPath, "Original.sarif", 1000000);
            }

            //Measure(Normal, jsonPath, "JSON, Newtonsoft -> SDK class", Region);
            //Measure(Custom, jsonPath, "JSON, CustomSerializer -> 'Ex' class", Region2);
            //Measure(Bion, bionPath, "BION, BionDataReader + BionSerializer -> struct", Region3);
            Measure(Elfie, binPath, "BIN, BlockReader + BlockSerializer -> SoA struct", Region4);

            //Measure(DefaultR2, jsonPath, "JSON, custom converter -> 'Ex' class", Region2);
            //Measure(DefaultR4, jsonPath, "JSON, custom converter -> SoA struct", Region4);
            //Measure(BionViaJsonSerializer, bionPath, "BION, BionDataReader + JsonSerializer -> class", Region);
            //Measure(BionDirectToClass, bionPath, "BION, BionReader + BionDirectSerializer -> 'Ex' class", Region2);
        }

        static T Measure<T>(Func<string, T> loader, string path, string description, Func<T, string> check, int iterations = 4)
        {
            T result = default(T);
            double ramBeforeMB = GC.GetTotalMemory(true) / Megabyte;
            Stopwatch w = Stopwatch.StartNew();

            Console.WriteLine(description);

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                w.Restart();
                result = loader(path);
                w.Stop();

                Console.Write($"{(iteration > 0 ? " | " : "")}{w.Elapsed.TotalSeconds:n2}s");
            }

            double ramAfterMB = GC.GetTotalMemory(true) / Megabyte;
            double fileSizeMB = new FileInfo(path).Length / Megabyte;
            double loadMegabytesPerSecond = fileSizeMB / w.Elapsed.TotalSeconds;

            Console.WriteLine();
            Console.WriteLine($" -> Read {fileSizeMB:n1} MB at {loadMegabytesPerSecond:n1} MB/s into {(ramAfterMB - ramBeforeMB):n1} MB RAM");
            Console.WriteLine($" -> Check {check(result)}");
            Console.WriteLine();

            return result;
        }

        static List<Region> Normal(string jsonPath)
        {
            using (JsonReader reader = new JsonTextReader(File.OpenText(jsonPath)))
            {
                return _jsonSerializer.Deserialize<List<Region>>(reader);
            }
        }

        static List<Region2> Custom(string jsonPath)
        {
            using (JsonReader reader = new JsonTextReader(File.OpenText(jsonPath)))
            {
                return _jsonCustomDeserializer.DeserializeRegion2s(reader);
            }
        }

        static List<Region2> DefaultR2(string jsonPath)
        {
            using (JsonReader reader = new JsonTextReader(File.OpenText(jsonPath)))
            {
                _jsonSerializer.ContractResolver = new RegionContractResolver();
                return _jsonSerializer.Deserialize<List<Region2>>(reader);
            }
        }

        static List<Region3> Bion(string bionPath)
        {
            using (BionReader reader = new BionReader(File.OpenRead(bionPath)))
            {
                return _bionDirectDeserializer.DeserializeRegion3s(reader);
            }
        }

        static RegionTable Elfie(string binPath)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(binPath)))
            {
                return _elfieSerializer.Deserialize(reader);
            }
        }

        static RegionTable DefaultR4(string jsonPath)
        {
            using (JsonReader reader = new JsonTextReader(File.OpenText(jsonPath)))
            {
                _jsonSerializer.ContractResolver = new RegionContractResolver();
                return _jsonSerializer.Deserialize<RegionTable>(reader);
            }
        }

        // --------

        static List<Region> BionViaJsonSerializer(string bionPath)
        {
            using (JsonReader reader = new BionDataReader(File.OpenRead(bionPath)))
            {
                return _jsonSerializer.Deserialize<List<Region>>(reader);
            }
        }

        static List<Region2> BionDirectToClass(string bionPath)
        {
            using (BionReader reader = new BionReader(File.OpenRead(bionPath)))
            {
                return _bionDirectDeserializer.DeserializeRegion2s(reader);
            }
        }

        // -----

        static string Region(List<Region> list)
        {
            return $"{list.Sum(r => r.StartLine):n0}";
        }

        static string Region2(List<Region2> list)
        {
            return $"{list.Sum(r => r.StartLine):n0}";
        }

        static string Region3(List<Region3> list)
        {
            return $"{list.Sum(r => r.StartLine):n0}";
        }

        static string Region4(IReadOnlyList<Region4> list)
        {
            return $"{list.Sum(r => r.StartLine):n0}";
        }
    }
}
