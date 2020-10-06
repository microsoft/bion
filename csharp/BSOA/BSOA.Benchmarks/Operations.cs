using BenchmarkDotNet.Attributes;

using BSOA.Benchmarks.Model;

namespace BSOA.Benchmarks
{
    public class Operations
    {
        private Run _run;

        public Operations()
        {
            _run = Generator.CreateOrLoad();
        }

        [Benchmark]
        public void Nothing()
        {

        }

        [Benchmark]
        public void IntegerOperation()
        {
            SumOfStartLine(_run);
        }

        [Benchmark]
        public void StringOperation()
        {
            SumOfMessageLength(_run);
        }

        private long SumOfStartLine(Run run)
        {
            long sum = 0;

            foreach (Result result in run.Results)
            {
                sum += result.StartLine;
            }

            return sum;
        }

        private int SumOfMessageLength(Run run)
        {
            int sum = 0;

            foreach(Result result in run.Results)
            {
                sum += result.Message.Length;
            }

            return sum;
        }
    }
}
