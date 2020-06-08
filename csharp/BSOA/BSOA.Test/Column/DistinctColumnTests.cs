using BSOA.Column;
using BSOA.Test.Components;
using System;
using System.Collections.Generic;
using Xunit;

namespace BSOA.Test
{
    public class DistinctColumnTests
    {
        [Fact]
        public void DistinctColumn_Basics()
        {
            int defaultValue = -1;

            Column.Basics<int>(
                () => new DistinctColumn<int>(new NumberColumn<int>(defaultValue), defaultValue),
                defaultValue,
                10,
                (i) => i
            );
        }

        [Fact]
        public void DistinctColumn_Conversion()
        {
            int defaultValue = -1;
            Func<DistinctColumn<int>> ctor = () => new DistinctColumn<int>(new NumberColumn<int>(defaultValue), defaultValue);

            DistinctColumn<int> column = ctor();
            List<int> expected = new List<int>();

            // Verify empty and set up to map values by default
            Assert.Empty(column);
            Assert.True(column.IsMappingValues);

            // Round trip and verify it resets back to mapping correctly
            column = TreeSerializer.RoundTrip(column, ctor, TreeFormat.Binary);
            Assert.Empty(column);
            Assert.True(column.IsMappingValues);

            // Add 1,000 values with 10 distinct values
            for (int i = 0; i < 1000; ++i)
            {
                int value = i % 10;
                column[i] = value;
                expected.Add(value);
            }

            // Verify column is mapping, has 11 unique values (default + 10), and matches expected array
            Assert.True(column.IsMappingValues);
            Assert.Equal(11, column.DistinctCount);
            CollectionReadVerifier.VerifySame(expected, column);

            // Round trip; verify mapped column rehydrates properly
            column = TreeSerializer.RoundTrip(column, ctor, TreeFormat.Binary);
            CollectionReadVerifier.VerifySame(expected, column);

            // Add enough values to force the column to convert
            for (int i = 1000; i < 1300; ++i)
            {
                column[i] = i;
                expected.Add(i);
            }

            Assert.False(column.IsMappingValues);
            Assert.Equal(-1, column.DistinctCount);
            CollectionReadVerifier.VerifySame(expected, column);

            // Round-trip; verify individual values column rehydrates properly
            column = TreeSerializer.RoundTrip(column, ctor, TreeFormat.Binary);
            CollectionReadVerifier.VerifySame(expected, column);

            // Test RemoveFromEnd on unmapped form of column
            column.RemoveFromEnd(100);
            expected.RemoveRange(expected.Count - 100, 100);
            CollectionReadVerifier.VerifySame(expected, column);
        }

        [Fact]
        public void DistinctColumn_Remap()
        {
            int defaultValue = -1;
            Func<DistinctColumn<int>> ctor = () => new DistinctColumn<int>(new NumberColumn<int>(defaultValue), defaultValue);

            DistinctColumn<int> column = ctor();
            List<int> expected = new List<int>();

            // Add 1,000 values with 10 distinct values
            for (int i = 0; i < 1000; ++i)
            {
                int value = 1 + i % 10;
                column[i] = value;
                expected.Add(value);
            }

            // Verify column is mapping, has 11 unique values (default + 10), and matches expected array
            Assert.True(column.IsMappingValues);
            Assert.Equal(11, column.DistinctCount);
            CollectionReadVerifier.VerifySame(expected, column);

            // Verify Trim does not remap
            column.Trim();
            Assert.True(column.IsMappingValues);
            Assert.Equal(11, column.DistinctCount);
            CollectionReadVerifier.VerifySame(expected, column);

            // Set rows to just three (middle) values
            for (int i = 0; i < 1000; ++i)
            {
                int value = 3 + i % 3;
                column[i] = value;
                expected[i] = value;
            }

            // Trim. Verify column finds unused values and removes them, and column values read back correctly
            column.Trim();
            Assert.True(column.IsMappingValues);
            Assert.Equal(4, column.DistinctCount);
            CollectionReadVerifier.VerifySame(expected, column);
        }
    }
}
