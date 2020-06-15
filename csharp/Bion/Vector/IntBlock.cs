// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

using Bion.IO;

namespace Bion.Vector
{
    public class IntBlockStats
    {
        public int TotalBlockCount;
        public int AdjustmentOnlyCount;
        public int BaseCount;
        public int BaseAndSlopeCount;
        public int[] CountPerLength;

        public IntBlockStats()
        {
            this.CountPerLength = new int[33];
        }

        public void Add(IntBlock block)
        {
            TotalBlockCount++;

            if (block.Slope != 0)
            {
                BaseAndSlopeCount++;
            }
            else if (block.Base != 0)
            {
                BaseCount++;
            }
            else
            {
                AdjustmentOnlyCount++;
            }

            CountPerLength[block.BitsPerAdjustment]++;
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine($"Total: {TotalBlockCount:n0}");
            result.AppendLine($"AdjOnly: {AdjustmentOnlyCount:n0}");
            result.AppendLine($"Base: {BaseCount:n0}");
            result.AppendLine($"BaseAndSlope: {BaseAndSlopeCount:n0}");
            result.AppendLine();

            for (int i = 0; i < this.CountPerLength.Length; ++i)
            {
                result.AppendLine($"{i} bit: {this.CountPerLength[i]:n0}");
            }

            return result.ToString();
        }
    }

    public struct IntBlock
    {
        public const int BlockSize = 128;
        public const byte CountMarker = 0x00;
        public const byte BaseMarker = 0x01;
        public const byte SlopeMarker = 0x05;
        public const byte AdjustmentMarker = 0x10;

        public byte Count;
        public int Base;
        public int Slope;
        public byte BitsPerAdjustment;

        public byte AdjustmentCount;

        public IntBlock(byte count, int baseV, int slope, byte bitsPerAdjustment)
        {
            this.Count = count;
            this.Base = baseV;
            this.Slope = slope;
            this.BitsPerAdjustment = bitsPerAdjustment;

            // Must write an even multiple of 8 counts
            this.AdjustmentCount = RequiredCount(count);
        }

        public void Write(BufferedWriter writer)
        {
            writer.EnsureSpace(this.TotalBytes);

            // Write count (if needed)
            if (this.Count != IntBlock.BlockSize)
            {
                writer.Buffer[writer.Index++] = CountMarker;
                writer.Buffer[writer.Index++] = this.Count;
            }

            // Write base, if needed
            WriteComponent(writer, this.Base, BaseMarker);

            // Write slope, if needed
            WriteComponent(writer, this.Slope, SlopeMarker);

            // Write adjustment marker
            writer.Buffer[writer.Index++] = (byte)(AdjustmentMarker + this.BitsPerAdjustment);
        }

        private void WriteComponent(BufferedWriter writer, int component, byte componentMarkerBase)
        {
            if (component == 0) { return; }
            byte byteLength = IntBlock.ByteLength(component);

            writer.Buffer[writer.Index++] = (byte)(componentMarkerBase + byteLength - 1);

            for (byte i = 0; i < byteLength; ++i)
            {
                writer.Buffer[writer.Index++] = (byte)(component & 0xFF);
                component = component >> 8;
            }
        }

        public static IntBlock Read(BufferedReader reader)
        {
            // Read enough for a max length block
            reader.EnsureSpace(513);

            // Component defaults
            byte count = IntBlock.BlockSize;
            int baseV = 0;
            int slope = 0;

            // Read components
            byte marker = reader.Buffer[reader.Index++];
            while (marker < IntBlock.AdjustmentMarker)
            {
                if (marker == IntBlock.CountMarker)
                {
                    count = reader.Buffer[reader.Index++];
                }
                else if (marker < IntBlock.SlopeMarker)
                {
                    baseV = ReadComponent(reader, marker, IntBlock.BaseMarker);
                }
                else
                {
                    slope = ReadComponent(reader, marker, IntBlock.SlopeMarker);
                }

                marker = reader.Buffer[reader.Index++];
            }

            byte adjustmentMarker = marker;
            byte adjustmentBitLength = (byte)(marker - IntBlock.AdjustmentMarker);

            return new IntBlock(count, baseV, slope, adjustmentBitLength);
        }

        private static int ReadComponent(BufferedReader reader, byte componentMarker, byte componentMarkerBase)
        {
            byte byteLength = (byte)(1 + componentMarker - componentMarkerBase);

            int component = 0;
            for (byte i = 0; i < byteLength; ++i)
            {
                component += reader.Buffer[reader.Index++] << (i * 8);
            }
            return component;
        }

        public static IntBlock Plan(int[] values, int index, int endIndex)
        {
            byte count = (byte)(endIndex - index);

            // If only one value, write base only
            if (count == 1) { return new IntBlock(count, values[index], 0, 0); }

            int first = values[index];

            // Compute overall slope and consider values around exact (non-int) slope
            int slopeL = (values[index + count - 1] - first) / (count - 1);
            int slopeH = slopeL + 1;

            int min = first;
            int max = first;
            int minL = first;
            int maxL = first;
            int minH = first;
            int maxH = first;

            // Find absolute and slope-relative min and max
            int line = 0;
            for (int i = 1; i < count; ++i)
            {
                int value = values[index + i];

                if (value < min) { min = value; }
                if (value > max) { max = value; }

                line += slopeL;
                int adjustment = value - line;
                if (adjustment < minL) { minL = adjustment; }
                if (adjustment > maxL) { maxL = adjustment; }

                adjustment = value - (line + i);
                if (adjustment < minH) { minH = adjustment; }
                if (adjustment > maxH) { maxH = adjustment; }
            }

            // Measure adjustments from slope and identify ideal base
            IntBlock adjustmentOnly = new IntBlock(count, 0, 0, (min < 0 ? (byte)32 : BitLength(max)));
            IntBlock withBase = new IntBlock(count, min, 0, BitLength(max - min));
            IntBlock baseAndSlopeL = new IntBlock(count, minL, slopeL, BitLength(maxL - minL));
            IntBlock baseAndSlopeH = new IntBlock(count, minH, slopeH, BitLength(maxH - minH));

            IntBlock plan = adjustmentOnly;
            if (withBase.TotalBytes < plan.TotalBytes) { plan = withBase; }
            if (baseAndSlopeL.TotalBytes < plan.TotalBytes) { plan = baseAndSlopeL; }
            if (baseAndSlopeH.TotalBytes < plan.TotalBytes) { plan = baseAndSlopeH; }

            return plan;
        }

        private static int PredictedSlope(int[] values, int index, int endIndex)
        {
            byte count = (byte)(endIndex - index);

            int first = values[index];
            long sumY = first;
            long sumXY = 0;

            // Find Min/Max and compute sums to derive trendline slope
            for (int i = 1; i < count; ++i)
            {
                int value = values[index + i];
                sumY += value;
                sumXY += i * value;
            }

            // Calculate slope
            double denominator = (double)((count - 1) * (count) * (count + 1));
            double numerator = (double)(6 * ((2 * sumXY) - ((count - 1) * sumY)));

            return (int)(numerator / denominator);
        }

        public int TotalBytes
        {
            get
            {
                int bytes = 1 + ((this.BitsPerAdjustment * AdjustmentCount) / 8);
                if (this.Base != 0) { bytes += 1 + ByteLength(this.Base); }
                if (this.Slope != 0) { bytes += 1 + ByteLength(this.Slope); }

                return bytes;
            }
        }

        public static byte BitLength(int maxValue)
        {
            // Negative and 29+ bits must be 32 bits
            if (maxValue < 0 || maxValue >= 268435456) { return 32; }

            // 26-28 bits must be 28 bits
            if (maxValue >= 67108864) { return 28; }

            // Otherwise, count bits to represent
            byte bits = 0;
            int valueLeft = maxValue;
            while (valueLeft > 0)
            {
                bits++;
                valueLeft = valueLeft >> 1;
            }

            return bits;
        }

        public static byte ByteLength(int value)
        {
            if (value == 0) { return 0; }
            if (value < 0) { return 4; }
            if (value < 256) { return 1; }
            if (value < 65536) { return 2; }
            if (value < 16777216) { return 3; }
            return 4;
        }

        public static byte RequiredCount(byte count)
        {
            // Count must be the next even multiple of 8
            return (byte)((count + 7) & ~7);
        }
    }

    /// <summary>
    ///  IntBlockWriter writes compact integer streams in the IntBlock format.
    /// </summary>
    public class IntBlockWriter : IDisposable
    {
        private BufferedWriter _writer;
        private int[] _buffer;
        private byte _bufferCount;

        public IntBlockStats Stats;

        public IntBlockWriter(BufferedWriter writer)
        {
            _writer = writer;
            _buffer = new int[IntBlock.BlockSize];
            _bufferCount = 0;

            Stats = new IntBlockStats();
        }

        public void Write(int value)
        {
            _buffer[_bufferCount++] = value;
            if (_bufferCount == IntBlock.BlockSize) { WriteBlock(_buffer, 0, _bufferCount); }
        }

        public void Write(int[] array, int index, int endIndex)
        {
            int length = endIndex - index;
            if (_bufferCount == 0 && length == IntBlock.BlockSize)
            {
                WriteBlock(array, index, endIndex);
            }
            else
            {
                while (index < endIndex)
                {
                    int countToCopy = Math.Min(IntBlock.BlockSize - _bufferCount, endIndex - index);
                    Buffer.BlockCopy(array, index, _buffer, _bufferCount, countToCopy * 4);
                    index += countToCopy;

                    if (_bufferCount == IntBlock.BlockSize) { WriteBlock(_buffer, 0, _bufferCount); }
                }
            }
        }

        private void WriteBlock(int[] array, int index, int endIndex)
        {
            int length = endIndex - index;
            if (length == 0) { return; }

            // Choose how to encode
            IntBlock block = IntBlock.Plan(array, index, endIndex);

            Stats?.Add(block);

            // Write the block markers
            block.Write(_writer);
            _writer.EnsureSpace(block.TotalBytes);

            // Write adjustment bits
            if (block.BitsPerAdjustment > 0)
            {
                int bitsLeftToWrite = 0;
                ulong toWrite = 0;

                for (int i = 0; i < block.AdjustmentCount; ++i)
                {
                    // Calculate adjustment (zero once out of real values)
                    ulong adjustment = unchecked((uint)(i >= block.Count ? 0 : array[index + i] - (block.Base + block.Slope * i)));

                    // Add new value to bits left to write at top of long
                    toWrite += adjustment << (64 - block.BitsPerAdjustment - bitsLeftToWrite);

                    // Write all whole bytes built
                    bitsLeftToWrite += block.BitsPerAdjustment;
                    while (bitsLeftToWrite >= 8)
                    {
                        // Write top byte
                        byte nextByte = (byte)(toWrite >> 56);
                        _writer.Buffer[_writer.Index++] = nextByte;

                        // Shift to next byte
                        toWrite = toWrite << 8;
                        bitsLeftToWrite -= 8;
                    }
                }
            }

            // Clear buffer; it's been written
            _bufferCount = 0;
        }

        public void Dispose()
        {
            WriteBlock(_buffer, 0, _bufferCount);

            _writer?.Dispose();
            _writer = null;
        }
    }

    public class IntBlockReader : IEnumerable<Memory<int>>, IDisposable
    {
        private BufferedReader _reader;
        private int[] _buffer;

        public IntBlockReader(BufferedReader reader)
        {
            _reader = reader;
            _buffer = new int[IntBlock.BlockSize];
        }

        public int Next(out int[] buffer)
        {
            buffer = _buffer;
            if (_reader.EndOfStream) { return 0; }

            // Read markers
            IntBlock block = IntBlock.Read(_reader);

            // Read adjustments
            //ReadScalar(block);
            IntBlockVectorReader.ReadVector(block, _reader, _buffer);

            return block.Count;
        }

        private void ReadScalar(IntBlock block)
        {
            if (block.BitsPerAdjustment == 0)
            {
                // If no adjustments, calculate each value
                int unadjusted = block.Base;
                for (int i = 0; i < block.Count; ++i)
                {
                    _buffer[i] = unadjusted;
                    unadjusted += block.Slope;
                }
            }
            else
            {
                // Build a mask to get the low adjustmentBitLength bits
                byte bitsPerAdjustment = block.BitsPerAdjustment;
                ulong mask = ulong.MaxValue >> (64 - bitsPerAdjustment);

                byte bitsLeftToRead = 0;
                ulong pending = 0;

                int countWritten = IntBlock.RequiredCount(block.Count);
                int unadjusted = block.Base;

                for (int i = 0; i < countWritten; ++i)
                {
                    // Read enough bits to get value
                    while (bitsLeftToRead < bitsPerAdjustment)
                    {
                        pending = pending << 8;
                        pending += _reader.Buffer[_reader.Index++];
                        bitsLeftToRead += 8;
                    }

                    // Extract correct bits and build value
                    int adjustment = unchecked((int)(((pending >> (bitsLeftToRead - bitsPerAdjustment)) & mask)));
                    _buffer[i] = unadjusted + adjustment;

                    bitsLeftToRead -= bitsPerAdjustment;
                    unadjusted += block.Slope;
                }
            }
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _reader = null;
        }

        //public Enumerator GetEnumerator()
        //{
        //    return new Enumerator(this);
        //}

        //IEnumerator<int> IEnumerable<int>.GetEnumerator()
        //{
        //    return new Enumerator(this);
        //}

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new PageEnumerator(this);
        }

        public PageEnumerator GetEnumerator()
        {
            return new PageEnumerator(this);
        }

        IEnumerator<Memory<int>> IEnumerable<Memory<int>>.GetEnumerator()
        {
            return new PageEnumerator(this);
        }

        public struct Enumerator : IEnumerator<int>
        {
            private IntBlockReader _reader;
            private int[] _currentPage;
            private int _currentPageCount;
            private int _index;
            
            internal Enumerator(IntBlockReader reader)
            {
                _reader = reader;

                _currentPage = null;
                _currentPageCount = 0;

                Current = 0;
                _index = -1;
                
                Reset();
            }

            public int Current { get; private set; }
            object IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (_index >= _currentPageCount)
                {
                    _currentPageCount = _reader.Next(out _currentPage);
                    if (_currentPageCount == 0) { return false; }
                    _index = 0;
                }

                Current = _currentPage[_index];
                _index++;
                return true;
            }

            public void Reset()
            {
                _reader._reader.Seek(0, System.IO.SeekOrigin.Begin);

                _currentPageCount = 0;
                Current = 0;
                _index = 0;
            }

            public void Dispose()
            { }
        }

        public struct PageEnumerator : IEnumerator<Memory<int>>
        {
            private IntBlockReader _reader;
            public Memory<int> Current { get; private set; }
            object IEnumerator.Current => Current;

            internal PageEnumerator(IntBlockReader reader)
            {
                _reader = reader;
                Current = default(Memory<int>);
                Reset();
            }

            public bool MoveNext()
            {
                int count = _reader.Next(out int[] array);
                Current = array.AsMemory(0, count);
                return count > 0;
            }

            public void Reset()
            {
                _reader._reader.Seek(0, System.IO.SeekOrigin.Begin);
                Current = default(Memory<int>);
            }

            public void Dispose()
            { }
        }
    }

    internal static class IntBlockVectorReader
    {
        private static sbyte[] ShuffleMasks;
        private static int[] MultiplyMasks;

        static IntBlockVectorReader()
        {
            // Build Shuffle and Multiply masks for each possible increment bit length
            ShuffleMasks = new sbyte[32 * 33];
            MultiplyMasks = new int[8 * 33];

            int shuffleIndex = 0;
            int multiplyIndex = 0;

            // For Each bit length... 
            for (int bitLength = 0; bitLength <= 32; ++bitLength)
            {
                int bitOffset = 0;

                // For the two halves (we decode two 4-int segments at a time)
                for (int vectorIndex = 0; vectorIndex < 2; ++vectorIndex)
                {
                    // For each int to decode
                    for (int intIndex = 0; intIndex < 4; ++intIndex)
                    {
                        // The shuffle mask selects the four bytes where the bits begin
                        for (int byteIndex = 0; byteIndex < 4; ++byteIndex)
                        {
                            ShuffleMasks[shuffleIndex++] = (sbyte)((bitOffset / 8) + (3 - byteIndex));
                        }

                        // The multiply mask shifts the bits to the top of that byte
                        MultiplyMasks[multiplyIndex++] = 1 << (bitOffset & 7);

                        // Update bit offset for next compressed int
                        bitOffset += bitLength;
                    }

                    // The second 4 ints will load the then-closest byte, resetting the byte offset
                    bitOffset = bitOffset & 7;
                }
            }
        }

        public static unsafe void ReadVector(IntBlock block, BufferedReader reader, int[] buffer)
        {
            // Build first unadjusted vector and per-vector increment
            Vector128<int> unadjusted = SetIncrement(block.Base, block.Slope);
            Vector128<int> increment = Set1(block.Slope * 4);

            if (block.BitsPerAdjustment == 0)
            {
                // If no adjustments, calculate in blocks and return
                fixed (int* resultPtr = buffer)
                {
                    for (int i = 0; i < block.Count; i += 4)
                    {
                        Unsafe.WriteUnaligned(&resultPtr[i], unadjusted);
                        unadjusted = Sse2.Add(unadjusted, increment);
                    }
                }

                return;
            }

            fixed (byte* bufferPtr = reader.Buffer)
            fixed (int* resultPtr = buffer)
            fixed (sbyte* shuffleMaskPtr = ShuffleMasks)
            fixed (int* multiplyMaskPtr = MultiplyMasks)
            {
                byte bitsPerAdjustment = block.BitsPerAdjustment;
                int index = reader.Index;
                int count = block.Count;

                // Calculate bytes consumed for the first and second four ints decoded (different for odd bit lengths)
                byte bytesPerEight = bitsPerAdjustment;
                byte bytes1 = (byte)(bytesPerEight / 2);

                // Calculate how much to shift values (from top of each int to bottom)
                byte shiftRightBits = (byte)(32 - bitsPerAdjustment);

                // Get shuffle mask (to get correct bits) and multiply value (to shift to top of each int) for halves
                Vector128<sbyte> shuffle1 = Unsafe.ReadUnaligned<Vector128<sbyte>>(&shuffleMaskPtr[32 * bitsPerAdjustment]);
                Vector128<int> multiply1 = Unsafe.ReadUnaligned<Vector128<int>>(&multiplyMaskPtr[8 * bitsPerAdjustment]);

                Vector128<sbyte> shuffle2 = Unsafe.ReadUnaligned<Vector128<sbyte>>(&shuffleMaskPtr[32 * bitsPerAdjustment + 16]);
                Vector128<int> multiply2 = Unsafe.ReadUnaligned<Vector128<int>>(&multiplyMaskPtr[8 * bitsPerAdjustment + 4]);

                for (int i = 0; i < count; i += 8, index += bytesPerEight)
                {
                    // Read source bytes
                    Vector128<int> vector1 = Unsafe.ReadUnaligned<Vector128<int>>(&bufferPtr[index]);
                    Vector128<int> vector2 = Unsafe.ReadUnaligned<Vector128<int>>(&bufferPtr[index + bytes1]);

                    // Shuffle to get the right bytes in each integer
                    vector1 = Sse.StaticCast<sbyte, int>(Ssse3.Shuffle(Sse.StaticCast<int, sbyte>(vector1), shuffle1));
                    vector2 = Sse.StaticCast<sbyte, int>(Ssse3.Shuffle(Sse.StaticCast<int, sbyte>(vector2), shuffle2));

                    // Multiply to shift each int so the desired bits are at the top
                    vector1 = Sse41.MultiplyLow(vector1, multiply1);
                    vector2 = Sse41.MultiplyLow(vector2, multiply2);

                    // Shift the desired bits to the bottom and zero the top
                    vector1 = Sse2.ShiftRightLogical(vector1, shiftRightBits);
                    vector2 = Sse2.ShiftRightLogical(vector2, shiftRightBits);

                    // Add the delta base value
                    vector1 = Sse2.Add(vector1, unadjusted);
                    unadjusted = Sse2.Add(unadjusted, increment);

                    vector2 = Sse2.Add(vector2, unadjusted);
                    unadjusted = Sse2.Add(unadjusted, increment);

                    // Write the decoded integers
                    Unsafe.WriteUnaligned(&resultPtr[i], vector1);
                    Unsafe.WriteUnaligned(&resultPtr[i + 4], vector2);
                }

                reader.Index = index;
            }
        }

        private static unsafe Vector128<int> Set1(int value)
        {
            int* array = stackalloc int[4];
            array[0] = value;
            array[1] = value;
            array[2] = value;
            array[3] = value;

            return Unsafe.ReadUnaligned<Vector128<int>>(array);
        }

        private static unsafe Vector128<int> SetIncrement(int baseV, int slope)
        {
            int current = baseV;

            int* array = stackalloc int[4];
            for (int i = 0; i < 4; ++i)
            {
                array[i] = current;
                current += slope;
            }

            return Unsafe.ReadUnaligned<Vector128<int>>(array);
        }
    }
}
