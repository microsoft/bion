// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;

using ScaleDemo.SoA;

namespace ScaleDemo
{
    /// <summary>
    ///  Region4 is a struct *and* uses the SoA (struct of Arrays) pattern,
    ///  allowing reading by column in bulk and avoiding any storage for unused columns.
    /// </summary>
    public struct Region4
    {
        private RegionTable Table { get; set; }
        private int Index { get; set; }

        public Region4(RegionTable table)
        {
            table.Add();
            this.Table = table;
            this.Index = (table.Count - 1);
        }

        public Region4(RegionTable table, int index)
        {
            this.Table = table;
            this.Index = index;
        }

        public int StartLine
        {
            get => Table.StartLine[Index];
            set => Table.StartLine[Index] = value;
        }

        public int StartColumn
        {
            get => Table.StartColumn[Index];
            set => Table.StartColumn[Index] = value;
        }

        public int EndLine
        {
            get => Table.EndLine[Index];
            set => Table.EndLine[Index] = value;
        }

        public int EndColumn
        {
            get => Table.EndColumn[Index];
            set => Table.EndColumn[Index] = value;
        }

        public int ByteOffset
        {
            get => Table.ByteOffset[Index];
            set => Table.ByteOffset[Index] = value;
        }

        public int ByteLength
        {
            get => Table.ByteLength[Index];
            set => Table.ByteLength[Index] = value;
        }

        public int CharOffset
        {
            get => Table.CharOffset[Index];
            set => Table.CharOffset[Index] = value;
        }

        public int CharLength
        {
            get => Table.CharLength[Index];
            set => Table.CharLength[Index] = value;
        }
    }

    [JsonConverter(typeof(RegionTableConverter))]
    public class RegionTable : ITable<Region4>
    {
        internal IntColumn StartLine;
        internal IntColumn StartColumn;
        internal IntColumn EndLine;
        internal IntColumn EndColumn;

        internal IntColumn ByteOffset;
        internal IntColumn ByteLength;
        internal IntColumn CharOffset;
        internal IntColumn CharLength;

        public RegionTable()
        {
            this.StartLine = new IntColumn(0);
            this.StartColumn = new IntColumn(0);
            this.EndLine = new IntColumn(0);
            this.EndColumn = new IntColumn(0);

            this.ByteOffset = new IntColumn(-1);
            this.ByteLength = new IntColumn(0);
            this.CharOffset = new IntColumn(-1);
            this.CharLength = new IntColumn(0);
        }

        public int Count { get; private set; }
        public Region4 this[int index] => new Region4(this, index);

        internal Region4 Add()
        {
            this.Count++;
            return this[this.Count - 1];
        }

        public void Read(BinaryReader reader, ref byte[] buffer)
        {
            this.Count = reader.ReadInt32();
            this.StartLine.Read(reader, ref buffer);
            this.StartColumn.Read(reader, ref buffer);
            this.EndLine.Read(reader, ref buffer);
            this.EndColumn.Read(reader, ref buffer);
            this.ByteOffset.Read(reader, ref buffer);
            this.ByteLength.Read(reader, ref buffer);
            this.CharOffset.Read(reader, ref buffer);
            this.CharLength.Read(reader, ref buffer);
        }

        public void Write(BinaryWriter writer, ref byte[] buffer)
        {
            writer.Write(this.Count);
            this.StartLine.Write(writer, ref buffer);
            this.StartColumn.Write(writer, ref buffer);
            this.EndLine.Write(writer, ref buffer);
            this.EndColumn.Write(writer, ref buffer);
            this.ByteOffset.Write(writer, ref buffer);
            this.ByteLength.Write(writer, ref buffer);
            this.CharOffset.Write(writer, ref buffer);
            this.CharLength.Write(writer, ref buffer);
        }

        public IEnumerator<Region4> GetEnumerator()
        {
            return new TableEnumerator<Region4>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TableEnumerator<Region4>(this);
        }
    }
}
