using BenchmarkDotNet.Running;

namespace BSOA.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            new Generator().Build();

            BenchmarkRunner.Run<Operations>();
        }
    }
}
