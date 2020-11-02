// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.IO;

using System.Collections.Generic;

namespace BSOA.Model
{
    public interface IDatabase : ITreeSerializable
    {
        // Expose the root table name for Garbage Collection
        string RootTableName { get; }

        // Database exposes Tables to hook up Garbage Collection
        Dictionary<string, ITable> Tables { get; }

        // Set named table fields on database from current Tables Dictionary values.
        void GetOrBuildTables();

        // Database exposes Collect() so users can explicitly request Garbage Collection
        bool Collect();
    }
}
