using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RoughBench
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

            List<TableCell> columns = new List<TableCell>()
            {
                new TableCell("File"),
                new TableCell("Size", Align.Right),
                new TableCell("Load", Align.Right),
                new TableCell("RAM", Align.Right, TableColor.Green)
            };

            foreach (string key in benchmarkMethods.Keys)
            {
                columns.Add(new TableCell(key, Align.Right, TableColor.Green));
            }

            ConsoleTable table = new ConsoleTable(columns.ToArray());

            foreach (string filePath in FilesForPath(inputPath))
            {
                long fileLengthBytes = new FileInfo(filePath).Length;

                ArgumentClass instance = default(ArgumentClass);
                List<TableCell> row = new List<TableCell>();

                // Use the loader to load the file; log name, size, load rate.
                MeasureResult load = Measure.Operation(() => instance = loader(filePath), MeasureSettings.Load);
                row.Add(TableCell.String(Path.GetFileName(filePath)));
                row.Add(TableCell.Size(fileLengthBytes));
                row.Add(TableCell.Rate(fileLengthBytes, load.SecondsPerIteration));
                row.Add(TableCell.Size(load.AddedMemoryBytes));

                // Log action time per operation.
                foreach (string key in benchmarkMethods.Keys)
                {
                    Action<ArgumentClass> operation = benchmarkMethods[key];
                    MeasureResult opResult = Measure.Operation(() => operation(instance));
                    row.Add(TableCell.Time(opResult.SecondsPerIteration));
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
        public static IEnumerable<string> FilesForPath(string inputPath)
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
