// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.IO;

using BSOA.IO;
using BSOA.Json;

using Microsoft.CodeAnalysis.Sarif;

using Newtonsoft.Json;

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
            SarifLog bsoa = null, unused = null;

            // Convert SarifLog to JSON, SoA JSON, and SoA Binary forms
            Convert(forceReconvert);

            //// Load with diagnostics (see column sizes)
            //Console.WriteLine();
            //LoadBsoaBinary(BsoaBinPath, diagnostics: true, diagnosticsDepth: 3);

            // Compare loading times
            bsoa = Measure(LoadBsoaBinary, BsoaBinPath, "BSOA Binary to SoA model", iterations: 10);
            unused = Measure(LoadBsoaViaNewtonsoft, NormalJsonPath, "JSON, Newtonsoft to BSOA directly", iterations: 3);
        }

        private void ChangeSomething(SarifLog log)
        {
            //log.Location[log.Location.Count / 2].PhysicalLocation.ArtifactLocation.Index = 45;

            //log.Location[log.Location.Count / 2].PhysicalLocation.Region.Snippet.Text = "Changed!";

            var results = log.Runs[0].Results;
            results[results.Count / 2].Message.Text = "Different";
        }

        private void Convert(bool force)
        {
            if (force == false && File.Exists(BsoaBinPath) && File.Exists(BsoaJsonPath) && File.Exists(NormalJsonPath)) { return; }

            SarifLog bsoaLog = new SarifLog();
            bsoaLog = Measure(LoadBsoaViaNewtonsoft, InputFilePath, $"Loading BSOA via Newtonsoft from SARIF JSON {InputFilePath}...", 1);

            // For "apples to apples" comparison, write SARIF JSON of the supported log subset out
            Time($"Writing as JSON to '{NormalJsonPath}'...", () =>
            {
                using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(NormalJsonPath)))
                {
                    writer.Formatting = Formatting.Indented;
                    _jsonSerializer.Serialize(writer, bsoaLog);
                }
            });

            Time($"Writing as BSOA Binary to '{BsoaBinPath}'...", () =>
            {
                using (BinaryTreeWriter writer = new BinaryTreeWriter(File.Create(BsoaBinPath)))
                {
                    bsoaLog.DB.Write(writer);
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

        private SarifLog LoadBsoaViaNewtonsoft(string jsonPath)
        {
            SarifLog log = AsJson.Load<SarifLog>(jsonPath);
            log.DB.Trim();
            return log;
        }

        private SarifLog LoadBsoaJson(string bsoaJsonPath)
        {
            SarifLog log = new SarifLog();

            using (ITreeReader reader = new JsonTreeReader(File.OpenRead(bsoaJsonPath)))
            {
                log.DB.Read(reader);
            }

            return log;
        }

        private SarifLog LoadBsoaBinary(string bsoaBinaryPath)
        {
            return LoadBsoaBinary(bsoaBinaryPath, false);
        }

        private SarifLog LoadBsoaBinary(string bsoaBinaryPath, bool diagnostics, int diagnosticsDepth = -1)
        {
            SarifLog log = new SarifLog();

            using (ITreeReader reader = Build(bsoaBinaryPath, diagnostics))
            {
                log.DB.Read(reader);

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
