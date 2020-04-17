using BSOA.IO;
using BSOA.Json;
using Microsoft.CodeAnalysis.Sarif;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ScaleDemo
{
    class Program
    {
        private const double Megabyte = 1024 * 1024;

        private static JsonSerializer _jsonSerializer = new JsonSerializer();

        static void Main(string[] args)
        {
            string folderPath = (args.Length > 0 ? args[0] : @"C:\Download\Demo");
            string jsonPath = Path.Combine(folderPath, "Regions.json");
            string bsoaPath = Path.Combine(folderPath, "Regions.bsoa");

            if (!File.Exists(bsoaPath))
            {
                Convert(jsonPath, bsoaPath);
            }

            //Measure(Normal, jsonPath, "JSON, Newtonsoft -> SDK class", Region);
            Measure(Bsoa, bsoaPath, "BSOA + BinaryTreeSerializer", Region4);
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

        static void Convert(string jsonPath, string bsoaPath)
        {
            List<Region> regions = Normal(jsonPath);

            RegionTable table = new RegionTable();
            foreach (Region region in regions)
            {
                table.Add(region);
            }

            using (BinaryTreeWriter writer = new BinaryTreeWriter(File.Create(bsoaPath)))
            {
                table.Write(writer);
            }

            using (JsonTreeWriter writer = new JsonTreeWriter(File.Create(Path.ChangeExtension(bsoaPath, ".bsoa.json"))))
            {
                table.Write(writer);
            }
        }

        static List<Region> Normal(string jsonPath)
        {
            using (JsonReader reader = new JsonTextReader(File.OpenText(jsonPath)))
            {
                return _jsonSerializer.Deserialize<List<Region>>(reader);
            }
        }

        static RegionTable Bsoa(string bsoaPath)
        {
            RegionTable table = new RegionTable();

            using (BinaryTreeReader reader = new BinaryTreeReader(File.OpenRead(bsoaPath)))
            {
                table.Read(reader);
            }

            return table;
        }

        static string Region(List<Region> list)
        {
            return $"{list.Sum(r => r.StartLine):n0}";
        }

        static string Region4(IReadOnlyList<Region4> list)
        {
            return $"{list.Sum(r => r.StartLine):n0}";
        }
    }
}
