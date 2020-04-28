using BSOA.Column;
using BSOA.Model;
using System.Collections.Generic;
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

            List<int> expected = new List<int>();
            MutableSlice<int> slice = column[0];

            // Verify lists begin as empty
            ReadOnlyList.VerifySame(expected, slice);

            // Add a small number of simple items
            for(int i = 5; i < 10; ++i)
            {
                slice.Add(i);
                expected.Add(i);
            }

            // Verify lists remain equal
            ReadOnlyList.VerifySame(expected, slice);


            Column.Basics(() => new RefListColumn(referencedTable), MutableSlice<int>.Empty, slice, (index) => slice);
        }
    }
}
