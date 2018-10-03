using System;
using System.IO;
using System.Text;

namespace Bion
{
    public unsafe class BionReader : IDisposable
    {
        private Stream _stream;
        private long _totalLengthBytes;

        // String and Property Name tokens, in order, are Len5b, Len2b, Len1b, Look1b, Look2b.
        private static sbyte[] LengthLookup = new sbyte[] { 5, 2, 1, -1, -2 };

        public BionReader(Stream stream)
        {
            _stream = new BufferedStream(stream);
            _buffer = new byte[1024];
            _totalLengthBytes = stream.Length - stream.Position;
        }

        public long BytesRead { get; private set; }
        public bool CloseStream { get; set; }

        public BionToken TokenType { get; private set; }

        private BionMarker _currentMarker;
        private int _currentLength;
        private byte[] _buffer;

        public bool Read()
        {
            if (BytesRead >= _totalLengthBytes) return false;

            // Read the current token marker
            _currentMarker = (BionMarker)_stream.ReadByte();
            _currentLength = 0;
            BytesRead++;

            if(_currentMarker >= BionMarker.EndArray)
            {
                // Container. Token is all marker bits. LoL and Length zero.
                TokenType = (BionToken)_currentMarker;
            }
            else if(_currentMarker >= BionMarker.String)
            {
                // String
                TokenType = BionToken.String;
                _currentLength = ReadStringLength();
            }
            else if(_currentMarker >= BionMarker.PropertyName)
            {
                // Property Name
                TokenType = BionToken.PropertyName;
                _currentLength = ReadStringLength();
            }
            else if(_currentMarker >= BionMarker.False)
            {
                // Literal
                TokenType = (BionToken)_currentMarker;
            }
            else if(_currentMarker >= BionMarker.InlineInteger)
            {
                // Inline Int
                TokenType = BionToken.Integer;
            }
            else if(_currentMarker >= BionMarker.Float)
            {
                // Integer | NegativeInteger | Float
                TokenType = (_currentMarker >= BionMarker.NegativeInteger ? BionToken.Integer : BionToken.Float);

                // Length is last four bits
                _currentLength = (int)_currentMarker & 0x0F;
            }
            else
            {
                throw new InvalidDataException($"Byte 0x{_currentMarker:X} at offset {BytesRead:n0} is not a valid BION marker.");
            }

            // Read value
            if(_currentLength > 0)
            {
                _stream.Read(_buffer, 0, _currentLength);
                BytesRead += _currentLength;
            }

            return true;
        }

        private int ReadStringLength()
        {
            sbyte lengthOfLength = LengthLookup[(byte)TokenType - (byte)_currentMarker];

            if(lengthOfLength < 0)
            {
                // Lookup
                throw new NotImplementedException("Lookups not implemented.");
            }

            _stream.Read(_buffer, 0, lengthOfLength);
            BytesRead += lengthOfLength;
            return (int)DecodeUnsignedInteger(lengthOfLength);
        }

        public bool CurrentBool()
        {
            if (TokenType == BionToken.True) return true;
            if (TokenType == BionToken.False) return false;
            throw new InvalidCastException($"@{BytesRead}: TokenType {TokenType} isn't a boolean type.");
        }

        public long CurrentInteger()
        {
            if (TokenType != BionToken.Integer) throw new InvalidCastException($"@{BytesRead}: TokenType {TokenType} isn't an integer type.");

            // Inline Integer
            if (_currentLength == 0) return ((int)_currentMarker & 0x0F);

            // Decode 7-bit value
            ulong value = DecodeUnsignedInteger(_currentLength);

            // Negate if type was NegativeInteger
            if (_currentMarker < BionMarker.Integer)
            {
                return SafeNegate(value);
            }

            return (long)value;
        }

        public unsafe double CurrentFloat()
        {
            if (TokenType != BionToken.Float) throw new InvalidCastException($"@{BytesRead}: TokenType {TokenType} isn't a float type.");

            // Decode as an integer and coerce .NET into reinterpreting the bytes
            ulong value = DecodeUnsignedInteger(_currentLength);
            return *(double*)&value;
        }

        public string CurrentString()
        {
            if (TokenType != BionToken.PropertyName && TokenType != BionToken.String) throw new InvalidCastException($"@{BytesRead}: TokenType {TokenType} isn't a string type.");
            return Encoding.UTF8.GetString(_buffer, 0, _currentLength);
        }

        private ulong DecodeUnsignedInteger(int length)
        {
            ulong value = 0;

            for (int i = length - 1; i >= 0; --i)
            {
                value = value << 7;
                value += (ulong)(_buffer[i] & 0x7F);
            }

            return value;
        }

        private long SafeNegate(ulong value)
        {
            // Decrement to ensure in range, then cast
            long inRange = (long)(value - 1);

            // Negate and undo the decrement
            return -inRange - 1;
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                if (CloseStream)
                {
                    _stream.Dispose();
                }

                _stream = null;
            }
        }
    }
}
