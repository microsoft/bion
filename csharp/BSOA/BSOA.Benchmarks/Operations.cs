using BenchmarkDotNet.Attributes;

using BSOA.Benchmarks.Model;
using BSOA.IO;

using System.IO;

namespace BSOA.Benchmarks
{
    public class Operations
    {
        public const string SampleFilePath = "Sample.Run.bsoa";
        private Run _run;

        public Operations()
        {
            Run run = new Generator().Build();

            using (BinaryTreeWriter writer = new BinaryTreeWriter(File.Create(SampleFilePath)))
            {
                run.DB.Write(writer);
            }

            _run = run;
        }

        [Benchmark]
        public void IntegerOperation()
        {
            SumOfStartLine(_run);
        }

        [Benchmark]
        public void StringOperation()
        {
            SumOfStartLine(_run);
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
