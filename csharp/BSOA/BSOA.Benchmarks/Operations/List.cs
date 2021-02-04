// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using BSOA.Test.Model.Log;

namespace BSOA.Benchmarks
{
    public class List
    {
        private Run _run;
        private List<Result> _results;

        public List()
        {
            _run = Generator.CreateOrLoad();
            _results = _run.Results.ToList();
        }

        [Benchmark]
        public void ListColumnEnumerate()
        {
            // Enumerate a list column (best case ListColumn / ColumnList enumeration speeds)
            long sum = 0;
            foreach (Result result in _results)
            {
                foreach (int value in result.Tags)
                {
                    sum += value;
                }
            }
        }

        [Benchmark]
        public void ListColumnFor()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                for (int i = 0; i < result.Tags.Count; ++i)
                {
                    sum += result.Tags[i];
                }
            }
        }

        [Benchmark]
        public void ListColumnForCached()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                IList<int> tags = result.Tags;
                int count = tags.Count;
                for (int i = 0; i < count; ++i)
                {
                    sum += tags[i];
                }
            }
        }
    }
}
