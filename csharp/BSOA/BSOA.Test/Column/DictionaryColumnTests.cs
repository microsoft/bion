// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using BSOA.Collections;
using BSOA.Column;
using BSOA.Model;

using Xunit;

namespace BSOA.Test
{
    public class DictionaryColumnTests
    {
        public static ColumnDictionary<string, string> SampleRow()
        {
            DictionaryColumn<string, string> column = new DictionaryColumn<string, string>(
                new DistinctColumn<string>(new StringColumn(), null),
                new StringColumn(),
                Nullability.NullsDisallowed);

            ColumnDictionary<string, string> first = (ColumnDictionary<string, string>)column[0];
            first["One"] = "One";
            first.Add("Two", "Two");

            return (ColumnDictionary<string, string>)column[1];
        }

        [Fact]
        public void DictionaryColumn_Basics()
        {
            DictionaryColumn<string, string> scratch = new DictionaryColumn<string, string>(new StringColumn(), new StringColumn(), Nullability.DefaultToEmpty);
            ColumnDictionary<string, string> defaultValue = ColumnDictionary<string, string>.Empty;

            ColumnDictionary<string, string> otherValue = SampleRow();
            Dictionary<string, string> model = new Dictionary<string, string>()
            {
                ["Name"] = "Scott",
                ["City"] = "Redmond"
            };

            otherValue.SetTo(model);
            
            // Test ColumnDictionary.Equals against non-ColumnDictionary IDictionary (slower compare path)
            Assert.True(otherValue.Equals(model));
            model["City"] = "Bellevue";
            Assert.False(otherValue.Equals(model));

            Column.Basics<IDictionary<string, string>>(
                () => new DictionaryColumn<string, string>(
                    new DistinctColumn<string>(new StringColumn()),
                    new StringColumn(),
                    Nullability.DefaultToEmpty),
                defaultValue,
                otherValue,
                (i) =>
                {
                    if (scratch[i].Count == 0)
                    {
                        scratch[i][(i % 10).ToString()] = i.ToString();
                        scratch[i][((i + 1) % 10).ToString()] = i.ToString();
                    }

                    return scratch[i];
                }
            );

            defaultValue = null;
            Column.Basics<IDictionary<string, string>>(
                () => new DictionaryColumn<string, string>(
                    new DistinctColumn<string>(new StringColumn()),
                    new StringColumn(),
                    Nullability.DefaultToNull),
                defaultValue,
                otherValue,
                (i) =>
                {
                    if (scratch[i].Count == 0)
                    {
                        scratch[i][(i % 10).ToString()] = i.ToString();
                        scratch[i][((i + 1) % 10).ToString()] = i.ToString();
                    }

                    return scratch[i];
                }
            );
        }

        [Fact]
        public void DictionaryColumn_NonString()
        {
            DictionaryColumn<int, int> column = new DictionaryColumn<int, int>(new NumberColumn<int>(-1), new NumberColumn<int>(-1), Nullability.NullsDisallowed);
            IDictionary<int, int> dictionary = column[0];

            dictionary[5] = 5;
            dictionary[8] = 8;
            dictionary[10] = 10;

            Assert.Equal(10, dictionary[10]);
            Assert.Equal(5, dictionary[5]);
            Assert.False(dictionary.ContainsKey(6));

            // Trigger column collection with all values still used; verify everything kept
            column.Trim();
            Assert.Equal(10, dictionary[10]);
            Assert.Equal(5, dictionary[5]);
            Assert.False(dictionary.ContainsKey(6));

            // Replace a value
            dictionary[10] = 11;
            Assert.Equal(11, dictionary[10]);

            // Remove a key
            dictionary.Remove(5);
            Assert.True(dictionary.ContainsKey(10));
            Assert.False(dictionary.ContainsKey(5));

            // Trigger unused key/value pair collection
            column.Trim();

            // Verify remaining key still present and with correct value
            Assert.Equal(11, dictionary[10]);
            Assert.Equal(8, dictionary[8]);
        }
    }
}
