using Bion.IO;
using Bion.Text;
using Bion.Vector;
using System;
using System.IO;
using System.Linq;

namespace Bion
{
    public unsafe class BionReader : IDisposable
    {
        private BufferedReader _reader;

        private BionMarker _currentMarker;
        private int _currentLength;
        private int _currentDepth;

        private ulong _currentDecoded;
        private String8 _currentString;
        private string _currentDecodedString;

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

        public BionReader(Stream stream): this(new BufferedReader(stream))
        { }

        public BionReader(BufferedReader reader)
        {
            _reader = reader;
        }

        public long BytesRead => _reader.BytesRead;
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
            _reader.EnsureSpace(20);
            byte marker = _reader.Buffer[_reader.Index++];

            // Identify the token type and length
            _currentMarker = (BionMarker)marker;
            _currentLength = LengthLookup[marker];
            TokenType = TokenLookup[marker];

            if (_currentLength > 0)
            {
                // Read value (non-string) or length (string)
                _currentDecoded = NumberConverter.ReadSevenBitExplicit(_reader, _currentLength);

                // Read value (string)
                if (marker >= 0xF0)
                {
                    _currentDecodedString = null;
                    _currentLength = (int)_currentDecoded;

                    _reader.EnsureSpace(_currentLength);
                    _currentString = String8.Reference(_reader.Buffer, _reader.Index, _currentLength);
                    _reader.Index += _currentLength;
                }
            }
            else
            {
                // Adjust depth (length 0 tokens only)
                _currentDepth += DepthLookup[marker];
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
            while (!_reader.EndOfStream)
            {
                _reader.EnsureSpace(128 * 1024);
                Span<byte> buffer = _reader.Buffer.AsSpan(_reader.Index, _reader.Length - _reader.Index);
                int endIndex = ByteVector.Skip(buffer, ref innerDepth);

                if (endIndex < buffer.Length)
                {
                    _reader.Index = endIndex + 1;
                    return;
                }

                _reader.Index = _reader.Length;
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

            // Negate if type was NegativeInteger
            if (_currentMarker < BionMarker.Integer)
            {
                return SafeNegate(_currentDecoded);
            }

            return (long)_currentDecoded;
        }

        public unsafe double CurrentFloat()
        {
            if (TokenType != BionToken.Float) throw new BionSyntaxException($"@{BytesRead}: TokenType {TokenType} isn't a float type.");

            // Decode as an integer and coerce .NET into reinterpreting the bytes
            ulong value = _currentDecoded;

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
                _currentDecodedString = _currentString.ToString();
            }

            return _currentDecodedString;
        }

        public String8 CurrentString8()
        {
            if (TokenType != BionToken.PropertyName && TokenType != BionToken.String) throw new BionSyntaxException($"@{BytesRead}: TokenType {TokenType} isn't a string type.");
            return _currentString;
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
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
        }
    }
}
