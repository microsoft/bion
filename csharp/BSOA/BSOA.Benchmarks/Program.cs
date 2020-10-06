using BenchmarkDotNet.Running;

namespace BSOA.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ensure sample file created
            Generator.EnsureSampleBuilt();

            if (args.Length > 0 && args[0].ToLowerInvariant() == "quick")
            {
                QuickBenchmarker.Run<Operations>();
            }
            else
            {
                BenchmarkRunner.Run<Operations>();
            }
        }
    }
}
