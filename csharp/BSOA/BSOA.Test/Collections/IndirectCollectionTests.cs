using BSOA.Collections;

using System;
using System.Collections.Generic;

using Xunit;

namespace BSOA.Test.Collections
{
    public class IndirectCollectionTests
    {
        [Fact]
        public void IndirectCollection_Basics()
        {
            IDictionary<string, string> row = DictionaryColumnTests.SampleRow();
            row["Name"] = "Scott";
            row["City"] = "Redmond";

            List<string> keys = new List<string>() { "Name", "City" };
            List<string> values = new List<string>() { "Scott", "Redmond" };

            IndirectCollection<string> collection = (IndirectCollection<string>)row.Keys;

            // Count
            Assert.Equal(2, collection.Count);

            // Enumeration
            ReadOnlyList.VerifySame(keys, (IndirectCollection<string>)row.Keys);
            ReadOnlyList.VerifySame(values, (IndirectCollection<string>)row.Values);

            // Read-Only-ness
            Assert.True(collection.IsReadOnly);
            Assert.Throws<NotSupportedException>(() => collection.Add("New"));
            Assert.Throws<NotSupportedException>(() => collection.Remove("Name"));
            Assert.Throws<NotSupportedException>(() => collection.Clear());

            // Contains
            Assert.True(true == collection.Contains("Name"));
            Assert.True(false == collection.Contains("New"));

            // CopyTo
            string[] names = new string[3];
            collection.CopyTo(names, 1);
            Assert.Equal("Name", names[1]);
            Assert.Equal("City", names[2]);
            Assert.Throws<ArgumentNullException>(() => collection.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(names, -1));
            Assert.Throws<ArgumentException>(() => collection.CopyTo(names, 2));
        }
    }
}
