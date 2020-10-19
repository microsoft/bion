using System;
using System.Collections.Generic;

namespace BSOA.Benchmarks
{
    public static class QuickBenchmarker
    {
        private static ConsoleTable _currentTable;

        /// <summary>
        ///  Similar to a fast, simple Benchmark.net Benchmarker.Run.
        ///  Benchmarks each method on the given class with the [Benchmark]
        ///  attribute.
        /// </summary>
        /// <param name="typeWithBenchmarkMethods">Type containing methods to benchmark</typeparam>
        /// <param name="settings">Measurement settings, or null for defaults</param>
        public static void Run(Type typeWithBenchmarkMethods, MeasureSettings settings = null)
        {
            Dictionary<string, Action> benchmarkMethods = BenchmarkReflector.BenchmarkMethods<Action>(typeWithBenchmarkMethods);

            if (_currentTable == null)
            {
                _currentTable = new ConsoleTable(new ConsoleColumn("Name"), new ConsoleColumn("Mean", Align.Right, Highlight.On));
            }

            foreach (string methodName in benchmarkMethods.Keys)
            {
                MeasureResult result = Measure.Operation(benchmarkMethods[methodName], settings);
                _currentTable.AppendRow(methodName, Friendly.Time(result.SecondsPerIteration));
            }
        }

        /// <summary>
        ///  Similar to a fast, simple Benchmark.net Benchmarker.Run.
        ///  Benchmarks each method on the given class with the [Benchmark]
        ///  attribute.
        /// </summary>
        /// <typeparam name="T">Type containing methods to benchmark</typeparam>
        /// <param name="settings">Measurement settings, or null for defaults</param>
        public static void Run<T>(MeasureSettings settings = null)
        {
            Run(typeof(T), settings);
        }
    }
}
