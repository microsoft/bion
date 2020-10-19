using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BSOA.Benchmarks.Diagnostics
{
    public static class FilesBenchmarker
    {
        /// <summary>
        ///  For each file in inputPath, load into an ArgumentClass with loader(),
        ///  then run each [Benchmark] method on operationsClass and report the results.
        /// </summary>
        /// <typeparam name="ArgumentClass">Object Model type each file turns into.</typeparam>
        /// <param name="operationsClass">Class containing [Benchmark] methods which take the ArgumentClass type</param>
        /// <param name="inputPath">Folder Path or single File Path which can be loaded into ArgumentClass</param>
        /// <param name="loader">Method which takes a file path and loads into an ArgumentClass intance</param>
        public static void RunFiles<ArgumentClass>(Type operationsClass, string inputPath, Func<string, ArgumentClass> loader)
        {
            // Find all [Benchmark] methods which take an ArgumentClass.
            Dictionary<string, Action<ArgumentClass>> benchmarkMethods = BenchmarkReflector.BenchmarkMethods<Action<ArgumentClass>>(operationsClass);

            List<ConsoleColumn> columns = new List<ConsoleColumn>()
            {
                new ConsoleColumn("File"),
                new ConsoleColumn("Size", Align.Right),
                new ConsoleColumn("Load", Align.Right),
                new ConsoleColumn("RAM", Align.Right, Highlight.On)
            };

            foreach (string key in benchmarkMethods.Keys)
            {
                columns.Add(new ConsoleColumn(key, Align.Right, Highlight.On));
            }

            ConsoleTable table = new ConsoleTable(columns.ToArray());

            foreach (string filePath in FilesForPath(inputPath))
            {
                long fileLengthBytes = new FileInfo(filePath).Length;

                ArgumentClass instance = default(ArgumentClass);
                List<string> row = new List<string>();

                // Use the loader to load the file; log name, size, load rate.
                MeasureResult load = Measure.Operation(() => instance = loader(filePath), MeasureSettings.Load);
                row.Add(Path.GetFileName(filePath));
                row.Add(Friendly.Size(fileLengthBytes));
                row.Add(Friendly.Rate(fileLengthBytes, load.Elapsed / load.Iterations));
                row.Add(Friendly.Size(load.AddedMemoryBytes));

                // Log action time per operation.
                foreach (string key in benchmarkMethods.Keys)
                {
                    Action<ArgumentClass> operation = benchmarkMethods[key];
                    MeasureResult opResult = Measure.Operation(() => operation(instance));
                    row.Add(Friendly.Time(opResult.SecondsPerIteration));
                }

                table.AppendRow(row);
            }
        }

        public static void RunFiles<OperationsClass, ArgumentClass>(string inputPath, Func<string, ArgumentClass> loader)
        {
            RunFiles<ArgumentClass>(typeof(OperationsClass), inputPath, loader);
        }

        /// <summary>
        ///  Return the list of files for a given path.
        ///  If it's a folder, list the files directly in the folder.
        ///  If it's a file, return just that file.
        /// </summary>
        internal static IEnumerable<string> FilesForPath(string inputPath)
        {
            if (Directory.Exists(inputPath))
            {
                return Directory.EnumerateFiles(inputPath).ToList();
            }
            else
            {
                return new string[] { inputPath };
            }
        }
    }
}
