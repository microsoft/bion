using BSOA.Collections;

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

            Dictionary<string, string> expected = new Dictionary<string, string>()
            {
                [sampleName] = sampleValue,
                [secondName] = secondValue
            };

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
            row[sampleName] = sampleValue;
            Assert.Equal(1, row.Count);
            Assert.True(row.TryGetValue(sampleName, out retrievedValue));
            Assert.Equal(sampleValue, retrievedValue);
            Assert.Equal(sampleValue, row[sampleName]);
            Assert.True(row.ContainsKey(sampleName));
            Assert.Equal($"{sampleName}", string.Join(", ", row.Keys));
            Assert.Equal($"{sampleValue}", string.Join(", ", row.Values));

            // Add a second value and verify
            row.Add(new KeyValuePair<string, string>(secondName, secondValue));
            Assert.Equal(2, row.Count);
            Assert.True(row.TryGetValue(secondName, out retrievedValue));
            Assert.Equal(secondValue, retrievedValue);
            Assert.True(row.ContainsKey(secondName));
            Assert.True(row.Contains(new KeyValuePair<string, string>(secondName, secondValue)));
            Assert.Equal($"{sampleName}, {secondName}", string.Join(", ", row.Keys));
            Assert.Equal($"{sampleValue}, {secondValue}", string.Join(", ", row.Values));

            // Test DictionaryEnumerator
            CollectionReadVerifier.VerifySame<KeyValuePair<string, string>>(expected, row);

            // Negative (missing item) cases
            Assert.False(row.Contains(new KeyValuePair<string, string>(sampleName, secondValue)));
            Assert.False(row.Contains(new KeyValuePair<string, string>(unusedName, sampleValue)));
            Assert.False(row.ContainsKey(unusedName));
            Assert.Throws<KeyNotFoundException>(() => row[unusedName]);

            // Remove
            Assert.True(row.Remove(secondName));
            Assert.False(row.Remove(secondName));
            Assert.False(row.ContainsKey(secondName));
            Assert.Equal($"{sampleName}", string.Join(", ", row.Keys));
            Assert.Equal($"{sampleValue}", string.Join(", ", row.Values));


        }
    }
}
