// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace ScaleDemo.SoA
{
    public interface ITable<T> : IBinarySerializable, IReadOnlyList<T>
    { }
}
