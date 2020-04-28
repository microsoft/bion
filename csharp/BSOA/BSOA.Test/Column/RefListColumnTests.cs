using BSOA.Column;
using BSOA.Model;
using BSOA.Test.Components;
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

            Column.Basics(() => new RefListColumn(referencedTable), MutableSlice<int>.Empty, column[0], (index) =>
            {
                column[index].SetTo(new ArraySlice<int>(new int[] { index, index + 1, index + 2 }));
                return column[index];
            });

            // Verify ReferencedTableName stored and correctly kept after deserialize
            Assert.Equal(referencedTable, TreeSerializer.RoundTrip(new RefListColumn(referencedTable), () => new RefListColumn(referencedTable), TreeFormat.Binary).ReferencedTableName);
        }

        [Fact]
        public void RefListColumn_MutableSlice_Basics()
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
            
            // Round trip and verify they deserialize correctly (note, these will be in a shared array now
            roundTripped = TreeSerializer.RoundTrip(column, () => new RefListColumn(referencedTable), TreeFormat.Binary);
            for (int i = 0; i < expected.Count; ++i)
            {
                ReadOnlyList.VerifySame(expected[i], column[i]);
            }

            // Verify second value is in a shared array, not at index zero, not expandable (yet), not ReadOnly
            MutableSlice<int> slice = roundTripped[1];
            Assert.True(slice.Slice.Index > 0);
            Assert.Equal(4, slice.Slice.Count);
            Assert.False(slice.Slice.IsExpandable);
            Assert.False(slice.IsReadOnly);

            // Test MutableSlice.Contains ('==' to avoid XUnit warning to use XUnit contains, which doesn't use IList.Contains)
            Assert.True(slice.Contains(2) == true);
            Assert.True(slice.Contains(5) == true);
            Assert.True(slice.Contains(6) == false);

            // Test IndexOf; verify indices are mapped back to [0, count-1]
            Assert.Equal(0, slice.IndexOf(2));
            Assert.Equal(3, slice.IndexOf(5));
            Assert.Equal(-1, slice.IndexOf(6));

            // Test CopyTo
            int[] other = new int[slice.Count + 1];
            slice.CopyTo(other, 1);
            Assert.Equal("0, " + string.Join(", ", slice), string.Join(", ", other));

            // Change an item inline; verify changed, array not moved, not expandable
            slice[0] = 1;
            Assert.Equal("1, 3, 4, 5", string.Join(", ", slice));
            Assert.Equal(1, slice[0]);
            Assert.True(slice.Slice.Index > 0);
            Assert.False(slice.Slice.IsExpandable);

            // Append an item; verify appended, count changed, inner array is now a separate, writeable array
            slice.Add(10);
            Assert.Equal("1, 3, 4, 5, 10", string.Join(", ", slice));
            Assert.Equal(0, slice.Slice.Index);
            Assert.True(slice.Slice.IsExpandable);

            // Test Remove, RemoveAt
            Assert.False(slice.Remove(9));
            Assert.True(slice.Remove(3));
            Assert.Equal("1, 4, 5, 10", string.Join(", ", slice));

            // Test RemoveAt bounds checks
            Assert.Throws<IndexOutOfRangeException>(() => slice.RemoveAt(-1));
            Assert.Throws<IndexOutOfRangeException>(() => slice.RemoveAt(4));

            // Test Insert
            slice.Insert(0, -1);
            Assert.Equal("-1, 1, 4, 5, 10", string.Join(", ", slice));
            slice.Insert(2, 99);
            Assert.Equal("-1, 1, 99, 4, 5, 10", string.Join(", ", slice));

            // Test Insert bounds checks
            Assert.Throws<IndexOutOfRangeException>(() => slice.Insert(-1, 10));
            Assert.Throws<IndexOutOfRangeException>(() => slice.Insert(6, 10));

            // Clear; verify empty, read-only static instance
            slice.Clear();
            Assert.Empty(slice);
            Assert.False(slice.Slice.IsExpandable);

            // Test Add until resize required; verify old elements copied to larger array properly
            for (int i = 0; i < 50; ++i)
            {
                slice.Add(i);
            }

            for (int i = 0; i < 50; ++i)
            {
                Assert.Equal(i, slice[i]);
            }

            // Verify 'slice' instance is being persisted back to column
            Assert.Equal(string.Join(", ", slice), string.Join(", ", roundTripped[1]));

            // Verify values are re-merged and re-loaded properly
            string values = string.Join(", ", slice);
            roundTripped = TreeSerializer.RoundTrip(roundTripped, () => new RefListColumn(referencedTable), TreeFormat.Binary);
            Assert.Equal(values, string.Join(", ", roundTripped[1]));

            // Column range check
            Assert.Throws<IndexOutOfRangeException>(() => roundTripped[-1]);
        }
    }
}
