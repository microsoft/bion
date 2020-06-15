// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IO;

namespace ScaleDemo.SoA
{
    public interface IBinarySerializable
    {
        void Read(BinaryReader reader, ref byte[] buffer);
        void Write(BinaryWriter writer, ref byte[] buffer);
    }
}
