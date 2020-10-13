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
            first["Two"] = "Two";

            return (ColumnDictionary<string, string>)column[1];
        }

        [Fact]
        public void DictionaryColumn_Basics()
        {
            DictionaryColumn<string, string> scratch = new DictionaryColumn<string, string>(new StringColumn(), new StringColumn(), Nullability.DefaultToEmpty);
            ColumnDictionary<string, string> defaultValue = ColumnDictionary<string, string>.Empty;

            ColumnDictionary<string, string> otherValue = SampleRow();
            otherValue.SetTo(new Dictionary<string, string>()
            {
                ["Name"] = "Scott",
                ["City"] = "Redmond"
            });

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
    }
}
