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
            WriteStringValue(BionToken.String, value, 0, value?.Length ?? 0);
        }

        public void WriteValue(string value, int index, int count)
        {
            WriteStringValue(BionToken.String, value, index, count);
        }

        public void WritePropertyName(string name)
        {
            WriteStringValue(BionToken.PropertyName, name, 0, name?.Length ?? 0);
        }

        public void WritePropertyName(string name, int index, int count)
        {
            WriteStringValue(BionToken.PropertyName, name, index, count);
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

            for(int index = 1; index <= lengthOfLength; ++index)
            {
                _buffer[index] = (byte)(stringLength & 0x7F);
                stringLength = stringLength >> 7;
            }

            _buffer[0] = (byte)((int)marker + markerAdjustment);

            _stream.Write(_buffer, 0, lengthOfLength + 1);
            BytesWritten += lengthOfLength + 1;
        }

        private void WriteStringValue(BionToken markerType, string value, int index, int count)
        {
            if (value == null)
            {
                WriteNull();
                return;
            }

            int length = Encoding.UTF8.GetByteCount(value, index, count);
            
            // Write marker and length
            WriteStringLength(markerType, length);

            // Encode and write value
            Allocator.EnsureBufferLength(ref _buffer, length);
            Encoding.UTF8.GetBytes(value, index, count, _buffer, 0);

            _stream.Write(_buffer, 0, length);
            BytesWritten += length;
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
