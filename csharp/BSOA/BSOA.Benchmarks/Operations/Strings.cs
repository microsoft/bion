// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using BSOA.Benchmarks.Model;

namespace BSOA.Benchmarks
{
    public class Strings
    {
        private Run _run;
        private List<Result> _results;

        public Strings()
        {
            _run = Generator.CreateOrLoad();
            _results = _run.Results.ToList();
        }

        [Benchmark]
        public void StringNulls()
        {
            // Benchmark reading a string column which is always nulls (no UTF-8 conversion costs)
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.Guid?.Length ?? 0;
            }
        }

        [Benchmark]
        public void StringDistinct()
        {
            // Benchmark reading a string column in a DistinctColumn (few distinct values; values will be cached as .NET strings)
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.RuleId.Length;
            }
        }

        [Benchmark]
        public void String()
        {
            // Benchmark reading a string column with all non-null values (requiring a conversion per value)
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.Message.Length;
            }
        }

        [Benchmark]
        public void String2x()
        {
            // Benchmark referring to the same string value twice in a row (testing caching of last-read-index; likely usage pattern)
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.Message.Length + result.Message.Length;
            }
        }
    }
}
