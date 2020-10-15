
using BSOA.Extensions;

using System;
using System.Collections.Generic;

using Xunit;

namespace BSOA.Test.Extensions
{
    public class ArrayExtensionsTests
    {
        [Fact]
        public void ArrayExtensions_BinarySearch()
        {
            string[] values = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten" };

            int[] indices = new int[]
            {
                5, 4, 1,                    // First sorted set (0 - 2)
                8, 7, 6, 10, 0,             // Second sorted set (3 - 7)
                2,                          // Third sorted set (8)
                4, 1, 3, 2,                 // Fourth sorted set (9 - 12)
            };

            IComparer<string> comparer = (IComparer<string>)StringComparer.Ordinal;

            // Find 'four' in first set (middle value)
            Assert.Equal(1, ArrayExtensions.IndirectBinarySearch(indices, 0, 3, values, "four", comparer));


            // Find 'eight' in second set (first value)
            Assert.Equal(3, ArrayExtensions.IndirectBinarySearch(indices, 3, 5, values, "eight", comparer));

            // Find 'seven' in second set (second value)
            Assert.Equal(4, ArrayExtensions.IndirectBinarySearch(indices, 3, 5, values, "seven", comparer));

            // Find 'zero' in second set (last value)
            Assert.Equal(7, ArrayExtensions.IndirectBinarySearch(indices, 3, 5, values, "zero", comparer));

            // Find 'ten' in second set (second to last value)
            Assert.Equal(6, ArrayExtensions.IndirectBinarySearch(indices, 3, 5, values, "ten", comparer));

            // Find 'two' in second set (missing; would be before 'zero' and after 'ten')
            Assert.Equal(~7, ArrayExtensions.IndirectBinarySearch(indices, 3, 5, values, "two", comparer));


            // Find 'two' in third set (only value)
            Assert.Equal(8, ArrayExtensions.IndirectBinarySearch(indices, 8, 1, values, "two", comparer));

            // Find 'three' in third set (missing, before only value)
            Assert.Equal(~8, ArrayExtensions.IndirectBinarySearch(indices, 8, 1, values, "three", comparer));

            // Find 'zero' in third set (missing, after only value
            Assert.Equal(~9, ArrayExtensions.IndirectBinarySearch(indices, 8, 1, values, "zero", comparer));


            // Find 'two' in an empty set (not found; insert at start index)
            Assert.Equal(~8, ArrayExtensions.IndirectBinarySearch(indices, 8, 0, values, "two", comparer));


            // Find 'four' in fourth set (first; even count)
            Assert.Equal(9, ArrayExtensions.IndirectBinarySearch(indices, 9, 4, values, "four", comparer));

            // Find 'one' in fourth set (second; even count)
            Assert.Equal(10, ArrayExtensions.IndirectBinarySearch(indices, 9, 4, values, "one", comparer));

            // Find 'three' in fourth set (third; even count)
            Assert.Equal(11, ArrayExtensions.IndirectBinarySearch(indices, 9, 4, values, "three", comparer));

            // Find 'two' in fourth set (fourth; even count)
            Assert.Equal(12, ArrayExtensions.IndirectBinarySearch(indices, 9, 4, values, "two", comparer));


            // Find 'five' in fourth set (before first; even count)
            Assert.Equal(~9, ArrayExtensions.IndirectBinarySearch(indices, 9, 4, values, "five", comparer));

            // Find 'fours' in fourth set (after first; even count)
            Assert.Equal(~10, ArrayExtensions.IndirectBinarySearch(indices, 9, 4, values, "fours", comparer));

            // Find 'ones' in fourth set (after second; even count)
            Assert.Equal(~11, ArrayExtensions.IndirectBinarySearch(indices, 9, 4, values, "ones", comparer));

            // Find 'threes' in fourth set (after third; even count)
            Assert.Equal(~12, ArrayExtensions.IndirectBinarySearch(indices, 9, 4, values, "threes", comparer));

            // Find 'zero' in fourth set (after last; even count)
            Assert.Equal(~13, ArrayExtensions.IndirectBinarySearch(indices, 9, 4, values, "zero", comparer));
        }
    }
}
