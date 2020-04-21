using BSOA.Demo.Conversion;
using BSOA.Demo.Model;
using BSOA.IO;
using BSOA.Json;
using Microsoft.CodeAnalysis.Sarif;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BSOA.Demo
{
    public class Benchmarker
    {
        public string InputFilePath { get; set; }
        public string WorkingFolderPath { get; set; }
        public string NormalJsonPath { get; set; }
        public string BsoaJsonPath { get; set; }
        public string BsoaBinPath { get; set; }

        private const double Megabyte = 1024 * 1024;
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public Benchmarker(string inputFilePath, string workingFolderPath)
        {
            InputFilePath = inputFilePath;
            WorkingFolderPath = workingFolderPath;

            string fileName = Path.GetFileName(inputFilePath);

            NormalJsonPath = Path.Combine(WorkingFolderPath, Path.ChangeExtension(fileName, ".json"));
            BsoaJsonPath = Path.Combine(WorkingFolderPath, Path.ChangeExtension(fileName, ".bsoa.json"));
            BsoaBinPath = Path.Combine(WorkingFolderPath, Path.ChangeExtension(fileName, ".bsoa.bin"));
        }

        public void Run()
        {
            // Convert SarifLog to JSON, SoA JSON, and SoA Binary forms
            Convert();

            // Compare loading times
            Measure(LoadNormalJson, NormalJsonPath, "JSON, Newtonsoft", Region);
            Measure(LoadBsoaJson, BsoaJsonPath, "BSOA JSON, Newtonsoft", Region4);
            Measure(LoadBsoaBinary, BsoaBinPath, "BSOA Binary", Region4);
        }

        private void Convert()
        {
            if (File.Exists(BsoaBinPath) && File.Exists(BsoaJsonPath) && File.Exists(NormalJsonPath)) { return; }

            // Load Sarif Log with current OM
            SarifLog log = null;

            Time($"Loading {InputFilePath}...", () => log = SarifLog.Load(InputFilePath));

            // Convert to 'DemoData' instance with scoped data
            List<Region> regions = null;
            RegionTable table = null;

            Time($"Converting to demo form...", () =>
            {
                DemoConvertingVisitor visitor = new DemoConvertingVisitor();
                visitor.VisitSarifLog(log);

                regions = visitor.Result.Regions;

                // Convert to Bsoa RegionTable
                table = new RegionTable();
                foreach (Region region in regions)
                {
                    RegionConverter.Convert(region, table);
                }
            });

            Time($"Writing as JSON to '{NormalJsonPath}'...", () =>
            {
                using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(NormalJsonPath)))
                {
                    //writer.Formatting = Formatting.Indented;
                    _jsonSerializer.Serialize(writer, regions);
                }
            });

            Time($"Writing as BSOA JSON to '{BsoaJsonPath}'...", () =>
            {
                using (JsonTreeWriter writer = new JsonTreeWriter(File.Create(BsoaJsonPath), new TreeSerializationSettings() { Compact = true }))
                {
                    table.Write(writer);
                }
            });

            Time($"Writing as BSOA Binary to '{BsoaBinPath}'...", () =>
            {
                using (BinaryTreeWriter writer = new BinaryTreeWriter(File.Create(BsoaBinPath)))
                {
                    table.Write(writer);
                }
            });
        }

        public static void Time(string description, Action method)
        {
            Console.WriteLine(description);

            Stopwatch w = Stopwatch.StartNew();
            method();
            w.Stop();

            Console.WriteLine($" -> {w.Elapsed.TotalSeconds:n2} sec.");
            Console.WriteLine();
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

        private List<Region> LoadNormalJson(string jsonPath)
        {
            using (JsonReader reader = new JsonTextReader(File.OpenText(jsonPath)))
            {
                return _jsonSerializer.Deserialize<List<Region>>(reader);
            }
        }

        private RegionTable LoadBsoaBinary(string bsoaBinaryPath)
        {
            RegionTable table = new RegionTable();

            using (BinaryTreeReader reader = new BinaryTreeReader(File.OpenRead(bsoaBinaryPath)))
            {
                table.Read(reader);
            }

            return table;
        }

        private RegionTable LoadBsoaJson(string bsoaJsonPath)
        {
            RegionTable table = new RegionTable();

            using (JsonTreeReader reader = new JsonTreeReader(File.OpenRead(bsoaJsonPath)))
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
