using BSOA.Demo.Model;
using BSOA.IO;
using BSOA.Json;
using Microsoft.CodeAnalysis.Sarif;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

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

        public void Run(bool forceReconvert)
        {
            // Convert SarifLog to JSON, SoA JSON, and SoA Binary forms
            Convert(forceReconvert);

            SarifLogFiltered filtered = null;
            SarifLogBsoa bsoa = null;

            // Compare loading times
            filtered = Measure(LoadNormalJson, NormalJsonPath, "JSON, Newtonsoft");

            bsoa = Measure(LoadBsoaJson, BsoaJsonPath, "BSOA JSON, Newtonsoft");
            Console.WriteLine($" -> {(filtered.Equals(bsoa) ? "Identical" : "Different!")}");

            bsoa = Measure(LoadBsoaBinary, BsoaBinPath, "BSOA Binary");
            Console.WriteLine($" -> {(filtered.Equals(bsoa) ? "Identical" : "Different!")}");

            // Change something and verify difference detected
            var snippet = bsoa.Region[bsoa.Region.Count / 2].Snippet;
            snippet.Text = "Changed!";
            Console.WriteLine();
            Console.WriteLine($"Verify difference detected:");
            Console.WriteLine($" -> {(filtered.Equals(bsoa) ? "Identical" : "Different!")}");
        }

        private void Convert(bool force)
        {
            if (force == false && File.Exists(BsoaBinPath) && File.Exists(BsoaJsonPath) && File.Exists(NormalJsonPath)) { return; }

            // Load Sarif Log with current OM
            SarifLog log = null;

            Time($"Loading {InputFilePath}...", () => log = SarifLog.Load(InputFilePath));

            // Extract a BSOA-supported SarifLog subset for apples-to-apples comparison with BSOA form
            SarifLogFiltered filtered = null;
            SarifLogBsoa bsoaLog = new SarifLogBsoa();

            Time($"Converting to demo form...", () =>
            {
                filtered = SarifLogFiltered.FromSarif(log);
                bsoaLog = filtered.ToBsoa();

                Console.WriteLine($" -> {filtered.ToString()}");
            });

            Time($"Writing as JSON to '{NormalJsonPath}'...", () =>
            {
                using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(NormalJsonPath)))
                {
                    //writer.Formatting = Formatting.Indented;
                    _jsonSerializer.Serialize(writer, filtered);
                }
            });

            Time($"Writing as BSOA JSON to '{BsoaJsonPath}'...", () =>
            {
                using (JsonTreeWriter writer = new JsonTreeWriter(File.Create(BsoaJsonPath), new TreeSerializationSettings() { Verbose = false }))
                {
                    bsoaLog.Write(writer);
                }
            });

            Time($"Writing as BSOA Binary to '{BsoaBinPath}'...", () =>
            {
                using (BinaryTreeWriter writer = new BinaryTreeWriter(File.Create(BsoaBinPath)))
                {
                    bsoaLog.Write(writer);
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

        static T Measure<T>(Func<string, T> loader, string path, string description, int iterations = 4)
        {
            T result = default(T);
            double ramBeforeMB = GC.GetTotalMemory(true) / Megabyte;
            Stopwatch w = Stopwatch.StartNew();

            Console.WriteLine();
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

            return result;
        }

        private SarifLogFiltered LoadNormalJson(string jsonPath)
        {
            using (JsonReader reader = new JsonTextReader(File.OpenText(jsonPath)))
            {
                return _jsonSerializer.Deserialize<SarifLogFiltered>(reader);
            }
        }

        private SarifLogBsoa LoadBsoaBinary(string bsoaBinaryPath)
        {
            SarifLogBsoa log = new SarifLogBsoa();

            using (BinaryTreeReader reader = new BinaryTreeReader(File.OpenRead(bsoaBinaryPath)))
            {
                log.Read(reader);
            }

            return log;
        }

        private SarifLogBsoa LoadBsoaJson(string bsoaJsonPath)
        {
            SarifLogBsoa log = new SarifLogBsoa();

            using (JsonTreeReader reader = new JsonTreeReader(File.OpenRead(bsoaJsonPath)))
            {
                log.Read(reader);
            }

            return log;
        }
    }
}
