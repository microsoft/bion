using System;
using System.IO;
using System.Text;

namespace Bion
{
    public unsafe class BionReader : IDisposable
    {
        private Stream _stream;
        private byte[] _buffer;

        private LookupDictionary _lookupDictionary;

        private BionMarker _currentMarker;
        private int _currentLength;
        private int _currentDepth;

        private short _lastPropertyLookupIndex;
        private string _currentDecodedString;

        // String and Property Name tokens, in order, are Len5b, Len2b, Len1b, Look1b, Look2b.
        private static sbyte[] LengthLookup = new sbyte[] { 5, 2, 1, -1, -2 };

        public BionReader(Stream stream) : this(stream, null)
        { }

        public BionReader(Stream stream, LookupDictionary lookupDictionary)
        {
            CloseStream = true;
            _stream = new BufferedStream(stream);
            _buffer = new byte[1024];

            _lookupDictionary = lookupDictionary;
        }

        public long BytesRead { get; private set; }
        public bool CloseStream { get; set; }
        public BionToken TokenType { get; private set; }

        public bool Read()
        {
            // Check for end (after reading one thing at the root depth)
            if (_currentDepth == 0 && BytesRead > 0) return false;

            // Read the current token marker
            _currentMarker = (BionMarker)_stream.ReadByte();
            _currentLength = 0;
            _currentDecodedString = null;
            BytesRead++;

            if(_currentMarker >= BionMarker.EndArray)
            {
                // Container. Token is all marker bits. LoL and Length zero.
                TokenType = (BionToken)_currentMarker;

                // Increment or Decrement depth
                _currentDepth += (_currentMarker >= BionMarker.StartArray ? 1 : -1);
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
                throw new BionSyntaxException($"@{BytesRead:n0}: Byte 0x{_currentMarker:X} is not a valid BION marker.");
            }

            // Read value
            if(_currentLength > 0)
            {
                Allocator.EnsureBufferLength(ref _buffer, _currentLength);
                _stream.Read(_buffer, 0, _currentLength);
                BytesRead += _currentLength;
            }

            return true;
        }

        public void Expect(BionToken expected)
        {
            if (this.TokenType != expected) throw new BionSyntaxException(this, expected);
        }

        public bool CurrentBool()
        {
            if (TokenType == BionToken.True) return true;
            if (TokenType == BionToken.False) return false;
            throw new InvalidCastException($"@{BytesRead}: TokenType {TokenType} isn't a boolean type.");
        }

        public long CurrentInteger()
        {
            if (TokenType != BionToken.Integer) throw new BionSyntaxException($"@{BytesRead}: TokenType {TokenType} isn't an integer type.");

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
            if (TokenType != BionToken.Float) throw new BionSyntaxException($"@{BytesRead}: TokenType {TokenType} isn't a float type.");

            // Decode as an integer and coerce .NET into reinterpreting the bytes
            ulong value = DecodeUnsignedInteger(_currentLength);

            if (_currentLength <= 5)
            {
                uint asInt = (uint)value;
                return (double)*(float*)&asInt;
            }
            else
            {
                
                return *(double*)&value;
            }
        }

        public string CurrentString()
        {
            if (TokenType == BionToken.Null) return null;
            if (TokenType != BionToken.PropertyName && TokenType != BionToken.String) throw new BionSyntaxException($"@{BytesRead}: TokenType {TokenType} isn't a string type.");

            if (_currentDecodedString == null)
            {
                _currentDecodedString = Encoding.UTF8.GetString(_buffer, 0, _currentLength);
            }

            return _currentDecodedString;
        }

        private int ReadStringLength()
        {
            sbyte lengthOfLength = LengthLookup[(byte)TokenType - (byte)_currentMarker];

            if (lengthOfLength < 0)
            {
                lengthOfLength = (sbyte)-lengthOfLength;
                _stream.Read(_buffer, 0, lengthOfLength);
                BytesRead += lengthOfLength;

                short lookupIndex = (short)DecodeUnsignedInteger(lengthOfLength);
                if (_lookupDictionary == null) throw new BionSyntaxException($"@{BytesRead}: Found {TokenType} lookup for index {lookupIndex}, but no LookupDictionary was passed to the reader.");

                if (TokenType == BionToken.PropertyName)
                {
                    _currentDecodedString = _lookupDictionary.PropertyName(lookupIndex);
                    _lastPropertyLookupIndex = lookupIndex;
                }
                else
                {
                    // A string value lookup can only appear right after a property name which is also indexed; look up the index from last time.
                    _currentDecodedString = _lookupDictionary.Value(_lastPropertyLookupIndex, lookupIndex);
                }

                return 0;
            }
            else
            {
                _stream.Read(_buffer, 0, lengthOfLength);
                BytesRead += lengthOfLength;
                return (int)DecodeUnsignedInteger(lengthOfLength);
            }
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
                if (CloseStream) _stream.Dispose();
                _stream = null;
            }

            if(_lookupDictionary != null)
            {
                _lookupDictionary.Dispose();
                _lookupDictionary = null;
            }
        }
    }
}
