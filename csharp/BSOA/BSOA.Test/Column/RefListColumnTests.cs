// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Column;
using BSOA.Test.Components;
using BSOA.Test.Model.V1;

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

            RefListColumn roundTripped = TreeSerializer.RoundTrip(column, () => new RefListColumn(referencedTable), TreeFormat.Binary);
            Assert.Equal(referencedTable, roundTripped.ReferencedTableName);
        }
    }
}
