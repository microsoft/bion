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

        private BionLookup _lookupDictionary;

        private string _lastPropertyName;
        private long _lastPropertyPosition;

        public BionWriter(Stream stream) : this(stream, null)
        { }

        public BionWriter(Stream stream, BionLookup lookupDictionary)
        {
            CloseStream = true;

            _stream = new BufferedStream(stream);
            _containers = new Stack<long>();
            _buffer = new byte[1024];

            _lookupDictionary = lookupDictionary;
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

        public void WritePropertyName(string name)
        {
            WriteStringValue(BionToken.PropertyName, name);

            _lastPropertyName = name;
            _lastPropertyPosition = BytesWritten;
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

            ConvertFixedInteger((ulong)stringLength, lengthOfLength);
            _buffer[0] = (byte)((int)marker + markerAdjustment);

            _stream.Write(_buffer, 0, lengthOfLength + 1);
            BytesWritten += lengthOfLength + 1;
        }

        private void WriteStringValue(BionToken markerType, string value)
        {
            if (value == null)
            {
                WriteNull();
                return;
            }

            if (_lookupDictionary != null)
            {
                if (markerType == BionToken.PropertyName)
                {
                    if (_lookupDictionary.TryLookup(value, out short index))
                    {
                        WriteLookupIndex(BionMarker.PropertyNameLookup1b, index);
                        return;
                    }
                }
                else if(_lastPropertyPosition == BytesWritten)
                {
                    if(_lookupDictionary.TryLookup(_lastPropertyName, value, out short index))
                    {
                        WriteLookupIndex(BionMarker.StringLookup1b, index);
                        return;
                    }
                }
            }

            int length = Encoding.UTF8.GetByteCount(value);

            // Write marker and length
            WriteStringLength(markerType, length);

            // Encode and write value
            Allocator.EnsureBufferLength(ref _buffer, length);
            Encoding.UTF8.GetBytes(value, _buffer);

            _stream.Write(_buffer, 0, length);
            BytesWritten += length;
        }

        private void WriteLookupIndex(BionMarker oneByteLookupMarker, short index)
        {
            byte length = ConvertVariableInteger((ushort)index);
            if (length == 2) oneByteLookupMarker -= 1;
            _buffer[0] = (byte)oneByteLookupMarker;

            _stream.Write(_buffer, 0, length + 1);
            BytesWritten += length + 1;
        }

        private void WriteVariableInteger(BionMarker marker, ulong value)
        {
            byte length = ConvertVariableInteger(value);
            _buffer[0] = (byte)(marker + length);

            _stream.Write(_buffer, 0, length + 1);
            BytesWritten += length + 1;
        }

        private void WriteFixedInteger(BionMarker marker, ulong value, byte length)
        {
            ConvertFixedInteger(value, length);
            _buffer[0] = (byte)(marker + length);

            _stream.Write(_buffer, 0, length + 1);
            BytesWritten += length + 1;
        }

        /// <summary>
        ///  Convert a non-negative integer to a fixed number of bytes
        ///  starting in buffer index one.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="length">Number of bytes to write to</param>
        private void ConvertFixedInteger(ulong value, int length)
        {
            for (int i = 1; i <= length; ++i)
            {
                _buffer[i] = (byte)(value & 0x7F);
                value = value >> 7;
            }
        }

        /// <summary>
        ///  Convert a non-negative integer to a variable number of bytes
        ///  starting in buffer index one. Return the number of bytes needed.
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>Byte length of converted value</returns>
        private byte ConvertVariableInteger(ulong value)
        {
            byte length = 1;
            do
            {
                _buffer[length] = (byte)(value & 0x7F);
                value = value >> 7;
                length++;
            } while (value > 0);

            return --length;
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
            if (_stream != null)
            {
                _stream.Flush();
                if (CloseStream) _stream.Dispose();
                _stream = null;
            }

            if (_lookupDictionary != null)
            {
                _lookupDictionary.Dispose();
                _lookupDictionary = null;
            }
        }
    }
}
