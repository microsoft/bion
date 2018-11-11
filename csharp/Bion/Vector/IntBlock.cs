using Bion.IO;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Bion.Vector
{
    public struct IntBlockPlan
    {
        public const int BlockSize = 128;
        public const byte CountMarker = 0x00;
        public const byte BaseMarker = 0x01;
        public const byte SlopeMarker = 0x05;
        public const byte AdjustmentMarker = 0x10;

        public byte Count;
        public int Base;
        public int Slope;
        public int MaxAdjustment;

        public byte BaseBytes;
        public byte SlopeBytes;
        public byte BitsPerAdjustment;

        public byte AdjustmentCount;
        public int TotalBytes;

        public IntBlockPlan(byte count, int baseV, int slope, int maxAdjustment)
        {
            this.Count = count;
            this.Base = baseV;
            this.Slope = slope;
            this.MaxAdjustment = maxAdjustment;

            // Must write an even multiple of 8 counts
            this.AdjustmentCount = RequiredCount(count);

            // Find widths of each component
            this.BaseBytes = ByteLength(baseV);
            this.SlopeBytes = ByteLength(slope);
            this.BitsPerAdjustment = BitLength(maxAdjustment);

            // Adjustment marker (always) plus other markers plus value bytes
            this.TotalBytes = 1 + ((this.BitsPerAdjustment * AdjustmentCount) / 8);
            if (this.Base != 0) { this.TotalBytes += 1 + this.BaseBytes; }
            if (this.Slope != 0) { this.TotalBytes += 1 + this.SlopeBytes; }
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
            if (value == 0) return 0;
            if (value < 0 || value >= 16777216) { return 4; }
            if (value >= 65536) { return 3; }
            if (value >= 256) { return 2; }
            return 1;
        }

        public static byte RequiredCount(byte count)
        {
            // Count must be the next even multiple of 8
            return (byte)((count + 7) & ~7);
        }

        public static IntBlockPlan Plan(int[] values, byte count)
        {
            // If only one value, write base only
            if (count == 1) { return new IntBlockPlan(count, values[0], 0, 0); }

            int first = values[0];
            int min = first;
            int max = min;
            int minSlope = values[1] - first;

            // Find Min/Max and Min/Max Slope
            for (int i = 1; i < count; ++i)
            {
                int value = values[i];
                if (value < min) { min = value; }
                if (value > max) { max = value; }

                int slope = (value - first) / i;
                if (slope < minSlope) { minSlope = slope; }
            }

            // Determine biggest adjustment if using base+slope
            int maxAdjustment = 0;
            for (int i = 1; i < count; ++i)
            {
                int line = first + minSlope * i;
                int adjustment = values[i] - line;
                if (adjustment > maxAdjustment) { maxAdjustment = adjustment; }
            }

            IntBlockPlan adjustmentOnly = new IntBlockPlan(count, 0, 0, (min >= 0 ? max : min));
            IntBlockPlan withBase = new IntBlockPlan(count, min, 0, max - min);
            IntBlockPlan baseAndSlope = new IntBlockPlan(count, first, minSlope, maxAdjustment);

            IntBlockPlan plan = adjustmentOnly;
            if (withBase.TotalBytes < plan.TotalBytes) { plan = withBase; }
            if (baseAndSlope.TotalBytes < plan.TotalBytes) { plan = baseAndSlope; }

            return plan;
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

        public IntBlockWriter(BufferedWriter writer)
        {
            _writer = writer;
            _buffer = new int[IntBlockPlan.BlockSize];
            _bufferCount = 0;
        }

        public void Write(int value)
        {
            _buffer[_bufferCount++] = value;
            if (_bufferCount == IntBlockPlan.BlockSize) { WriteBlock(); }
        }

        private void WriteBlock()
        {
            if (_bufferCount == 0) { return; }

            // Choose how to encode
            IntBlockPlan plan = IntBlockPlan.Plan(_buffer, _bufferCount);

            _writer.EnsureSpace(plan.TotalBytes);

            // Write count (if needed)
            if (plan.Count != IntBlockPlan.BlockSize)
            {
                _writer.Buffer[_writer.Index++] = IntBlockPlan.CountMarker;
                _writer.Buffer[_writer.Index++] = plan.Count;
            }

            // Write base, if needed
            WriteComponent(plan.Base, plan.BaseBytes, IntBlockPlan.BaseMarker);

            // Write slope, if needed
            WriteComponent(plan.Slope, plan.SlopeBytes, IntBlockPlan.SlopeMarker);

            // Write adjustment marker
            _writer.Buffer[_writer.Index++] = (byte)(IntBlockPlan.AdjustmentMarker + plan.BitsPerAdjustment);

            // Write adjustment bits
            if (plan.BitsPerAdjustment > 0)
            {
                int bitsLeftToWrite = 0;
                ulong toWrite = 0;

                for (int i = 0; i < plan.AdjustmentCount; ++i)
                {
                    // Calculate adjustment (zero once out of real values)
                    ulong adjustment = unchecked((uint)(i >= plan.Count ? 0 : _buffer[i] - (plan.Base + plan.Slope * i)));

                    // Add new value to bits left to write at top of long
                    toWrite += adjustment << (64 - plan.BitsPerAdjustment - bitsLeftToWrite);

                    // Write all whole bytes built
                    bitsLeftToWrite += plan.BitsPerAdjustment;
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

        private void WriteComponent(int component, byte byteLength, byte componentMarkerBase)
        {
            if (component == 0) { return; }

            _writer.Buffer[_writer.Index++] = (byte)(componentMarkerBase + byteLength - 1);

            for (byte i = 0; i < byteLength; ++i)
            {
                _writer.Buffer[_writer.Index++] = (byte)(component & 0xFF);
                component = component >> 8;
            }
        }

        public void Dispose()
        {
            WriteBlock();

            _writer?.Dispose();
            _writer = null;
        }
    }

    public class IntBlockReader : IDisposable
    {
        private BufferedReader _reader;
        private int[] _buffer;

        public IntBlockReader(BufferedReader reader)
        {
            _reader = reader;
            _buffer = new int[IntBlockPlan.BlockSize];
        }

        public int Next(out int[] buffer)
        {
            buffer = _buffer;
            if (_reader.EndOfStream) { return 0; }

            // Read enough for a max length block
            _reader.EnsureSpace(513);

            // TODO: Reuse plan on read to store components.

            // Component defaults
            byte count = IntBlockPlan.BlockSize;
            int baseV = 0;
            int slope = 0;

            // Read components
            byte marker = _reader.Buffer[_reader.Index++];
            while (marker < IntBlockPlan.AdjustmentMarker)
            {
                if (marker == IntBlockPlan.CountMarker)
                {
                    count = _reader.Buffer[_reader.Index++];
                }
                else if (marker < IntBlockPlan.SlopeMarker)
                {
                    baseV = ReadComponent(marker, IntBlockPlan.BaseMarker);
                }
                else
                {
                    slope = ReadComponent(marker, IntBlockPlan.SlopeMarker);
                }

                marker = _reader.Buffer[_reader.Index++];
            }

            byte adjustmentMarker = marker;
            byte adjustmentBitLength = (byte)(marker - IntBlockPlan.AdjustmentMarker);

            //ReadScalar(count, baseV, slope, adjustmentBitLength);
            IntBlockVectorReader.ReadVector(count, baseV, slope, adjustmentBitLength, _reader, _buffer);
       
            return count;
        }

        private void ReadScalar(byte count, int baseV, int slope, byte adjustmentBitLength)
        {
            if (adjustmentBitLength == 0)
            {
                // If no adjustments, calculate each value
                int unadjusted = baseV;
                for (int i = 0; i < count; ++i)
                {
                    _buffer[i] = unadjusted;
                    unadjusted += slope;
                }
            }
            else
            {
                // Build a mask to get the low adjustmentBitLength bits
                ulong mask = ulong.MaxValue >> (64 - adjustmentBitLength);

                byte bitsLeftToRead = 0;
                ulong pending = 0;

                int countWritten = IntBlockPlan.RequiredCount(count);
                int unadjusted = baseV;

                for (int i = 0; i < countWritten; ++i)
                {
                    // Read enough bits to get value
                    while (bitsLeftToRead < adjustmentBitLength)
                    {
                        pending = pending << 8;
                        pending += _reader.Buffer[_reader.Index++];
                        bitsLeftToRead += 8;
                    }

                    // Extract correct bits and build value
                    int adjustment = unchecked((int)(((pending >> (bitsLeftToRead - adjustmentBitLength)) & mask)));
                    _buffer[i] = unadjusted + adjustment;

                    bitsLeftToRead -= adjustmentBitLength;
                    unadjusted += slope;
                }
            }
        }

        private int ReadComponent(byte componentMarker, byte componentMarkerBase)
        {
            byte byteLength = (byte)(1 + componentMarker - componentMarkerBase);

            int component = 0;
            for (byte i = 0; i < byteLength; ++i)
            {
                component += _reader.Buffer[_reader.Index++] << (i * 8);
            }
            return component;
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _reader = null;
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

        public static unsafe void ReadVector(byte count, int baseV, int slope, byte adjustmentBitLength, BufferedReader reader, int[] buffer)
        {
            // Build first unadjusted vector and per-vector increment
            Vector128<int> unadjusted = SetIncrement(baseV, slope);
            Vector128<int> increment = Set1(slope * 4);

            if (adjustmentBitLength == 0)
            {
                // If no adjustments, calculate in blocks and return
                fixed (int* resultPtr = buffer)
                {
                    for (int i = 0; i < count; i += 4)
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
                int index = reader.Index;

                // Calculate bytes consumed for the first and second four ints decoded (different for odd bit lengths)
                byte bytesPerEight = adjustmentBitLength;
                byte bytes1 = (byte)(bytesPerEight / 2);

                // Calculate how much to shift values (from top of each int to bottom)
                byte shiftRightBits = (byte)(32 - adjustmentBitLength);

                // Get shuffle mask (to get correct bits) and multiply value (to shift to top of each int) for halves
                Vector128<sbyte> shuffle1 = Unsafe.ReadUnaligned<Vector128<sbyte>>(&shuffleMaskPtr[32 * adjustmentBitLength]);
                Vector128<int> multiply1 = Unsafe.ReadUnaligned<Vector128<int>>(&multiplyMaskPtr[8 * adjustmentBitLength]);

                Vector128<sbyte> shuffle2 = Unsafe.ReadUnaligned<Vector128<sbyte>>(&shuffleMaskPtr[32 * adjustmentBitLength + 16]);
                Vector128<int> multiply2 = Unsafe.ReadUnaligned<Vector128<int>>(&multiplyMaskPtr[8 * adjustmentBitLength + 4]);

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
