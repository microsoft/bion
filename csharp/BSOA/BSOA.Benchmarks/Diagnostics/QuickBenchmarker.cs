using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BSOA.Benchmarks
{
    public class QuickBenchmarker
    {
        public const string BaselinePath = "Baseline.md";

        private MeasureSettings _settings;
        private DateTime _start;
        private ConsoleTable _table;
        private Dictionary<string, double> _baseline;

        public QuickBenchmarker(MeasureSettings settings)
        {
            _settings = settings;
            _start = DateTime.UtcNow;

            _table = new ConsoleTable(
                new ConsoleColumn("Name"),
                new ConsoleColumn("Mean", Align.Right, Highlight.On),
                new ConsoleColumn("Baseline", Align.Right),
                new ConsoleColumn("Base/Mean", Align.Right));

            _baseline = ParseBaseline();
        }

        /// <summary>
        ///  Similar to a fast, simple Benchmark.net Benchmarker.Run.
        ///  Benchmarks each method on the given class with the [Benchmark]
        ///  attribute.
        /// </summary>
        /// <param name="typeWithBenchmarkMethods">Type containing methods to benchmark</typeparam>
        /// <param name="settings">Measurement settings, or null for defaults</param>
        public void Run(Type typeWithBenchmarkMethods)
        {
            Dictionary<string, Action> benchmarkMethods = BenchmarkReflector.BenchmarkMethods<Action>(typeWithBenchmarkMethods);

            foreach (string methodName in benchmarkMethods.Keys)
            {
                MeasureResult result = Measure.Operation(benchmarkMethods[methodName], _settings);

                double baselineTime;
                if (!_baseline.TryGetValue(methodName, out baselineTime)) { baselineTime = 0.0; }

                _table.AppendRow(
                    methodName, 
                    Friendly.Time(result.SecondsPerIteration),
                    Friendly.Time(baselineTime),
                    Friendly.Ratio(baselineTime, result.SecondsPerIteration));
            }

            _table.Save(File.Create($"Benchmarks_{_start:yyyyMMddhhmmss}.md"));
        }

        /// <summary>
        ///  Similar to a fast, simple Benchmark.net Benchmarker.Run.
        ///  Benchmarks each method on the given class with the [Benchmark]
        ///  attribute.
        /// </summary>
        /// <typeparam name="T">Type containing methods to benchmark</typeparam>
        /// <param name="settings">Measurement settings, or null for defaults</param>
        public void Run<T>()
        {
            Run(typeof(T));
        }

        private static Dictionary<string, double> ParseBaseline()
        {
            Dictionary<string, double> baseline = new Dictionary<string, double>();

            if (File.Exists(BaselinePath))
            {
                try
                {
                    IEnumerable<string> baselineFileLines = File.ReadLines(BaselinePath);
                    string headingLine = baselineFileLines.First();

                    List<string> columnNames = headingLine
                        .Split('|')
                        .Select((cell) => cell.Trim())
                        .ToList();

                    int meanIndex = columnNames.IndexOf("Mean");
                    if (meanIndex == -1)
                    {
                        throw new FormatException($"Unable to find 'Mean' column in {BaselinePath}; heading line was: \"{headingLine}\".");
                    }

                    foreach (string contentLine in baselineFileLines.Skip(2))
                    {
                        string[] cells = contentLine.Split('|');
                        string functionName = cells[1].Trim();
                        double meanSeconds = Friendly.ParseTime(cells[meanIndex]);
                        baseline[functionName] = meanSeconds;
                    }
                }
                catch (FileNotFoundException)
                {
                    // Return empty baseline
                }
            }

            return baseline;
        }
    }
}
