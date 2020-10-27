// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace BSOA.Model
{
    public interface ITable<T> : IColumn<T>, ITable where T : IRow<T>
    { }

    public interface ITable : IColumn
    {
        Dictionary<string, IColumn> Columns { get; }
    }
}
