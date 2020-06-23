// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using BSOA.Column;
using BSOA.IO;
using BSOA.Test.Components;

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

            // Add enough to force conversion of values
            List<string> expected = new List<string>();
            StringColumn column = new StringColumn();
            for (int i = 0; i < 300; ++i)
            {
                column[i] = "A";
                expected.Add("A");
            }

            CollectionReadVerifier.VerifySame(expected, column, true);
        }

        [Fact]
        public void StringColumn_EmptyCases()
        {
            // StringColumns are extremely common in object models,
            // so having very compact representations for common cases
            // is really important to file size for small databases.

            StringColumn column = new StringColumn();
            TreeDiagnostics diagnostics;

            // Empty: { }
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(diagnostics.Length <= 2);

            // All null: { IsNull: { Count: 100, Capacity: 100 } }
            for (int i = 0; i < 100; ++i)
            {
                column[i] = null;
            }

            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, TreeFormat.Binary, testDoubleDispose: false));
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(1 == diagnostics.Children.Count);
            Assert.True(diagnostics.Length <= 13);

            // All empty: Only nulls false written
            for (int i = 0; i < 100; ++i)
            {
                column[i] = "";
            }

            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, TreeFormat.Binary, testDoubleDispose: false));
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(1 == diagnostics.Children.Count);
            Assert.True(diagnostics.Length <= 13);

            // No nulls, No Empty: 3b / value (2b end + 1b text) + 4 pages x 4b + 20b overhead
            for (int i = 0; i < 100; ++i)
            {
                column[i] = "-";
            }

            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, TreeFormat.Binary, testDoubleDispose: false));
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(1 == diagnostics.Children.Count);
            Assert.True(diagnostics.Length <= 336);

            // Nulls and Non-Nulls; both parts must be written
            column[50] = null;

            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, TreeFormat.Binary, testDoubleDispose: false));
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(2 == diagnostics.Children.Count);
            Assert.True(diagnostics.Length <= 336 + 40);
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
            CollectionReadVerifier.VerifySame(expected, column);

            // Proactively Trim (before serialization) and verify values not corrupted
            column.Trim();
            CollectionReadVerifier.VerifySame(expected, column);

            // Verify roundtripped column and column not corrupted by serialization
            roundTripped = TreeSerializer.RoundTrip(column, TreeFormat.Binary);
            CollectionReadVerifier.VerifySame(expected, roundTripped);
            CollectionReadVerifier.VerifySame(expected, column);

            // Set a short value to long and a long value to short, and add another value
            expected[0] = new string(':', 2400);
            expected[2] = "MuchShorter";
            expected.Add("Simple");

            for (int i = 0; i < expected.Count; ++i)
            {
                column[i] = expected[i];
            }

            // Verify values read back correctly immediately
            CollectionReadVerifier.VerifySame(expected, column);

            // Verify values re-roundtrip again properly (merging old and new immutable values)
            roundTripped = TreeSerializer.RoundTrip(column, TreeFormat.Binary);
            CollectionReadVerifier.VerifySame(expected, roundTripped);
            CollectionReadVerifier.VerifySame(expected, column);

            // Add a value causing a gap; verify count, new value returned, values in gap defaulted properly
            column[100] = "Centennial";

            Assert.Equal(101, column.Count);
            Assert.Equal("Centennial", column[100]);
            Assert.Null(column[99]);
        }
    }
}
