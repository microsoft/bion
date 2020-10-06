namespace BSOA.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // Ensure sample file created
            Generator.EnsureSampleBuilt();

            Measure.Run<Operations>();
            //BenchmarkRunner.Run<Operations>();
        }
    }
}
