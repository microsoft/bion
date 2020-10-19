using System;
using System.Collections.Generic;
using System.IO;

namespace BSOA.Benchmarks
{
    public class QuickBenchmarker
    {
        private MeasureSettings _settings;
        private ConsoleTable _table;
        private DateTime _start;

        public QuickBenchmarker(MeasureSettings settings)
        {
            _settings = settings;
            _table = new ConsoleTable(new ConsoleColumn("Name"), new ConsoleColumn("Mean", Align.Right, Highlight.On));
            _start = DateTime.UtcNow;
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
                _table.AppendRow(methodName, Friendly.Time(result.SecondsPerIteration));
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
    }
}
