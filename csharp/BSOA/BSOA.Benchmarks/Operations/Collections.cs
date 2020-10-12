using BenchmarkDotNet.Attributes;

// Uncomment line for model to test (they're signature identical)
using BSOA.Benchmarks.Model;
//using BSOA.Benchmarks.NonBsoaModel;

using System.Collections.Generic;
using System.Linq;

namespace BSOA.Benchmarks
{
    public class Collections
    {
        private Run _run;
        private List<Result> _results;

        public Collections()
        {
            _run = Generator.CreateOrLoad();
            _results = _run.Results.ToList();
        }

        [Benchmark]
        public void DictionaryReadSuccess()
        {
            // Benchmark reading a string from a Dictionary which will always be there
            // [Requires checking all other keys and UTF-8 decoding value]
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.Properties["Commit"].Length;
            }
        }

        //[Benchmark]
        public void DictionaryReadFail()
        {
            // Benchmark reading a string from a Dictionary which is not there
            // [Requires checking all keys, but no value decoding]
            long sum = 0;
            foreach (Result result in _results)
            {
                if (result.Properties.TryGetValue("NotThere", out string commit))
                {
                    sum += commit.Length;
                }
            }
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

        //[Benchmark]
        public void ListColumnFor()
        {
            long sum = 0;
            foreach (Result result in _results)
            {
                for(int i = 0; i < result.Tags.Count; ++i)
                {
                    sum += result.Tags[i];
                }
            }
        }

        //[Benchmark]
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
