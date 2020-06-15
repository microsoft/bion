// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BSOA.Column
{
    /// <summary>
    ///  RefListColumn provides a reference from an item in one table to a set
    ///  of items in another table. It stores the integer indices of the references.
    /// </summary>
    public class RefListColumn : NumberListColumn<int>
    {
        public string ReferencedTableName { get; }

        public RefListColumn(string referencedTableName)
        {
            ReferencedTableName = referencedTableName;
        }
    }
}