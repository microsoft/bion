// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using BSOA.Test.Model.Log;

namespace BSOA.Benchmarks
{
    public class Basics
    {
        private Run _run;
        private List<Result> _results;

        public Basics()
        {
            _run = Generator.CreateOrLoad();
            _results = _run.Results.ToList();
        }

        [Benchmark]
        public void Enumerate()
        {
            // Benchmark enumerating a collection (RefList to another table) using a foreach loop.
            // NOTE: ForEach is the fastest option for BSOA, as it retrieves the real list of indices and count only once.
            long count = 0;
            foreach (Result result in _run.Results)
            {
                count++;
            }
        }

        [Benchmark]
        public void ForLoop()
        {
            // Benchmark enumerating a collection via for loop.
            // NOTE: Slower; each access of Count and indexer must re-retrieve the real index list.
            long count = 0;
            for (int i = 0; i < _run.Results.Count; ++i)
            {
                Result result = _run.Results[i];
                count++;
            }
        }

        [Benchmark]
        public void IntegerSum()
        {
            // Benchmark foreaching over a set and summing an integer column (minimal possible work on cheapest enumeration)
            long sum = 0;
            foreach (Result result in _run.Results)
            {
                sum += result.StartLine;
            }
        }

        [Benchmark]
        public void IntegerSumCached()
        {
            // Benchmark summing an integer on pre-cached collection (avoid enumeration and construction costs; pure measure of integer retrieval from column)
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.StartLine;
            }
        }

        [Benchmark]
        public void DateTimeCached()
        {
            // Benchmark DateTime retrieval and decoding (without enumeration or construction costs)
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.WhenDetectedUtc.Ticks;
            }
        }

        [Benchmark]
        public void BooleanCached()
        {
            // Benchmark boolean retrieval and decoding (without enumeration or construction costs)
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += (result.IsActive ? 1 : 0);
            }
        }

        [Benchmark]
        public void EnumCached()
        {
            // Benchmark enum retrieval and decoding (without enumeration or construction costs)
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += (result.BaselineState == BaselineState.Unchanged ? 1 : 0);
            }
        }
    }
}
