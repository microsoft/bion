// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Reflection;

using BSOA.Diagnostics;
using BSOA.IO;

using Microsoft.CodeAnalysis.Sarif;

namespace BSOA.Demo
{
    public class Benchmarker
    {
        public string InputFilePath { get; set; }
        public string BsoaBinPath { get; set; }
        public string JsonOutPath { get; set; }
        public string WorkingFolderPath { get; set; }

        private string SdkDescription { get; }

        public Benchmarker(string inputFilePath, string workingFolderPath)
        {
            InputFilePath = inputFilePath;
            WorkingFolderPath = workingFolderPath;
            Directory.CreateDirectory(WorkingFolderPath);

            string fileName = Path.GetFileName(inputFilePath);
            BsoaBinPath = Path.Combine(WorkingFolderPath, Path.ChangeExtension(fileName, ".bsoa"));
            JsonOutPath = Path.Combine(WorkingFolderPath, fileName);

            Assembly sdk = typeof(SarifLog).Assembly;
            SdkDescription = $"{sdk.GetName().Name} v{sdk.GetName().Version}";
        }

        public void Run(bool forceReconvert)
        {
            SarifLog bsoa = null, unused = null;

            // Convert SarifLog to JSON, SoA JSON, and SoA Binary forms
            Convert(forceReconvert);

            // Load with diagnostics (see column sizes)
            TreeDiagnostics diagnostics = SarifLog.Diagnostics(BsoaBinPath);
            Console.WriteLine();
            diagnostics.Write(Console.Out, logToDepth: 3);

            // Compare loading times
            bsoa = Measure.LoadPerformance<SarifLog>(SarifLog.Load, BsoaBinPath, $"Loading {Path.GetFileName(BsoaBinPath)} into {SdkDescription}", iterations: 5);
            Console.WriteLine($"Line Sum: {LineTotal(bsoa):n0}");

            unused = Measure.LoadPerformance(SarifLog.Load, InputFilePath, $"Loading {Path.GetFileName(InputFilePath)} into {SdkDescription}", iterations: 5);
        }

        private void Convert(bool force)
        {
            if (force == false && File.Exists(BsoaBinPath)) { return; }

            SarifLog bsoaLog = new SarifLog();
            bsoaLog = Measure.LoadPerformance(SarifLog.Load, InputFilePath, $"Loading {Path.GetFileName(InputFilePath)} into {SdkDescription}...", 1);

            Measure.Time($"Writing as '{BsoaBinPath}'...", () =>
            {
                bsoaLog.Save(BsoaBinPath);
            });

            Measure.Time($"Writing as '{JsonOutPath}'...", () =>
            {
                bsoaLog.Save(File.Create(JsonOutPath), SarifFormat.IndentedJSON);
            });
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