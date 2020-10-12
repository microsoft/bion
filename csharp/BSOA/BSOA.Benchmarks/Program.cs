using BenchmarkDotNet.Running;

using System;

namespace BSOA.Benchmarks
{
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

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].ToLowerInvariant().Contains("detailed"))
            {
                BenchmarkRunner.Run<Basics>();
                BenchmarkRunner.Run<Strings>();
                BenchmarkRunner.Run<Collections>();
            }
            else
            {
                Console.WriteLine("Quick benchmarks. Pass --detailed for Benchmark.net numbers.");
                MeasureSettings settings = new MeasureSettings(TimeSpan.FromSeconds(5), 1, 10000, false);

                //QuickBenchmarker.Run<Basics>(settings);
                QuickBenchmarker.Run<Strings>(settings);
                //QuickBenchmarker.Run<Collections>(settings);
            }
        }
    }
}
