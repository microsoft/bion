// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;

using BSOA.Diagnostics;
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
        public string BsoaBinPath { get; set; }

        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        public Benchmarker(string inputFilePath, string workingFolderPath)
        {
            InputFilePath = inputFilePath;
            WorkingFolderPath = workingFolderPath;

            string fileName = Path.GetFileName(inputFilePath);

            NormalJsonPath = Path.Combine(WorkingFolderPath, Path.ChangeExtension(fileName, ".json"));
            BsoaBinPath = Path.Combine(WorkingFolderPath, Path.ChangeExtension(fileName, ".bsoa"));
        }

        public void Run(bool forceReconvert)
        {
            SarifLog bsoa = null, unused = null;

            // Convert SarifLog to JSON, SoA JSON, and SoA Binary forms
            Convert(forceReconvert);

            //// Load with diagnostics (see column sizes)
            //Console.WriteLine();
            //ShowBsoaDiagnostics(BsoaBinPath, diagnosticsDepth: 3);

            GC.Collect();

            // Compare loading times
            bsoa = Measure.LoadPerformance<SarifLog>(LoadBsoaBinary, BsoaBinPath, "BSOA Binary to SoA model", iterations: 5);
            unused = Measure.LoadPerformance(LoadBsoaViaNewtonsoft, NormalJsonPath, "JSON, Newtonsoft to BSOA directly", iterations: 2);

            long lineTotal = LineTotal(bsoa);
            Console.WriteLine($"Line Sum: {lineTotal:n0}");
        }

        private void Convert(bool force)
        {
            if (force == false && File.Exists(BsoaBinPath) && File.Exists(NormalJsonPath)) { return; }

            SarifLog bsoaLog = new SarifLog();
            bsoaLog = Measure.LoadPerformance(LoadBsoaViaNewtonsoft, InputFilePath, $"Loading BSOA via Newtonsoft from SARIF JSON {InputFilePath}...", 1);

            Measure.Time($"Writing as BSOA Binary to '{BsoaBinPath}'...", () =>
            {
                using (BinaryTreeWriter writer = new BinaryTreeWriter(File.Create(BsoaBinPath)))
                {
                    bsoaLog.DB.Write(writer);
                }
            });

            // For "apples to apples" comparison, write SARIF JSON of the supported log subset out
            Measure.Time($"Writing as JSON to '{NormalJsonPath}'...", () =>
            {
                using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(NormalJsonPath)))
                {
                    writer.Formatting = Formatting.Indented;
                    _jsonSerializer.Serialize(writer, bsoaLog);
                }
            });
        }

        private SarifLog LoadBsoaViaNewtonsoft(string jsonPath)
        {
            SarifLog log = new SarifLog();
            log = AsJson.Load<SarifLog>(jsonPath);
            log.DB.Trim();
            return log;
        }

        private SarifLog LoadBsoaBinary(string bsoaBinaryPath)
        {
            SarifLog log = new SarifLog();

            using (ITreeReader reader = new BinaryTreeReader(File.OpenRead(bsoaBinaryPath)))
            {
                log.DB.Read(reader);
            }

            return log;
        }

        private void ShowBsoaDiagnostics(string bsoaBinaryPath, int diagnosticsDepth = -1)
        {
            SarifLog log = new SarifLog();

            using (ITreeReader reader = new TreeDiagnosticsReader(new BinaryTreeReader(File.OpenRead(bsoaBinaryPath))))
            {
                log.DB.Read(reader);

                TreeDiagnostics tree = ((TreeDiagnosticsReader)reader).Tree;
                tree.Write(Console.Out, diagnosticsDepth);
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