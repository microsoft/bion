using Bion.IO;
using System;

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
            this.AdjustmentCount = (byte)((this.Count + 7) & ~7);

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
            return 0;
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _reader = null;
        }
    }
}
