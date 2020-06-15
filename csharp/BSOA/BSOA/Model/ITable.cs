// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BSOA.Model
{
    public interface ITable<T> : IColumn<T>, ITable where T : IRow
    { }

    public interface ITable : IColumn
    {
        // Do I need this? If only copying from identical table type, no.
        // Dictionary<string, IColumn> Columns { get; }
    }
}
