// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;

using BSOA.Extensions;

using Xunit;

namespace BSOA.Test.Extensions
{
    public class ReadOnlyListExtensionsTests
    {
        [Fact]
        public void ReadOnlyListExtensions_Basics()
        {
            Assert.True(ReadOnlyListExtensions.AreEqual<int>(null, null));
            Assert.False(ReadOnlyListExtensions.AreEqual<int>(new int[] { 1 }, null));

            int[] array = Enumerable.Range(0, 100).ToArray();
            int last = -1;
            ReadOnlyListExtensions.ForEachReverse(array, (value) => last = value);
            Assert.Equal(0, last);
        }
    }
}
