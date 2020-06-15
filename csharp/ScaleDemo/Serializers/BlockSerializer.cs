// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.CodeAnalysis.Sarif;

namespace ScaleDemo
{
    public class BlockSerializer
    {
        private byte[] buffer = null;

        public void Serialize(BinaryWriter writer, RegionTable table)
        {
            table.Write(writer, ref buffer);
        }

        public RegionTable Deserialize(BinaryReader reader)
        {
            RegionTable table = new RegionTable();
            table.Read(reader, ref buffer);
            buffer = null;
            return table;
        }

        public RegionTable Convert(IList<Region> regions)
        {
            RegionTable table = new RegionTable();

            for (int i = 0; i < regions.Count; ++i)
            {
                Region4 region = new Region4(table);
                Convert(regions[i], region);
            }

            return table;
        }

        public void Convert(Region source, Region4 target)
        {
            target.StartLine = source.StartLine;
            target.StartColumn = source.StartColumn;
            target.EndLine = source.EndLine;
            target.EndColumn = source.EndColumn;
            target.ByteOffset = source.ByteOffset;
            target.ByteLength = source.ByteLength;
            target.CharOffset = source.CharOffset;
            target.CharLength = source.CharLength;
        }
    }
}
