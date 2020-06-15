// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace ScaleDemo.SoA
{
    public interface IColumn<T> : IBinarySerializable
    {
        T this[int index] { get; set; }
    }
}
