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

        [BenchmarkDotNet.Attributes.Benchmark]
        public void Nothing()
        {

        }

        [BenchmarkDotNet.Attributes.Benchmark]
        public void IntegerOperation()
        {
            SumOfStartLine(_run);
        }

        [BenchmarkDotNet.Attributes.Benchmark]
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
