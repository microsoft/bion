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

        // Benchmark Costs (1,000 elements)
        // ================================
        //  Non-BSOA List<struct>     , sum int             4.00 us
        //  BSOA class, cached in List, sum int             5.75 us (BSOA int retrieval very cheap)
        //                            , sum DateTime        8.00 us (DateTime construction cheap)
        //                            , sum bool            6.75 us (bool cheap)
        //                            , sum enum            7.80 us (enum cast cheap)
        //   [all strings cached]     , sum string length  16.50 us
        //  DistinctColumn, cached strings, sum length      

        //  List as IEnumerable<T> enumerate               10.00 us (2x strongly-typed List enumerate)
        //  BSOA List enumerate                            20.00 us (more expensive, but not if several operations on each item)

        // Improvements
        // ============
        //  - ListEnumerator caches length (65 -> 40 us)
        //  - EnumeratorConverter          (40 -> 20 us)

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

        //[Benchmark]
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
        public void BooleanCachedList()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += (result.IsActive ? 1 : 0);
            }
        }

        //[Benchmark]
        public void EnumCachedList()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += (result.BaselineState == BaselineState.Unchanged ? 1 : 0);
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

        //[Benchmark]
        public void StringCached2x()
        {
            long sum = 0;
            foreach (Result result in _run.Results)
            {
                sum += result.Message.Length + result.Message.Length;
            }
        }

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
