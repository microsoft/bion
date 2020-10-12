using BenchmarkDotNet.Attributes;

using BSOA.Benchmarks.Model;
//using BSOA.Benchmarks.NonBsoaModel;

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

        // TODO:
        //  Add test for enumerate Dictionary and List columns; try for loop for List column. Look for caching opportunities.

        // Benchmark Costs (1,000 elements)
        // ================================
        //  Non-BSOA List<struct>     , sum int             4.00 us
        //  BSOA class, cached in List, sum int             5.75 us (BSOA int retrieval very cheap)
        //                            , sum DateTime        8.00 us (DateTime construction cheap)
        //                            , sum bool            6.75 us (bool cheap)
        //                            , sum enum            7.80 us (enum cast cheap)
        //   [all strings cached]     , sum string length  16.50 us
        //  DistinctColumn<string> (cached), sum length     9.00 us (faster than in StringColumn; List lookup vs. Dictionary underneath)

        //  List as IEnumerable<T> enumerate               10.00 us (2x strongly-typed List enumerate)
        //  BSOA List enumerate                            20.00 us (more expensive, but not if several operations on each item)

        // Improvements
        // ============
        //  - ListEnumerator caches length (65 -> 40 us)
        //  - EnumeratorConverter          (40 -> 20 us)
        //  - DistinctColumn caching       (83 ->  9 us for Message.Length Sum)
        //  - For Loop, no list caching    (178 -> 70 us)

        // Learnings
        // =========
        //  - In StringColumn, IsNull check first is worthwhile (much faster for all null columns, minimal impact on no-null columns).
        //  - DistinctColumn caching is very worthwhile (string form of values kept anyway to look up index on set; keeping another list of references saves all conversions on get).
        //  - StringColumn caching too expensive with "remove oldest from cache" (remove too expensive vs. convert)
        //  - StringColumn caching only worthwhile if the cache hits relatively often; usage pattern will vary.
        //  - StringColumn "cache last read" is often helpful and minimal overhead otherwise.
        //  - Columns 
        
        //[Benchmark]
        public void Nothing()
        {

        }

        //[Benchmark]
        public void Enumerate()
        {
            // NOTE: ForEach is much faster for BSOA, as it can retrieve the real index list once.
            long count = 0;
            foreach (Result result in _run.Results)
            {
                count++;
            }
        }

        //[Benchmark]
        public void ForLoop()
        {
            // NOTE: Slower; each access of Count and indexer must re-retrieve the real index list.
            long count = 0;
            for (int i = 0; i < _run.Results.Count; ++i)
            {
                Result result = _run.Results[i];
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

        //[Benchmark]
        public void IntegerSumCached()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.StartLine;
            }
        }

        //[Benchmark]
        public void DateTimeCached()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.WhenDetectedUtc.Ticks;
            }
        }

        //[Benchmark]
        public void BooleanCached()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += (result.IsActive ? 1 : 0);
            }
        }

        //[Benchmark]
        public void EnumCached()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += (result.BaselineState == BaselineState.Unchanged ? 1 : 0);
            }
        }

        //[Benchmark]
        public void StringNulls()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.RuleId?.Length ?? 0;
            }
        }

        //[Benchmark]
        public void StringCached()
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
            foreach (Result result in _results)
            {
                sum += result.Message.Length + result.Message.Length;
            }
        }

        [Benchmark]
        public void DictionaryReadSuccess()
        {
            long sum = 0;
            int badCount = 0;
            foreach (Result result in _results)
            {
                if (result.Properties.TryGetValue("Commit", out string commit) && commit != null)
                {
                    sum += commit.Length;
                }
                else
                {
                    badCount++;
                }
            }
        }
    }
}
