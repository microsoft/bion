using BenchmarkDotNet.Attributes;

using BSOA.Benchmarks.Model;

using System.Collections.Generic;
using System.Linq;

namespace BSOA.Benchmarks
{
    public class Operations
    {
        private Run _run;
        private List<Result> _results;

        public Operations()
        {
            _run = Generator.CreateOrLoad();
            _results = _run.Results.ToList();
        }

        //[Benchmark]
        public void Nothing()
        {

        }

        //[Benchmark]
        public void Enumerate()
        {
            long count = 0;
            foreach (Result result in _run.Results)
            {
                count++;
            }
        }

        [Benchmark]
        public void IntegerSum()
        {
            long sum = 0;
            foreach (Result result in _run.Results)
            {
                sum += result.StartLine;
            }
        }

        [Benchmark]
        public void IntegerSumCachedList()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.StartLine;
            }
        }

        //[Benchmark]
        public void DateTimeCachedList()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.WhenDetectedUtc.Ticks;
            }
        }

        [Benchmark]
        public void StringCachedList()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.Message.Length;
            }
        }

        [Benchmark]
        public void StringOperation2x()
        {
            long sum = 0;
            foreach (Result result in _run.Results)
            {
                sum += result.Message.Length + result.Message.Length;
            }
        }

        // Conclusions:
        //  - IntColumn value retrieval cost is minimal vs. normal integer field on List<struct>.
        //  - DateTime conversion is still extremely cheap.

        // Normal classes with 1,000 Results (NonBsoaModel)
        //  foreach (Result result in run.Results) { sum += result.StartLine; }         4 us.
        //  foreach (Result result in run.Results) { sum += result.Message.Length; }    4 us.
        
        // IntColumn retrieve with pre-cached List:                                   5.6 us.
        // IntColumn retrieve with pre-cached List via IEnumerable<Result>:          10.6 us.
        // DateTimeColumn retrieve with pre-cached List:                              9.0 us.
        // String, cached in Dictionary, pre-cached List:                            22.5 us.

        // BSOA Model (default, pre-optimizations):
        //  foreach (Result result in run.Results) { sum += result.StartLine; }       66 us.
        //  foreach (Result result in run.Results) { sum += result.Message.Length; }  150 us.

        // BSOA ListEnumerator cache count:
        //  foreach (Result result in run.Results) { sum += result.StartLine; }       40 us.
        //  foreach (Result result in run.Results) { sum += result.Message.Length; }  122 us.

        // BSOA EnumeratorConverter:
        //  foreach (Result result in run.Results) { sum += result.StartLine; }           22 us.
        //  foreach (Result result in run.Results) { sum += result.Message.Length; }      98 us.
        //  foreach (Result result in run.Results) { 2x sum += result.Message.Length; }  166 us.

        // BSOA struct-y Result: (Reverted)
        //  foreach (Result result in run.Results) { sum += result.StartLine; }       19 us.
        //  foreach (Result result in run.Results) { sum += result.Message.Length; }  90 us.

        // BSOA DistinctColumn; string cache on read:
        //  foreach (Result result in run.Results) { sum += result.Message.Length; }     34 us.
        //  foreach (Result result in run.Results) { 2x sum += result.Message.Length; }  58 us.

        // BSOA DistinctColumn but no cache:
        //  foreach (Result result in run.Results) { sum += result.Message.Length; }     112 us.
        //  foreach (Result result in run.Results) { 2x sum += result.Message.Length; }  190 us.

        // Ensure Messages Unique:
        //  foreach (Result result in run.Results) { sum += result.StartLine; }           22 us.
        //  foreach (Result result in run.Results) { sum += result.Message.Length; }      98 us.
        //  foreach (Result result in run.Results) { 2x sum += result.Message.Length; }  161 us.
    }
}
