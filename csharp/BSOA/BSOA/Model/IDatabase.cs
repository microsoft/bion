// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.IO;

using System.Collections.Generic;

namespace BSOA.Model
{
    public interface IDatabase : ITreeSerializable
    {
        // Database exposes Tables to hook up Garbage Collection
        Dictionary<string, ITable> Tables { get; }

        // Database exposes Collect() so users can explicitly request Garbage Collection
        bool Collect();
    }
}
