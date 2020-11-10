// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

using BenchmarkDotNet.Attributes;

using BSOA.Column;
using BSOA.Model;
using BSOA.Test.Model.Log;

namespace BSOA.Benchmarks
{
    public class Dictionary
    {
        private Run _run;
        private List<Result> _results;

        private DictionaryColumn<int, int> _nonStringColumn;

        public Dictionary()
        {
            _run = Generator.CreateOrLoad();
            _results = _run.Results.ToList();
            NonStringDictionarySetup();
        }

        private void NonStringDictionarySetup()
        {
            _nonStringColumn = new DictionaryColumn<int, int>(new NumberColumn<int>(0), new NumberColumn<int>(0), Nullability.NullsDisallowed);

            for (int row = 0; row < 1000; ++row)
            {
                IDictionary<int, int> dictionary = _nonStringColumn[row];

                for (int key = 0; key < 128; ++key)
                {
                    dictionary[key * 2] = key * 4;
                }
            }
        }

        [Benchmark]
        public void DictionaryReadSuccess()
        {
            // Benchmark reading a string from a Dictionary which will always be there
            // [Requires checking all other keys and UTF-8 decoding value]
            long sum = 0;
            foreach (Result result in _results)
            {
                // Read a key near the middle when sorted but late in insertion order
                sum += result.Properties["FirstDetectedUtc"].Length;
            }
        }

        [Benchmark]
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
        public void Dictionary2x()
        {
            // Benchmark reading a string from a Dictionary which is not there
            // [Requires checking all keys, but no value decoding]
            long sum = 0;
            foreach (Result result in _results)
            {
                sum += result.Properties.Count;
                if (result.Properties.ContainsKey("NotThere"))
                {
                    sum += 1;
                }
            }
        }

        [Benchmark]
        public void DictionaryAddRemove()
        {
            // Benchmark adding and removing a key
            foreach (Result result in _results)
            {
                IDictionary<string, string> properties = result.Properties;

                // Add a key
                properties["OrganizationName"] = "mseng";

                // Remove it
                properties.Remove("OrganizationName");
            }
        }

        [Benchmark]
        public void DictionaryInt2x()
        {
            // Benchmark reading from a Dictionary<int, int>; one key which is there and one which isn't
            int rows = _nonStringColumn.Count;
            long sum = 0;
            for (int row = 0; row < rows; ++row)
            {
                IDictionary<int, int> dictionary = _nonStringColumn[row];
                sum += dictionary[30];
                if (dictionary.TryGetValue(21, out int value)) { sum += value; }
            }
        }

        // TODO: Dictionary, string keys exceed DistinctColumn limit (will be terrible, but better with sorted keys)
    }
}
