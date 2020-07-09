// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;

using BSOA.Collections;
using BSOA.Column;
using BSOA.IO;
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
