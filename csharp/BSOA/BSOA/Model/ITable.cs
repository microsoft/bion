// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using BSOA.GC;

namespace BSOA.Model
{
    public interface ITable<T> : IColumn<T>, ITable where T : IRow<T>
    { }

    public interface ITable : IColumn
    {
        // ITable exposes Columns to hook up Garbage Collection (GC builds a map of IRefColumns to find reachable items and update indices)
        Dictionary<string, IColumn> Columns { get; }

        // Set named column fields on table from current Columns Dictionary values; build missing columns.
        void GetOrBuildColumns();

        // Fix count on a table after GC-related shenanigans. :/
        void SetCount(int count);

        // Hook to set updater which redirects object model instances to the current table and index (when Garbage Collection must move them)
        RowUpdater Updater { get; set; }

        // Method object model instances call to trigger redirection if it is needed
        void EnsureCurrent(IRow item);
    }
}
