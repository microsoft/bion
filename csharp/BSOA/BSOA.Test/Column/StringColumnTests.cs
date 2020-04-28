using BSOA.Column;
using BSOA.IO;
using BSOA.Test.Components;
using System;
using System.Collections.Generic;

using Xunit;

namespace BSOA.Test
{
    public class StringColumnTests
    {
        [Fact]
        public void StringColumn_Basics()
        {
            Column.Basics<string>(
                () => new StringColumn(),
                null,
                "AnotherValue",
                (i) => i.ToString()
            );
        }

        [Fact]
        public void StringColumn_BackToEmpty()
        {
            StringColumn column = new StringColumn();
            int count = 512;
            TreeDiagnostics diagnostics;

            column[0] = "A";
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            int fixedSizeCost = (int)diagnostics.Length;

            // Set many non-empty values
            string value = "0123456789";
            for (int i = 0; i < count; ++i)
            {
                column[i] = value;
            }

            // Expect size: value bytes, 2 byte lengths, 4 byte page starts, 1 bit IsNull
            int sizeEstimate = fixedSizeCost + count * value.Length + count * 2 + ((count * 4) / 32) + (count / 8);
            column.Trim();
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(diagnostics.Length < sizeEstimate);

            // Reload column
            column = TreeSerializer.RoundTrip(column, TreeFormat.Binary);

            // Set almost all values empty
            for (int i = 10; i < count; ++i)
            {
                column[i] = string.Empty;
            }

            // Expect size: 10 values, 10 lengths, normal page starts and IsNull
            sizeEstimate = fixedSizeCost + 10 * value.Length + 10 * 2 + ((count * 4) / 32) + (count / 8);
            column.Trim();
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(diagnostics.Length < sizeEstimate);

            // Clear column and round trip (verify Chapters created and then emptied serialize and restore properly)
            column.Clear();
            Assert.Empty(TreeSerializer.RoundTrip(column, TreeFormat.Binary));

            // Bounds check for VariableLengthColumn
            Assert.Throws<IndexOutOfRangeException>(() => column[-1]);
        }

        [Fact]
        public void StringColumn_LongValuesAndMerging()
        {
            StringColumn column = new StringColumn();
            List<string> expected = new List<string>();
            StringColumn roundTripped;

            // Test values just at and above LargeValue limit
            expected.Add(new string(' ', 2047));
            expected.Add(null);
            expected.Add(string.Empty);
            expected.Add("Normal");
            expected.Add(new string(' ', 2048));

            for (int i = 0; i < expected.Count; ++i)
            {
                column[i] = expected[i];
            }

            // Verify values properly captured
            ReadOnlyList.VerifySame(expected, column);

            // Proactively Trim (before serialization) and verify values not corrupted
            column.Trim();
            ReadOnlyList.VerifySame(expected, column);

            // Verify roundtripped column and column not corrupted by serialization
            roundTripped = TreeSerializer.RoundTrip(column, TreeFormat.Binary);
            ReadOnlyList.VerifySame(expected, roundTripped);
            ReadOnlyList.VerifySame(expected, column);

            // Set a short value to long and a long value to short, and add another value
            expected[0] = new string(':', 2400);
            expected[2] = "MuchShorter";
            expected.Add("Simple");

            for (int i = 0; i < expected.Count; ++i)
            {
                column[i] = expected[i];
            }

            // Verify values read back correctly immediately
            ReadOnlyList.VerifySame(expected, column);

            // Verify values re-roundtrip again properly (merging old and new immutable values)
            roundTripped = TreeSerializer.RoundTrip(column, TreeFormat.Binary);
            ReadOnlyList.VerifySame(expected, roundTripped);
            ReadOnlyList.VerifySame(expected, column);

            // Add a value causing a gap; verify count, new value returned, values in gap defaulted properly
            column[100] = "Centennial";

            Assert.Equal(101, column.Count);
            Assert.Equal("Centennial", column[100]);
            Assert.Null(column[99]);
        }
    }
}
