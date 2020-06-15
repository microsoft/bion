// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Column;
using BSOA.Test.Components;

using Xunit;

namespace BSOA.Test
{
    public class RefColumnTests
    {
        [Fact]
        public void RefColumn_Basics()
        {
            string referencedTable = "ReferencedTable";
            Column.Basics<int>(() => new RefColumn(referencedTable), -1, 10, (index) => 2 * index);

            // Verify ReferencedTableName stored and correctly kept after deserialize
            Assert.Equal(referencedTable, TreeSerializer.RoundTrip(new RefColumn(referencedTable), () => new RefColumn(referencedTable), TreeFormat.Binary).ReferencedTableName);
        }
    }
}
