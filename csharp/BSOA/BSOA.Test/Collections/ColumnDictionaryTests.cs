using BSOA.Collections;

using System;
using System.Collections.Generic;

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

            IDictionary<string, string> row = DictionaryColumnTests.SampleRow();
            Assert.True(0 == ColumnDictionary<string, string>.Empty.Count);
            Assert.False(row.IsReadOnly);

            // Test Empty Dictionary
            Assert.False(row.TryGetValue(sampleName, out retrievedValue));
            Assert.False(row.ContainsKey(sampleName));
            Assert.False(row.Remove(sampleName));
            Assert.Equal(0, row.Count);
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
            Assert.False(row.Contains(new KeyValuePair<string, string>(sampleName, secondValue)));
            Assert.False(row.Contains(new KeyValuePair<string, string>(unusedName, sampleValue)));
            Assert.False(row.ContainsKey(unusedName));
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
            ((ColumnDictionary<string, string>)row).SetTo(expected);
            CollectionReadVerifier.VerifySame<KeyValuePair<string, string>>(expected, row);

            // Verify other collection manipulation
            // NOTE: Must use unique keys, because Add(KeyValuePair) will throw for a duplicate key
            CollectionChangeVerifier.VerifyCollection(row, (i) => new KeyValuePair<string, string>(i.ToString(), i.ToString()));

            Assert.Throws<IndexOutOfRangeException>(() => new ColumnDictionary<string, string>(null, -1));
        }
    }
}
