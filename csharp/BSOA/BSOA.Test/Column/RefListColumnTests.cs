using BSOA.Column;
using BSOA.Test.Components;
using BSOA.Test.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BSOA.Test
{
    public class RefListColumnTests
    {
        [Fact]
        public void RefListColumn_Basics()
        {
            string referencedTable = "ReferencedTable";
            RefListColumn column = new RefListColumn(referencedTable);
            column[0].SetTo(new ArraySlice<int>(new int[] { 0, 1, 2 }));

            Column.Basics(() => new RefListColumn(referencedTable), NumberList<int>.Empty, column[0], (index) =>
            {
                column[index].SetTo(new ArraySlice<int>(new int[] { index, index + 1, index + 2 }));
                return column[index];
            });

            // Verify ReferencedTableName stored and correctly kept after deserialize
            Assert.Equal(referencedTable, TreeSerializer.RoundTrip(new RefListColumn(referencedTable), () => new RefListColumn(referencedTable), TreeFormat.Binary).ReferencedTableName);
        }

        private RefListColumn BuildSampleColumn()
        {
            string referencedTable = "ReferencedTable";

            List<ArraySlice<int>> expected = new List<ArraySlice<int>>();
            expected.Add(new ArraySlice<int>(new int[] { 5, 6, 7 }));
            expected.Add(new ArraySlice<int>(new int[] { 2, 3, 4, 5 }));
            expected.Add(new ArraySlice<int>(Enumerable.Range(0, 8192).ToArray()));

            RefListColumn column = new RefListColumn(referencedTable);
            RefListColumn roundTripped;

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
            roundTripped = TreeSerializer.RoundTrip(column, () => new RefListColumn(referencedTable), TreeFormat.Binary);
            for (int i = 0; i < expected.Count; ++i)
            {
                ReadOnlyList.VerifySame(expected[i], column[i]);
            }

            return roundTripped;
        }

        [Fact]
        public void RefListColumn_NumberList_Basics()
        {
            // Set up column with sample values, roundtrip, re-verify
            RefListColumn column = BuildSampleColumn();

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
            column = TreeSerializer.RoundTrip(column, () => new RefListColumn("TableName"), TreeFormat.Binary);
            Assert.Equal(values, string.Join(", ", column[1]));

            // Column range check
            Assert.Throws<IndexOutOfRangeException>(() => column[-1]);
        }

        [Fact]
        public void RefListColumn_NumberListConverter_Basics()
        {
            // Set up column with sample values, roundtrip, re-verify
            RefListColumn column = BuildSampleColumn();

            // Verify second value is in a shared array, not at index zero, not expandable (yet), not ReadOnly
            NumberList<int> innerSlice = column[1];
            NumberListConverter<int, PersonTable> slice = new NumberListConverter<int, PersonTable>(innerSlice, null, (table, index) => index, (table, index) => index);

            // Test second sample row slice IList members on MutableSlice*Wrapper*
            IList.Basics(slice, (index) => index % 20);

            // Verify values are re-merged and re-loaded properly
            string values = string.Join(", ", innerSlice);
            column = TreeSerializer.RoundTrip(column, () => new RefListColumn("TableName"), TreeFormat.Binary);
            Assert.Equal(values, string.Join(", ", column[1]));

            // SetTo(MutableSliceWrapper)
            NumberListConverter<int, PersonTable> firstRow = new NumberListConverter<int, PersonTable>(column[0], null, (table, index) => index, (table, index) => index);
            slice.SetTo(firstRow);
            Assert.Equal(string.Join(", ", firstRow), string.Join(", ", slice));

            // SetTo(null)
            slice.SetTo(null);
            Assert.Empty(slice);

            // SetTo(IList)
            slice.SetTo(new int[] { 2, 3, 4, 5 });
            Assert.Equal("2, 3, 4, 5", string.Join(", ", slice));

            // SetTo(empty)
            slice.SetTo(Array.Empty<int>());
            Assert.Empty(slice);
        }
    }
}
