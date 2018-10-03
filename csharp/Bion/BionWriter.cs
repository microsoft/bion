using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bion
{
    public unsafe class BionWriter : IDisposable
    {
        private const int MaxOneByteLength = 127;   // 7 bits
        private const int MaxTwoByteLength = 16383; // 14 bits
        private const int MaxInlineInteger = 15;    // 4 bits

        public long BytesWritten { get; private set; }
        public bool CloseStream { get; set; }

        private Stream _stream;
        private Stack<long> _containers;

        private byte[] _buffer;

        public BionWriter(Stream stream)
        {
            _stream = new BufferedStream(stream);
            _containers = new Stack<long>();
            _buffer = new byte[1024];
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
            if(value < 0)
            {
                WriteInteger(BionMarker.NegativeInteger, (ulong)(-value));
            }
            else if (value <= MaxInlineInteger)
            {
                Write(BionMarker.InlineInteger, (byte)value);
            }
            else
            {
                WriteInteger(BionMarker.Integer, (ulong)value);
            }
        }

        public unsafe void WriteValue(double value)
        {
            // Coerce .NET to reinterpret the bytes as a ulong and write that
            WriteInteger(BionMarker.Float, *(ulong*)&value);
        }

        public void WriteValue(string value)
        {
            WriteStringValue(BionMarker.String, value, 0, value?.Length ?? 0);
        }

        public void WriteValue(string value, int index, int count)
        {
            WriteStringValue(BionMarker.String, value, index, count);
        }

        public void WritePropertyName(string name)
        {
            WriteStringValue(BionMarker.PropertyName, name, 0, name?.Length ?? 0);
        }

        public void WritePropertyName(string name, int index, int count)
        {
            WriteStringValue(BionMarker.PropertyName, name, index, count);
        }

        private void WriteInteger(BionMarker marker, ulong value)
        {
            byte length = 1;
            while (value > 0)
            {
                _buffer[length++] = (byte)(value & 0x7F);
                value = value >> 7;
            }

            _buffer[0] = (byte)(marker + length - 1);

            _stream.Write(_buffer, 0, length);
            BytesWritten += length;
        }

        private void WriteStringValue(BionMarker markerType, string value, int index, int count)
        {
            if (value == null)
            {
                WriteNull();
                return;
            }

            // Figure out string length
            int length = Encoding.UTF8.GetByteCount(value, index, count);

            // Ensure buffer large enough for marker, length, and value
            EnsureBufferLength(length + 6);

            // Figure out convert marker and length to encoded value
            byte lengthBeforeValue = ConvertStringLength(markerType, length);

            // Encode value
            Encoding.UTF8.GetBytes(value, index, count, _buffer, lengthBeforeValue);

            // Write marker, length, and value
            _stream.Write(_buffer, 0, lengthBeforeValue + length);
            BytesWritten += lengthBeforeValue + length;
        }

        private byte ConvertStringLength(BionMarker markerType, int length)
        {
            if (length <= MaxOneByteLength)
            {
                // Write as one byte (enum: Marker + 1)
                _buffer[0] = (byte)(markerType + 1);
                _buffer[1] = (byte)(length & 0x7F);
                return 2;
            }
            else if (length <= MaxTwoByteLength)
            {
                // Write as two bytes (enum: Marker + 2)
                _buffer[0] = (byte)(markerType + 2);
                _buffer[1] = (byte)(length & 0x7F);
                _buffer[2] = (byte)((length >> 7) & 0x7F);
                return 3;
            }
            else
            {
                // Write as five bytes (enum: Marker + 3)
                _buffer[0] = (byte)(markerType + 3);
                for (int index = 0; index < 5; ++index)
                {
                    _buffer[index + 1] = (byte)(length & 0x7F);
                    length = length >> 7;
                    index++;
                }
                return 6;
            }
        }

        private void WriteStartContainer(BionMarker container)
        {
            Write(container);
            _containers.Push(BytesWritten);
        }

        private void WriteEndContainer(BionMarker container)
        {
            Write(container);

            long end = BytesWritten;
            long start = _containers.Pop();
            long length = BytesWritten - start - 4;
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
            _stream.WriteByte(value);
            BytesWritten++;
        }

        private void EnsureBufferLength(int length)
        {
            if (_buffer.Length >= length) return;

            int newLength = _buffer.Length + _buffer.Length / 4;
            if (length > newLength) newLength = length;

            _buffer = new byte[newLength];
        }

        public void Dispose()
        {
            if(_stream != null)
            {
                _stream.Flush();

                if (CloseStream)
                {
                    _stream.Close();
                    _stream.Dispose();
                }

                _stream = null;
            }
        }
    }
}
