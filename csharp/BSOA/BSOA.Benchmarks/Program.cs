using BenchmarkDotNet.Running;

using System;

namespace BSOA.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ensure sample file created
            Generator.EnsureSampleBuilt();

            if (args.Length > 0 && args[0].ToLowerInvariant().Contains("detailed"))
            {
                BenchmarkRunner.Run<Operations>();
            }
            else
            {
                Console.WriteLine("Quick benchmarks. Pass --detailed for Benchmark.net numbers.");
                QuickBenchmarker.Run<Operations>();
            }
        }
    }
}
