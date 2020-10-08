using BenchmarkDotNet.Running;

using BSOA.Column;

using System;

namespace BSOA.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ensure sample file created
            Generator.CreateOrLoad();

            if (args.Length > 0 && args[0].ToLowerInvariant().Contains("detailed"))
            {
                BenchmarkRunner.Run<Operations>();
            }
            else
            {
                Console.WriteLine("Quick benchmarks. Pass --detailed for Benchmark.net numbers.");
                QuickBenchmarker.Run<Operations>(new MeasureSettings(TimeSpan.FromSeconds(5), 1, 10000, false));
            }

            //Console.WriteLine($"Convert: {StringColumn.ConvertToStringCount:n0}\r\nReuse: {StringColumn.ReuseFromCacheCount:n0}");
        }
    }
}
