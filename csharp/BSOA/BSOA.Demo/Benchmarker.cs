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

        public void Run(bool forceReconvert)
        {
            // Convert SarifLog to JSON, SoA JSON, and SoA Binary forms
            Convert(forceReconvert);

            SarifLogFiltered filtered = null;
            SarifLogBsoa bsoa = null;

            // Load with diagnostics (see column sizes)
            Console.WriteLine();
            LoadBsoaBinary(BsoaBinPath, diagnostics: true, diagnosticsDepth: 3);

            // Compare loading times
            filtered = Measure(LoadNormalJson, NormalJsonPath, "JSON, Newtonsoft to Normal classes", iterations: 3);
            bsoa = Measure(LoadBsoaBinary, BsoaBinPath, "BSOA Binary to SoA model", iterations: 10);

            // Verify logs match; change something to test verification logic
            Console.WriteLine($" -> {(filtered.Equals(bsoa) ? "Identical" : "Different!")}");
            ChangeSomething(bsoa);
            Console.WriteLine();
            Console.WriteLine($"Test difference detected:");
            Console.WriteLine($" -> {(filtered.Equals(bsoa) ? "Identical" : "Different!")}");
        }

        private void ChangeSomething(SarifLogBsoa log)
        {
            //var artifactLocation = log.Location[log.Location.Count / 2].PhysicalLocation.ArtifactLocation;
            //artifactLocation.Index = 45;

            //var snippet = log.Location[log.Location.Count / 2].PhysicalLocation.Region.Snippet;
            //snippet.Text = "Changed!";

            var results = log.Run[0].Results;
            var message = results[results.Count / 2].Message;
            message.Text = "Different";
        }

        private void Convert(bool force)
        {
            if (force == false && File.Exists(BsoaBinPath) && File.Exists(BsoaJsonPath) && File.Exists(NormalJsonPath)) { return; }

            // Load Sarif Log with current OM
            SarifLog log = null;

            Time($"Loading {InputFilePath}...", () => log = SarifLog.Load(InputFilePath));

            SarifLogFiltered filtered = null;
            Time($"Extracting supported subset...", () =>
            {
                filtered = SarifLogFiltered.FromSarif(log);
            });

            // Extract a BSOA-supported SarifLog subset for apples-to-apples comparison with BSOA form
            SarifLogBsoa bsoaLog = new SarifLogBsoa();

            Time($"Converting to BSOA model...", () =>
            {
                bsoaLog = filtered.ToBsoa();
            }, iterations: 10);

            Console.WriteLine($" -> {bsoaLog.ToString()}");

            Time($"Writing as JSON to '{NormalJsonPath}'...", () =>
            {
                using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(NormalJsonPath)))
                {
                    //writer.Formatting = Formatting.Indented;
                    _jsonSerializer.Serialize(writer, filtered);
                }
            });

            Time($"Trimming (BSOA pre-serialization cost, not specific to JSON or Binary)...", () =>
            {
                bsoaLog.Trim();
            });

            Time($"Writing as BSOA Binary to '{BsoaBinPath}'...", () =>
            {
                using (BinaryTreeWriter writer = new BinaryTreeWriter(File.Create(BsoaBinPath)))
                {
                    bsoaLog.Write(writer);
                }
            });

            Time($"Writing as BSOA JSON to '{BsoaJsonPath}'...", () =>
            {
                using (JsonTreeWriter writer = new JsonTreeWriter(File.Create(BsoaJsonPath), new TreeSerializationSettings() { Verbose = true }))
                {
                    bsoaLog.Write(writer);
                }
            });
        }

        public static TimeSpan Time(string description, Action method, int iterations = 1)
        {
            Console.WriteLine();
            Console.WriteLine(description);

            Stopwatch w = Stopwatch.StartNew();
            TimeSpan elapsedAfterFirst = TimeSpan.Zero;

            for (int iteration = 0; iteration < iterations; iteration++)
            {
                GC.Collect();

                w.Restart();
                method();
                w.Stop();

                Console.Write($"{(iteration > 0 ? " | " : "")}{w.Elapsed.TotalSeconds:n2}s");
                if (iteration > 0) { elapsedAfterFirst += w.Elapsed; }
            }

            Console.WriteLine();
            return (iterations == 1 ? w.Elapsed : (elapsedAfterFirst / (iterations - 1)));
        }

        static T Measure<T>(Func<string, T> loader, string path, string description, int iterations = 5)
        {
            T result = default(T);
            double ramBeforeMB = GC.GetTotalMemory(true) / Megabyte;

            // Run and time the method
            TimeSpan averageRuntime = Time(description, () => result = loader(path), iterations);

            double ramAfterMB = GC.GetTotalMemory(true) / Megabyte;
            double fileSizeMB = new FileInfo(path).Length / Megabyte;
            double loadMegabytesPerSecond = fileSizeMB / averageRuntime.TotalSeconds;

            Console.WriteLine($" -> Read {result} in {fileSizeMB:n1} MB at {loadMegabytesPerSecond:n1} MB/s into {(ramAfterMB - ramBeforeMB):n1} MB RAM");

            return result;
        }

        private SarifLogFiltered LoadNormalJson(string jsonPath)
        {
            using (JsonReader reader = new JsonTextReader(File.OpenText(jsonPath)))
            {
                return _jsonSerializer.Deserialize<SarifLogFiltered>(reader);
            }
        }

        private SarifLogBsoa LoadBsoaJson(string bsoaJsonPath)
        {
            SarifLogBsoa log = new SarifLogBsoa();

            using (ITreeReader reader = new JsonTreeReader(File.OpenRead(bsoaJsonPath)))
            {
                log.Read(reader);
            }

            return log;
        }

        private SarifLogBsoa LoadBsoaBinary(string bsoaBinaryPath)
        {
            return LoadBsoaBinary(bsoaBinaryPath, false);
        }

        private SarifLogBsoa LoadBsoaBinary(string bsoaBinaryPath, bool diagnostics, int diagnosticsDepth = -1)
        {
            SarifLogBsoa log = new SarifLogBsoa();

            using (ITreeReader reader = Build(bsoaBinaryPath, diagnostics))
            {
                log.Read(reader);

                if (diagnostics)
                {
                    TreeDiagnostics tree = ((TreeDiagnosticsReader)reader).Tree;
                    tree.Write(Console.Out, diagnosticsDepth);
                }
            }

            return log;
        }

        private ITreeReader Build(string bsoaBinaryPath, bool diagnostics)
        {
            ITreeReader reader = new BinaryTreeReader(File.OpenRead(bsoaBinaryPath));
            if (diagnostics) { reader = new TreeDiagnosticsReader(reader); }
            return reader;
        }
    }
}
