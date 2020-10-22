// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Collections;
using BSOA.Test.Model.V1;

using Xunit;

namespace BSOA.Test.Collections
{
    public class CollectionCompareTests
    {
        [Fact]
        public void CollectionNullComparison()
        {
            Assert.True(((ColumnList<int>)null) == (ColumnList<int>)null);
            Assert.False(((ColumnList<int>)null) == ColumnList<int>.Empty);
            Assert.False(((ColumnList<int>)null) != (ColumnList<int>)null);
            Assert.True(((ColumnList<int>)null) != ColumnList<int>.Empty);

            Assert.True(((ColumnDictionary<string, string>)null) == (ColumnDictionary<string, string>)null);
            Assert.False(((ColumnDictionary<string, string>)null) == ColumnDictionary<string, string>.Empty);
            Assert.False(((ColumnDictionary<string, string>)null) != (ColumnDictionary<string, string>)null);
            Assert.True(((ColumnDictionary<string, string>)null) != ColumnDictionary<string, string>.Empty);

            Assert.True(((NumberList<int>)null) == (NumberList<int>)null);
            Assert.False(((NumberList<int>)null) == NumberList<int>.Empty);
            Assert.False(((NumberList<int>)null) != (NumberList<int>)null);
            Assert.True(((NumberList<int>)null) != NumberList<int>.Empty);

            Assert.True(((TypedList<Person>)null) == (TypedList<Person>)null);
            Assert.False(((TypedList<Person>)null) == TypedList<Person>.Empty);
            Assert.False(((TypedList<Person>)null) != (TypedList<Person>)null);
            Assert.True(((TypedList<Person>)null) != TypedList<Person>.Empty);
        }
    }
}
