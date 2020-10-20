using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BSOA.Benchmarks
{
    public class QuickBenchmarker
    {
        // Baseline file to compare against and failure threshold (< 80% of baseline speed)
        public const string OutputFolderPath = "Reports";
        public const string BaselinePath = "Baseline.md";
        public const string BaselineColumnName = "Baseline";
        public const string LastColumnName = "Last";
        public const string CalibrationMethodName = "Calibration";

        private double _failThreshold = 0.8d;
        private MeasureSettings _settings;
        private ConsoleTable _table;
        private Dictionary<string, Dictionary<string, double>> _baselines;

        public bool HasFailures { get; private set; }
        public string OutputPath { get; }

        public QuickBenchmarker(MeasureSettings settings)
        {
            _settings = settings;
            _baselines = LoadBaselines();

            List<TableCell> columns = new List<TableCell>();
            columns.Add(new TableCell("Name"));
            columns.Add(new TableCell("Mean", Align.Right, TableColor.Green));

            foreach (string baseline in _baselines.Keys)
            {
                columns.Add(new TableCell(baseline, Align.Right));
                columns.Add(new TableCell("/Mean", Align.Right));
            }

            _table = new ConsoleTable(columns);

            Directory.CreateDirectory(OutputFolderPath);
            OutputPath = Path.GetFullPath($"{OutputFolderPath}/Benchmarks.{DateTime.UtcNow:yyyyMMddhhmmss}.md");

            Calibrate();
        }

        /// <summary>
        ///  Similar to Benchmark.net's Benchmarker.Run.
        ///  Benchmarks each method on the given class with the [Benchmark] attribute.
        ///  Less accurate but much faster to complete than Benchmark.net runs.
        /// </summary>
        /// <typeparam name="T">Type containing methods to benchmark</typeparam>
        /// <param name="settings">Measurement settings, or null for defaults</param>
        public void Run<T>()
        {
            Run(typeof(T));
        }

        /// <summary>
        ///  Similar to Benchmark.net's Benchmarker.Run.
        ///  Benchmarks each method on the given class with the [Benchmark] attribute.
        ///  Less accurate but much faster to complete than Benchmark.net runs.
        /// </summary>
        /// <param name="typeWithBenchmarkMethods">Type containing methods to benchmark</typeparam>
        /// <param name="settings">Measurement settings, or null for defaults</param>
        public void Run(Type typeWithBenchmarkMethods)
        {
            Dictionary<string, Action> benchmarkMethods = BenchmarkReflector.BenchmarkMethods<Action>(typeWithBenchmarkMethods);

            foreach (var method in benchmarkMethods)
            {
                Run(method.Key, method.Value);
            }

            _table.Save(File.Create(OutputPath));
        }

        /// <summary>
        ///  Similar to Benchmark.net's Benchmarker.Run.
        ///  Benchmarks each method on the given class with the [Benchmark] attribute.
        ///  Less accurate but much faster to complete than Benchmark.net runs.
        /// </summary>
        /// <param name="typeWithBenchmarkMethods">Type containing methods to benchmark</typeparam>
        /// <param name="settings">Measurement settings, or null for defaults</param>
        public MeasureResult Run(string methodName, Action method)
        {
            List<TableCell> row = new List<TableCell>();

            // Benchmark this method
            MeasureResult result = Measure.Operation(method, _settings);

            // Report current time
            row.Add(TableCell.String(methodName));
            row.Add(TableCell.Time(result.SecondsPerIteration));

            // Compare to each loaded benchmark
            foreach (var baseline in _baselines)
            {
                double baselineTime;
                if (!baseline.Value.TryGetValue(methodName, out baselineTime)) { baselineTime = 0.0; }

                bool failed = false;
                row.Add(TableCell.Time(baselineTime));
                row.Add(TableCell.Ratio(baselineTime, result.SecondsPerIteration, _failThreshold, ref failed));

                if (baseline.Key == BaselineColumnName) { HasFailures |= failed; }
            }

            _table.AppendRow(row);
            return result;
        }

        internal void Calibrate()
        {
            MeasureResult calibrationResult = Run(CalibrationMethodName, CalibrationFunction);

            // Ensure a "failure" in calibration isn't counted
            HasFailures = false;

            // Calibrate fail threshold based on baseline (if Calibrate took twice as long, expect all other methods to as well)
            Dictionary<string, double> baseline = _baselines.Values.FirstOrDefault();
            if (baseline != null && baseline.TryGetValue(CalibrationMethodName, out double baselineSeconds))
            {
                _failThreshold = _failThreshold * (calibrationResult.SecondsPerIteration / baselineSeconds);
            }
        }

        private static void CalibrationFunction()
        {
            long sum = 0;

            for (int i = 0; i < 1000; ++i)
            {
                sum += i;
            }
        }

        private static void Nothing()
        {
            // Available to test overhead of benchmarking infrastructure 
            // (2.5 ns in Debug typical)
        }

        internal static Dictionary<string, Dictionary<string, double>> LoadBaselines()
        {
            Dictionary<string, Dictionary<string, double>> baselines = new Dictionary<string, Dictionary<string, double>>();
            Dictionary<string, double> baseline;

            if (TryLoadBaseline(BaselinePath, out baseline))
            {
                baselines[BaselineColumnName] = baseline;
            }

            if (Directory.Exists(OutputFolderPath) && TryLoadBaseline(Directory.GetFiles(OutputFolderPath).LastOrDefault(), out baseline))
            {
                baselines[LastColumnName] = baseline;
            }

            return baselines;
        }

        /// <summary>
        ///  Load and Parse a previously written Markdown report, returning the previous
        ///  function names and Mean times in a Dictionary to use as baseline values for
        ///  the current run.
        /// </summary>
        /// <remarks>
        ///  Requires Function Name to be the first column and Mean to be the second column.
        /// </remarks>
        /// <param name="baselineFilePath">File Path of Baseline ConsoleTable Markdown to load</param>
        /// <param name="result">Loaded baseline, if found and parsed successfully</param>
        /// <returns>True if baseline loaded, False otherwise</returns>
        internal static bool TryLoadBaseline(string baselineFilePath, out Dictionary<string, double> result)
        {
            result = null;
            if (baselineFilePath == null || !File.Exists(baselineFilePath)) { return false; }

            Dictionary<string, double> baseline = new Dictionary<string, double>();

            try
            {
                IEnumerable<string> baselineFileLines = File.ReadLines(baselineFilePath);
                string headingLine = baselineFileLines.First();

                List<string> columnNames = headingLine
                    .Split('|')
                    .Select((cell) => cell.Trim())
                    .ToList();

                foreach (string contentLine in baselineFileLines.Skip(2))
                {
                    string[] cells = contentLine.Split('|');
                    string functionName = cells[1].Trim();
                    double meanSeconds = Format.ParseTime(cells[2].Trim());
                    baseline[functionName] = meanSeconds;
                }
            }
            catch (FileNotFoundException)
            {
                // Return no baseline available
                return false;
            }
            catch (FormatException) when (!Debugger.IsAttached)
            {
                // Return no baseline available
                Console.WriteLine($"Unable to parse baseline \"{baselineFilePath}\". Excluding.");
                return false;
            }

            result = baseline;
            return true;
        }
    }
}
