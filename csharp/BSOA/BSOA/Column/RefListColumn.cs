// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Collections;
using BSOA.Model;

using System;

namespace BSOA.Column
{
    /// <summary>
    ///  RefListColumn provides a reference from an item in one table to a set
    ///  of items in another table. It stores the integer indices of the references.
    /// </summary>
    public class RefListColumn : NullableColumn<NumberList<int>>, IRefColumn
    {
        public string ReferencedTableName { get; }

        public RefListColumn(string referencedTableName, bool nullByDefault = true) : base(new NumberListColumn<int>(), nullByDefault)
        {
            ReferencedTableName = referencedTableName;
        }

        public void ForEach(Action<ArraySlice<int>> action)
        {
            ((NumberListColumn<int>)Values).ForEach(action);
        }
    }
}