// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace BSOA.Model
{
    public interface ITable<T> : IColumn<T>, ITable where T : IRow<T>
    { }

    public interface ITable : IColumn
    {
        // ITable exposes Columns to hook up Garbage Collection (GC builds a map of IRefColumns to find reachable items and update indices)
        Dictionary<string, IColumn> Columns { get; }
    }
}
