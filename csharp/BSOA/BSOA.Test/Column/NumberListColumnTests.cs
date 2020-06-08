using BSOA.Collections;
using BSOA.Column;
using BSOA.Test.Components;
using System;
using System.Collections.Generic;
using System.Linq;
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
                ReadOnlyList.VerifySame(expected[i], column[i]);
            }

            // Round trip and verify they deserialize correctly (note, these will be in a shared array now)
            roundTripped = TreeSerializer.RoundTrip(column, TreeFormat.Binary);
            for (int i = 0; i < expected.Count; ++i)
            {
                ReadOnlyList.VerifySame(expected[i], column[i]);
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
            IList.Basics(slice, (index) => index % 20);

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

        [Fact]
        public void NumberListColumn_TypeList_Basics()
        {
            // Set up column with sample values, roundtrip, re-verify
            NumberListColumn<int> column = BuildSampleColumn();

            // Verify second value is in a shared array, not at index zero, not expandable (yet), not ReadOnly
            NumberList<int> row1List = column[1];
            TypedList<int> row1Typed = new TypedList<int>(row1List, (index) => index, (index) => index);

            // Test second sample row slice IList members on NumberListConverter
            IList.Basics(row1Typed, (index) => index % 20);

            // Verify values are re-merged and re-loaded properly
            string values = string.Join(", ", row1List);
            column = TreeSerializer.RoundTrip(column, TreeFormat.Binary);
            Assert.Equal(values, string.Join(", ", column[1]));

            // TypedList Equality
            TypedList<int> row1 = new TypedList<int>(column[1], (index) => index, (index) => index);
            TypedList<int> row0 = new TypedList<int>(column[0], (index) => index, (index) => index);
            Assert.True(row1.Equals(row1));
            Assert.False(row1.Equals(row0));
            Assert.False(row1 == row0);
            Assert.True(row1 != row0);
            Assert.False(null == row0);
            Assert.True(null != row0);
            Assert.Equal(row1.GetHashCode(), row1.GetHashCode());

            // TypedList.Indices
            Assert.Equal(row1.Indices, column[1]);

            // SetTo(other)
            TypedList<int> firstRow = new TypedList<int>(column[0], (index) => index, (index) => index);
            row1Typed.SetTo(firstRow);
            Assert.Equal(string.Join(", ", firstRow), string.Join(", ", row1Typed));

            // SetTo(null)
            row1Typed.SetTo(null);
            Assert.Empty(row1Typed);

            // SetTo(IList)
            row1Typed.SetTo(new int[] { 2, 3, 4, 5 });
            Assert.Equal("2, 3, 4, 5", string.Join(", ", row1Typed));

            // SetTo(empty)
            row1Typed.SetTo(Array.Empty<int>());
            Assert.Empty(row1Typed);
        }
    }
}
