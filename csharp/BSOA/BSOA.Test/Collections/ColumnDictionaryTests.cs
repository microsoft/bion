// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;

using BSOA.Collections;

using Xunit;

namespace BSOA.Test.Collections
{
    public class ColumnDictionaryTests
    {
        [Fact]
        public void ColumnDictionary_Basics()
        {
            string sampleName = "Name";
            string sampleValue = "Scott";
            string retrievedValue = null;

            string secondName = "City";
            string secondValue = "Redmond";

            string unusedName = "Unused";

            Dictionary<string, string> expected = new Dictionary<string, string>();

            ColumnDictionary<string, string> row = DictionaryColumnTests.SampleRow();
            Assert.True(0 == ColumnDictionary<string, string>.Empty.Count);
            Assert.False(row.IsReadOnly);

            // Test Empty Dictionary
            Assert.False(row.TryGetValue(sampleName, out retrievedValue));
            Assert.False(row.ContainsKey(sampleName));
            Assert.False(row.Remove(sampleName));
            Assert.True(0 == row.Count);
            Assert.Empty(row.Keys);
            Assert.Empty(row.Values);

            // Add a single value and test results
            expected[sampleName] = sampleValue;
            row[sampleName] = sampleValue;
            CollectionReadVerifier.VerifySame(expected, row);
            CollectionReadVerifier.VerifySame(expected.Keys, row.Keys);
            CollectionReadVerifier.VerifySame(expected.Values, row.Values);

            // Add a second value and verify
            expected.Add(secondName, secondValue);
            row.Add(new KeyValuePair<string, string>(secondName, secondValue));
            CollectionReadVerifier.VerifySame(expected, row);
            CollectionReadVerifier.VerifySame(expected.Keys, row.Keys);
            CollectionReadVerifier.VerifySame(expected.Values, row.Values);

            // Negative (missing item / already added item) cases
            Assert.True(false == row.Contains(new KeyValuePair<string, string>(sampleName, secondValue)));
            Assert.True(false == row.Contains(new KeyValuePair<string, string>(unusedName, sampleValue)));
            Assert.True(false == row.ContainsKey(unusedName));
            Assert.Throws<KeyNotFoundException>(() => row[unusedName]);
            Assert.Throws<ArgumentException>(() => row.Add(new KeyValuePair<string, string>(sampleName, secondValue)));

            // Change value and verify, then change back
            Assert.Equal(expected[sampleName], row[sampleName]);
            expected[sampleName] = secondValue;
            row[sampleName] = secondValue;
            Assert.Equal(expected[sampleName], row[sampleName]);
            CollectionReadVerifier.VerifySame(expected, row);
            CollectionReadVerifier.VerifySame(expected.Keys, row.Keys);
            CollectionReadVerifier.VerifySame(expected.Values, row.Values);

            expected[sampleName] = sampleValue;
            row[sampleName] = sampleValue;
            Assert.Equal(expected[sampleName], row[sampleName]);

            // Remove
            Assert.True(row.Remove(secondName));
            Assert.False(row.Remove(secondName));
            Assert.False(row.ContainsKey(secondName));
            Assert.False(row.Remove(new KeyValuePair<string, string>(unusedName, sampleValue)));
            Assert.False(row.Remove(new KeyValuePair<string, string>(sampleName, secondValue)));
            Assert.True(row.Remove(new KeyValuePair<string, string>(sampleName, sampleValue)));
            Assert.Empty(row);

            // SetTo
            row.SetTo(expected);
            CollectionReadVerifier.VerifySame<KeyValuePair<string, string>>(expected, row);

            // Create another Dictionary with the same values inserted in a different order
            ColumnDictionary<string, string> row2 = DictionaryColumnTests.SampleRow();
            row2[secondName] = secondValue;
            row2[sampleName] = sampleValue;

            // Equals (order-independent)
            Assert.True(row.Equals(row));
            Assert.True(row.Equals(row2));
            Assert.False(row.Equals(ColumnDictionary<string, string>.Empty));
            Assert.False(row.Equals(null));

            // Operators
            Assert.True(row == row2);
            Assert.False(row == ColumnDictionary<string, string>.Empty);
            Assert.True(ColumnDictionary<string, string>.Empty != row);
            Assert.False(row2 != row);
            Assert.True((ColumnDictionary<string, string>)null == null);
            Assert.False((ColumnDictionary<string, string>)null != null);

            // GetHashCode (order-independent)
            Assert.Equal(row.GetHashCode(), row2.GetHashCode());
            Assert.NotEqual(row.GetHashCode(), ColumnDictionary<string, string>.Empty.GetHashCode());

            // GetHashCode handles null key/values safely
            row[null] = null;
            Assert.Equal(row.GetHashCode(), row2.GetHashCode());

            // Verify other collection manipulation
            // NOTE: Must use unique keys, because Add(KeyValuePair) will throw for a duplicate key
            CollectionChangeVerifier.VerifyCollection(row, (i) => new KeyValuePair<string, string>(i.ToString(), i.ToString()));

            if (!Debugger.IsAttached)
            {
                Assert.Throws<IndexOutOfRangeException>(() => ColumnDictionary<string, string>.Get(null, -1));
            }
        }
    }
}
