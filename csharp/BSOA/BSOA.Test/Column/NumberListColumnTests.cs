// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using BSOA.Collections;
using BSOA.Column;
using BSOA.IO;
using BSOA.Model;
using BSOA.Test.Components;

using Xunit;

namespace BSOA.Test
{
    public class NumberListColumnTests
    {
        [Fact]
        public void NumberListColumn_Basics()
        {
            NumberListColumn<int> column = new NumberListColumn<int>();
            column[0].SetTo(new ArraySlice<int>(new int[] { 0, 1, 2 }));

            TreeDiagnostics diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            int tinyColumnLength = (int)diagnostics.Length;

            Column.Basics(() => new NumberListColumn<int>(), NumberList<int>.Empty, column[0], (index) =>
            {
                column[index].SetTo(new ArraySlice<int>(new int[] { index, index + 1, index + 2 }));
                return column[index];
            });

            // NumberList Equals, GetHashCode
            int[] asArray = new int[] { 0, 1, 3 };
            column[1].SetTo(new ArraySlice<int>(asArray));
            Assert.False(column[0] == column[1]);
            Assert.True(column[0] != column[1]);
            Assert.NotEqual(column[0].GetHashCode(), column[1].GetHashCode());

            // Compare to array
            Assert.True(column[1].Equals(asArray));

            // ForEach
            column.Clear();
            column[0].SetTo(new ArraySlice<int>(new int[] { 0, 1, 2 }));
            int sum = 0;
            column.ForEach((slice) =>
            {
                int[] array = slice.Array;
                int end = slice.Index + slice.Count;
                for (int i = slice.Index; i < end; ++i)
                {
                    sum += array[i];
                }
            });
            Assert.Equal(3, sum);
        }

        [Fact]
        public void GenericNumberListColumn_Basics()
        {
            List<int> empty = new List<int>();

            GenericNumberListColumn<int> column = new GenericNumberListColumn<int>();
            column[0] = new int[] { 0, 1, 2 };

            TreeDiagnostics diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            int tinyColumnLength = (int)diagnostics.Length;

            Column.Basics(() => new GenericNumberListColumn<int>(), null, column[0], (index) =>
            {
                IList<int> values = column[index];
                if (values == null || values.Count == 0)
                {
                    column[index] = new int[] { index, index + 1, index + 2 };
                    values = column[index];
                }

                return values;
            });

            // ForEach
            column.Clear();
            column[0] = new int[] { 0, 1, 2 };
            int sum = 0;
            column.ForEach((slice) =>
            {
                int[] array = slice.Array;
                int end = slice.Index + slice.Count;
                for (int i = slice.Index; i < end; ++i)
                {
                    sum += array[i];
                }
            });
            Assert.Equal(3, sum);
        }

        [Fact]
        public void NumberListColumn_NullableCases()
        {
            // StringColumns are extremely common in object models,
            // so having very compact representations for common cases
            // is really important to file size for small databases.

            GenericNumberListColumn<int> column = new GenericNumberListColumn<int>(Nullability.DefaultToNull);
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
            List<int> empty = new List<int>();
            for (int i = 0; i < 100; ++i)
            {
                column[i] = empty;
            }

            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, TreeFormat.Binary, testDoubleDispose: false));
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(1 == diagnostics.Children.Count);
            Assert.True(diagnostics.Length <= 13);

            // No nulls, No Empty: 4b + 2.125b / value (613b) + 4 pages x 4b (16b) + overhead (~10b)
            List<int> single = new List<int>();
            single.Add(1);

            for (int i = 0; i < 100; ++i)
            {
                column[i] = single;
            }

            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, TreeFormat.Binary, testDoubleDispose: false));
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(1 == diagnostics.Children.Count);
            Assert.True(diagnostics.Length <= 640);

            // Nulls and Non-Nulls; both parts must be written
            column[50] = null;

            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, TreeFormat.Binary, testDoubleDispose: false));
            diagnostics = TreeSerializer.Diagnostics(column, TreeFormat.Binary);
            Assert.True(2 == diagnostics.Children.Count);
            Assert.True(diagnostics.Length <= 670);
        }

        private NumberListColumn<int> BuildSampleColumn()
        {
            List<ArraySlice<int>> expected = new List<ArraySlice<int>>();
            expected.Add(new ArraySlice<int>(new int[] { 5, 6, 7 }));
            expected.Add(new ArraySlice<int>(new int[] { 2, 3, 4, 5 }));
            expected.Add(new ArraySlice<int>(Enumerable.Range(0, 8192).ToArray()));

            NumberListColumn<int> column = new NumberListColumn<int>();
            NumberListColumn<int> roundTripped;

            // Set each list, and verify they are correct when read back
            for (int i = 0; i < expected.Count; ++i)
            {
                column[i].SetTo(expected[i]);
            }

            for (int i = 0; i < expected.Count; ++i)
            {
                CollectionReadVerifier.VerifySame(expected[i], column[i]);
            }

            // Round trip and verify they deserialize correctly (note, these will be in a shared array now)
            roundTripped = TreeSerializer.RoundTrip(column, TreeFormat.Binary);
            for (int i = 0; i < expected.Count; ++i)
            {
                CollectionReadVerifier.VerifySame(expected[i], column[i]);
            }

            return roundTripped;
        }

        [Fact]
        public void NumberListColumn_NumberList_Basics()
        {
            // Set up column with sample values, roundtrip, re-verify
            NumberListColumn<int> column = BuildSampleColumn();

            // Verify second value is in a shared array, not at index zero, not expandable (yet), not ReadOnly
            NumberList<int> slice = column[1];
            Assert.Equal(4, slice.Count);
            Assert.True(slice.Slice.Index > 0);
            Assert.False(slice.Slice.IsExpandable);

            // Test second sample row slice IList members
            CollectionChangeVerifier.VerifyList(slice, (index) => index % 20);

            // Verify expandable after test
            Assert.Equal(0, slice.Slice.Index);
            Assert.True(slice.Slice.IsExpandable);

            // Verify values are re-merged and re-loaded properly
            string values = string.Join(", ", slice);
            column = TreeSerializer.RoundTrip(column, TreeFormat.Binary);
            Assert.Equal(values, string.Join(", ", column[1]));

            // Column range check
            Assert.Throws<IndexOutOfRangeException>(() => column[-1]);
        }
    }
}
