// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
        }
    }
}
