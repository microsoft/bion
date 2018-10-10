using Bion.Vector;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Bion
{
    public unsafe class BionReader : IDisposable
    {
        private Stream _stream;
        private Memory<byte> _buffer;

        private BionLookup _lookupDictionary;

        private BionMarker _currentMarker;
        private int _currentLength;
        private int _currentDepth;

        private short _lastPropertyLookupIndex;
        private string _currentDecodedString;

        private Encoding _textEncoding = Encoding.UTF8;

        // Lookups: Marker to Depth, Length, TokenType
        private static sbyte[] DepthLookup = Enumerable.Repeat((sbyte)0, 256).ToArray();
        private static sbyte[] LengthLookup = Enumerable.Repeat((sbyte)0, 256).ToArray();
        private static BionToken[] TokenLookup = Enumerable.Repeat(BionToken.None, 256).ToArray();

        static BionReader()
        {
            // Set DepthLookup for items with depth
            DepthLookup[(byte)BionMarker.StartObject] = 1;
            DepthLookup[(byte)BionMarker.StartArray] = 1;
            DepthLookup[(byte)BionMarker.EndObject] = -1;
            DepthLookup[(byte)BionMarker.EndArray] = -1;

            // Set LengtLookup for "length of length" or length of value
            LengthLookup[(byte)BionMarker.StringLength5b] = 5;
            LengthLookup[(byte)BionMarker.StringLength2b] = 2;
            LengthLookup[(byte)BionMarker.StringLength1b] = 1;
            LengthLookup[(byte)BionMarker.StringLookup1b] = -1;

            LengthLookup[(byte)BionMarker.PropertyNameLength5b] = 5;
            LengthLookup[(byte)BionMarker.PropertyNameLength2b] = 2;
            LengthLookup[(byte)BionMarker.PropertyNameLength1b] = 1;
            LengthLookup[(byte)BionMarker.PropertyNameLookup1b] = -1;
            LengthLookup[(byte)BionMarker.PropertyNameLookup2b] = -2;

            // Float/NegativeInt/Int
            for(int i = 0xB0; i < 0xE0; ++i)
            {
                LengthLookup[i] = (sbyte)(i & 15);
            }

            // Set TokenLookup
            for (int i = 0xF0; i <= 0xFF; ++i)
            {
                TokenLookup[i] = (BionToken)i;
            }

            Array.Fill(TokenLookup, BionToken.Float, 0xB0, 0xC0 - 0xB0);
            Array.Fill(TokenLookup, BionToken.Integer, 0xC0, 0xF0 - 0xC0);
            Array.Fill(TokenLookup, BionToken.PropertyName, 0xF3, 5);
            Array.Fill(TokenLookup, BionToken.String, 0xF8, 4);
        }

        public BionReader(Stream stream) : this(stream, null)
        { }

        public BionReader(Stream stream, BionLookup lookupDictionary)
        {
            CloseStream = true;
            _stream = stream;
            _lookupDictionary = lookupDictionary;
        }

        public long BytesRead { get; private set; }
        public bool CloseStream { get; set; }
        public BionToken TokenType { get; private set; }

        public bool Read()
        {
            // Check for end (after reading one thing at the root depth)
            if (_currentDepth == 0 && BytesRead > 0)
            {
                TokenType = BionToken.None;
                return false;
            }

            // Read the marker
            byte marker = ReadByte();
            BytesRead++;

            // Identify the token type and length
            _currentMarker = (BionMarker)marker;
            _currentLength = LengthLookup[marker];
            TokenType = TokenLookup[marker];

            if (_currentLength < 0)
            {
                // Negative lengths are lookup strings; look up the value
                LookupString();
            }
            else if (_currentLength > 0)
            {
                // Read value (non-string) or length (string)
                _buffer = Read(_currentLength);
                BytesRead += _currentLength;

                // Read value (string)
                if (marker >= 0xF0)
                {
                    _currentDecodedString = null;
                    _currentLength = (int)DecodeUnsignedInteger(_currentLength);
                    _buffer = Read(_currentLength);
                    BytesRead += _currentLength;
                }
            }
            else
            {
                // Adjust depth (length 0 tokens only)
                _currentDepth += DepthLookup[marker];
                _buffer = Memory<byte>.Empty;
            }

            return true;
        }

        public bool Read(BionToken expected)
        {
            bool result = Read();
            Expect(expected);
            return result;
        }

        public void Expect(BionToken expected)
        {
            if (this.TokenType != expected) throw new BionSyntaxException(this, expected);
        }

        public void Skip()
        {
            // Record depth
            int depth = _currentDepth;

            // Read one token
            Read();

            // If it wasn't a container, we're done
            if (depth == _currentDepth) return;

            // Otherwise, find the matching end container
            int innerDepth = 1;
            while (true)
            {
                Span<byte> buffer = Read(128 * 1024).Span;
                int endIndex = ByteVector.Skip(buffer, ref innerDepth);

                if (endIndex < buffer.Length)
                {
                    Return(buffer.Length - endIndex + 1);
                    BytesRead += endIndex;
                    return;
                }

                BytesRead += buffer.Length;
            }
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
                _currentDecodedString = _textEncoding.GetString(_buffer.Span);
            }

            return _currentDecodedString;
        }

        private void LookupString()
        {
            _currentLength = -_currentLength;
            _buffer = Read(_currentLength);
            BytesRead += _currentLength;

            short lookupIndex = (short)DecodeUnsignedInteger(_currentLength);
            if (_lookupDictionary == null) throw new BionSyntaxException($"@{BytesRead}: Found {TokenType} lookup for index {lookupIndex}, but no LookupDictionary was passed to the reader.");

            if (_currentMarker == BionMarker.StringLookup1b)
            {
                // A string value lookup can only appear right after a property name which is also indexed; look up the index from last time.
                _currentDecodedString = _lookupDictionary.Value(_lastPropertyLookupIndex, lookupIndex);
            }
            else
            {
                _currentDecodedString = _lookupDictionary.PropertyName(lookupIndex);
                _lastPropertyLookupIndex = lookupIndex;
            }
        }

        private ulong DecodeUnsignedInteger(int length)
        {
            ulong value = 0;

            Span<byte> span = _buffer.Span;
            for (int i = length - 1; i >= 0; --i)
            {
                value = value << 7;
                value += (ulong)(span[i] & 0x7F);
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

        #region Inlined Buffered Stream
        byte[] _innerBuffer = new byte[64 * 1024];
        int _innerIndex;
        int _innerLength;

        private byte ReadByte()
        {
            if (_innerIndex >= _innerLength) ReadNext(1);
            return _innerBuffer[_innerIndex++];
        }

        private Memory<byte> Read(int length)
        {
            if (_innerIndex + length > _innerLength) length = ReadNext(length);
            Memory<byte> result = new Memory<byte>(_innerBuffer, _innerIndex, length);
            _innerIndex += length;
            return result;
        }

        private int ReadNext(int size)
        {
            byte[] readInto = _innerBuffer;

            // Resize if needed
            if (size > _innerBuffer.Length)
            {
                readInto = new byte[Math.Max(_innerBuffer.Length * 5 / 4, size)];
            }

            // Copy unused bytes
            int lengthLeft = _innerLength - _innerIndex;
            if (lengthLeft > 0)
            {
                Buffer.BlockCopy(_innerBuffer, _innerIndex, readInto, 0, lengthLeft);
            }

            // Fill remaining buffer
            _innerLength = lengthLeft + _stream.Read(readInto, lengthLeft, readInto.Length - lengthLeft);

            // Reset variables
            _innerBuffer = readInto;
            _innerIndex = 0;

            // Return the safe size to read, if less than size
            return Math.Min(size, _innerLength);
        }

        private void Return(int length)
        {
            // Put 'length' bytes back into the buffer
            if (_innerIndex < length) throw new InvalidOperationException("Can't rewind data before current buffer.");
            _innerIndex -= length;
        }
        #endregion
    }
}
