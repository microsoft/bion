using Bion.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bion
{
    public unsafe class BionWriter : IDisposable
    {
        public const int MaxInlineInteger = 15;
        public const int MaxOneByteLength = 127;
        public const int MaxTwoByteLength = 16383;

        private BufferedWriter _writer;
        private Stack<long> _containers;

        public long BytesWritten => _writer.BytesWritten;

        public BionWriter(Stream stream) : this(new BufferedWriter(stream))
        { }

        public BionWriter(BufferedWriter writer)
        {
            _writer = writer;
            _containers = new Stack<long>();
        }

        public void WriteStartObject()
        {
            WriteStartContainer(BionMarker.StartObject);
        }

        public void WriteEndObject()
        {
            WriteEndContainer(BionMarker.EndObject);
        }

        public void WriteStartArray()
        {
            WriteStartContainer(BionMarker.StartArray);
        }

        public void WriteEndArray()
        {
            WriteEndContainer(BionMarker.EndArray);
        }

        public void WriteNull()
        {
            Write(BionMarker.Null);
        }

        public void WriteValue(bool value)
        {
            Write((value ? BionMarker.True : BionMarker.False));
        }

        public void WriteValue(bool? value)
        {
            if (!value.HasValue)
            {
                Write(BionMarker.Null);
            }
            else
            {
                Write((value.Value ? BionMarker.True : BionMarker.False));
            }
        }

        public void WriteValue(long value)
        {
            if (value < 0)
            {
                WriteVariableInteger(BionMarker.NegativeInteger, (ulong)(-value));
            }
            else if (value <= MaxInlineInteger)
            {
                Write(BionMarker.InlineInteger, (byte)value);
            }
            else
            {
                WriteVariableInteger(BionMarker.Integer, (ulong)value);
            }
        }

        public unsafe void WriteValue(float value)
        {
            // Coerce .NET to reinterpret the bytes as a uint and write that
            uint valueBytes = *(uint*)&value;
            WriteFixedInteger(BionMarker.Float, valueBytes, 5);
        }

        public unsafe void WriteValue(double value)
        {
            // See if the value can be represented with a float instead of a double
            if (value >= float.MinValue && value <= float.MaxValue)
            {
                float asFloat = (float)value;
                if ((double)asFloat == value)
                {
                    // If the float form converted back identically, store in five bytes
                    WriteValue(asFloat);
                    return;
                }
            }

            // Coerce .NET to reinterpret the bytes as a ulong and write that
            ulong valueBytes = *(ulong*)&value;
            WriteFixedInteger(BionMarker.Float, valueBytes, 10);
        }

        public void WriteValue(string value)
        {
            WriteStringValue(BionToken.String, value);
        }

        public void WriteValue(ReadOnlySpan<byte> utf8Text)
        {
            WriteStringValue(BionToken.String, utf8Text);
        }

        public void WritePropertyName(string name)
        {
            WriteStringValue(BionToken.PropertyName, name);
        }

        public void WritePropertyName(ReadOnlySpan<byte> utf8Text)
        {
            WriteStringValue(BionToken.PropertyName, utf8Text);
        }

        private void WriteStringLength(BionToken marker, int stringLength)
        {
            int lengthOfLength;
            int markerAdjustment;

            if (stringLength <= MaxOneByteLength)
            {
                lengthOfLength = 1;
                markerAdjustment = -2;
            }
            else if (stringLength <= MaxTwoByteLength)
            {
                lengthOfLength = 2;
                markerAdjustment = -1;
            }
            else
            {
                lengthOfLength = 5;
                markerAdjustment = 0;
            }

            _writer.EnsureSpace(lengthOfLength + 1);
            int index = _writer.Index++;
            NumberConverter.WriteSevenBitFixed(_writer, (ulong)stringLength, lengthOfLength);
            _writer.Buffer[index] = (byte)((int)marker + markerAdjustment);
        }

        private void WriteStringValue(BionToken markerType, ReadOnlySpan<byte> value)
        {
            // Write marker and length
            WriteStringLength(markerType, value.Length);

            // Write value
            _writer.EnsureSpace(value.Length);
            value.CopyTo(_writer.Buffer.AsSpan(_writer.Index));
            _writer.Index += value.Length;
        }

        private void WriteStringValue(BionToken markerType, string value)
        {
            if (value == null)
            {
                WriteNull();
                return;
            }

            int length = Encoding.UTF8.GetByteCount(value);

            // Write marker and length
            WriteStringLength(markerType, length);

            // Encode and writer value
            _writer.EnsureSpace(length);
            Encoding.UTF8.GetBytes(value, 0, value.Length, _writer.Buffer, _writer.Index);
            _writer.Index += length;
        }

        private void WriteVariableInteger(BionMarker marker, ulong value)
        {
            _writer.EnsureSpace(11);
            int index = _writer.Index++;

            byte length = NumberConverter.WriteSevenBitExplicit(_writer, value);
            _writer.Buffer[index] = (byte)(marker + length);
        }

        private void WriteFixedInteger(BionMarker marker, ulong value, byte length)
        {
            _writer.EnsureSpace(length + 1);
            int index = _writer.Index++;

            NumberConverter.WriteSevenBitFixed(_writer, value, length);
            _writer.Buffer[index] = (byte)(marker + length);
        }

        private void WriteStartContainer(BionMarker container)
        {
            Write(container);
            _containers.Push(_writer.BytesWritten);
        }

        private void WriteEndContainer(BionMarker container)
        {
            Write(container);

            long end = _writer.BytesWritten;
            long start = _containers.Pop();
            long length = _writer.BytesWritten - start - 4;
        }

        private void Write(BionMarker marker)
        {
            Write((byte)marker);
        }

        private void Write(BionMarker marker, byte lengthOrValue)
        {
            Write((byte)((byte)marker | lengthOrValue));
        }

        private void Write(byte value)
        {
            _writer.EnsureSpace(1);
            _writer.Buffer[_writer.Index++] = value;
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.Dispose();
                _writer = null;
            }
        }
    }
}
