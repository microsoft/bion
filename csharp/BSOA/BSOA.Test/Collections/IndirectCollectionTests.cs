// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using BSOA.Collections;

using Xunit;

namespace BSOA.Test.Collections
{
    public class IndirectCollectionTests
    {
        [Fact]
        public void IndirectCollection_Basics()
        {
            // Note: ColumnDictionary now sorts keys internally, so collections will come in sorted order, not insertion order
            IDictionary<string, string> row = DictionaryColumnTests.SampleRow();
            row["City"] = "Redmond";
            row["Name"] = "Scott";

            List<string> keys = new List<string>() { "City", "Name" };
            List<string> values = new List<string>() { "Redmond", "Scott" };

            IndirectCollection<string> collection = (IndirectCollection<string>)row.Keys;

            // Count
            Assert.Equal(2, collection.Count);

            // Enumeration
            CollectionReadVerifier.VerifySame(keys, (IndirectCollection<string>)row.Keys);
            CollectionReadVerifier.VerifySame(values, (IndirectCollection<string>)row.Values);

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
            Assert.Equal("City", names[1]);
            Assert.Equal("Name", names[2]);
            Assert.Throws<ArgumentNullException>(() => collection.CopyTo(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => collection.CopyTo(names, -1));
            Assert.Throws<ArgumentException>(() => collection.CopyTo(names, 2));
        }
    }
}
