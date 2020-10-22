// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Benchmarks.Model
{ 
    internal partial class RunDatabase
    {
        public override IColumn BuildColumn(string tableName, string columnName, Type type, object defaultValue = null)
        {
            if (columnName == "RuleId")
            {
                // RuleId column is a DistinctColumn to test DistinctColumn caching
                return new DistinctColumn<string>(new StringColumn());
            }
            else if (columnName == "Tags")
            {
                // Instead of nullable GenericNumberListColumn, configure with NullsDisallowed to avoid wrapping NullableColumn
                return new GenericNumberListColumn<int>(Nullability.NullsDisallowed);
            }
            else
            {
                return base.BuildColumn(tableName, columnName, type, defaultValue);
            }
        }
    }
}
