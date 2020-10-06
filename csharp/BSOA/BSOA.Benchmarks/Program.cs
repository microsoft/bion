using BSOA.Benchmarks.Diagnostics;

namespace BSOA.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ensure sample file created
            Generator.EnsureSampleBuilt();

            QuickBenchmarker.Run<Operations>();
            //BenchmarkRunner.Run<Operations>();
        }
    }
}
