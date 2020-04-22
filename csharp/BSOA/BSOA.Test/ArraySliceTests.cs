using BSOA.Test.Components;
using System;
using System.Linq;
using Xunit;

namespace BSOA.Test
{
    public class ArraySliceTests
    {
        [Fact]
        public void ArraySliceTests_Basics()
        {
            int[] sample = Enumerable.Range(100, 50).ToArray();
            int[] copyToTarget = new int[100];
            ArraySlice<int> slice;

            // Empty ArraySlice
            slice = ArraySlice<int>.Empty;
            Assert.Empty(slice);
            VerifyCopyTo<int>(slice, copyToTarget);
            VerifyRoundTrip<int>(slice, copyToTarget);
            ReadOnlyList.VerifySame(slice, TreeSerializer.RoundTrip(slice, TreeFormat.Binary));
            ReadOnlyList.VerifySame(slice, TreeSerializer.RoundTrip(slice, TreeFormat.Json));

            // Whole Array
            slice = new ArraySlice<int>(sample);
            Assert.Equal(sample.Length, slice.Count);
            Assert.Equal(sample, slice);
            Assert.Equal(sample[10], slice[10]);
            VerifyCopyTo<int>(slice, copyToTarget);
            VerifyRoundTrip<int>(slice, copyToTarget);
            ReadOnlyList.VerifySame(slice, TreeSerializer.RoundTrip(slice, TreeFormat.Binary));
            ReadOnlyList.VerifySame(slice, TreeSerializer.RoundTrip(slice, TreeFormat.Json));

            // Array slice-to-end
            slice = new ArraySlice<int>(sample, index: 10);
            Assert.Equal(sample.Length - 10, slice.Count);
            Assert.Equal(sample[20], slice[10]);
            VerifyCopyTo<int>(slice, copyToTarget);
            VerifyRoundTrip<int>(slice, copyToTarget);
            ReadOnlyList.VerifySame(slice, TreeSerializer.RoundTrip(slice, TreeFormat.Binary));
            ReadOnlyList.VerifySame(slice, TreeSerializer.RoundTrip(slice, TreeFormat.Json));

            // Array slice
            slice = new ArraySlice<int>(sample, index: 10, length: 20);
            Assert.Equal(20, slice.Count);
            Assert.Equal(sample[10], slice[0]);
            VerifyCopyTo<int>(slice, copyToTarget);
            VerifyRoundTrip<int>(slice, copyToTarget);
            ReadOnlyList.VerifySame(slice, TreeSerializer.RoundTrip(slice, TreeFormat.Binary));
            ReadOnlyList.VerifySame(slice, TreeSerializer.RoundTrip(slice, TreeFormat.Json));

            // Bounds checks
            Assert.Throws<ArgumentNullException>(() => new ArraySlice<int>(null, 0, 0));                            // Array null
            Assert.Throws<ArgumentOutOfRangeException>(() => new ArraySlice<int>(sample, -1, 0));                   // index < 0
            Assert.Throws<ArgumentOutOfRangeException>(() => new ArraySlice<int>(sample, sample.Length + 1, 0));    // index > array.Length
            Assert.Throws<ArgumentOutOfRangeException>(() => new ArraySlice<int>(sample, 0, sample.Length + 1));    // length too long
            Assert.Throws<ArgumentOutOfRangeException>(() => new ArraySlice<int>(sample, 2, sample.Length + 3));
        }

        internal static void VerifyRoundTrip<T>(ArraySlice<T> slice, T[] copyToTargetArray) where T : unmanaged
        {
            ArraySlice<T> roundTripped = TreeSerializer.RoundTrip(slice, TreeFormat.Binary);
            ReadOnlyList.VerifySame<T>(slice, roundTripped);
            VerifyCopyTo<T>(roundTripped, copyToTargetArray);
        }

        internal static void VerifyCopyTo<T>(ArraySlice<T> slice, T[] copyToTargetArray) where T : unmanaged
        {
            slice.CopyTo(copyToTargetArray, 1);
            for (int i = 0; i < slice.Count; ++i)
            {
                Assert.Equal(slice[i], copyToTargetArray[i + 1]);
            }
        }
    }
}
