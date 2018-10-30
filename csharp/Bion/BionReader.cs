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
        private WordCompressor _compressor;
        private BufferedReader _decompressReader;
        private BufferedWriter _decompressWriter;

        private BionMarker _currentMarker;
        private int _currentLength;
        private int _currentDepth;

        private ulong _currentDecodedNumber;
        private String8 _currentString;
        private string _currentDecodedString;
        private int _currentCompressedStringStart;

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
            LengthLookup[(byte)BionMarker.StringCompressedTerminated] = -1;

            LengthLookup[(byte)BionMarker.PropertyNameLength5b] = 5;
            LengthLookup[(byte)BionMarker.PropertyNameLength2b] = 2;
            LengthLookup[(byte)BionMarker.PropertyNameLength1b] = 1;
            LengthLookup[(byte)BionMarker.PropertyNameCompressedTerminated] = -1;

            // Float/NegativeInt/Int
            for (int i = 0xB0; i < 0xE0; ++i)
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
            Array.Fill(TokenLookup, BionToken.PropertyName, 0xF5, 3);
            Array.Fill(TokenLookup, BionToken.String, 0xF8, 3);
            TokenLookup[(byte)BionMarker.StringCompressedTerminated] = BionToken.String;
            TokenLookup[(byte)BionMarker.PropertyNameCompressedTerminated] = BionToken.PropertyName;
        }

        public BionReader(Stream stream, WordCompressor compressor = null) : this(new BufferedReader(stream), compressor)
        { }

        public BionReader(BufferedReader reader, WordCompressor compressor = null)
        {
            _reader = reader;
            _compressor = compressor;
        }

        public long BytesRead => _reader.BytesRead;
        public BionToken TokenType { get; private set; }
        public int Depth => _currentDepth;

        public bool Read()
        {
            _reader.EnsureSpace(20);

            // Check for end (after reading one thing at the root depth)
            if (_reader.EndOfStream)
            {
                TokenType = BionToken.None;
                return false;
            }

            // Read the marker
            byte marker = _reader.Buffer[_reader.Index++];

            // Identify the token type and length
            _currentMarker = (BionMarker)marker;
            _currentLength = LengthLookup[marker];
            TokenType = TokenLookup[marker];

            if (_currentLength < 0)
            {
                _currentLength = LengthIncludingTerminator();
                _currentDecodedString = null;
                _currentCompressedStringStart = _reader.Index;
                _reader.Index += _currentLength;
            }
            else if (_currentLength > 0)
            {
                // Read value (non-string) or length (string)
                _currentDecodedNumber = NumberConverter.ReadSevenBitExplicit(_reader, _currentLength);

                // Read value (string)
                if (marker >= 0xF0)
                {
                    _currentCompressedStringStart = -1;
                    _currentDecodedString = null;
                    _currentLength = (int)_currentDecodedNumber;

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
                int endIndex = ByteVector.Skip(_reader.Buffer, _reader.Index, _reader.Length, ref innerDepth);

                if (endIndex < _reader.Length)
                {
                    _reader.Index = endIndex + 1;
                    return;
                }

                _reader.Index = _reader.Length;
            }
        }

        public bool SeekToParent()
        {
            _reader.Index--;

            int batchSize = 64 * 1024;
            int targetDepth = _currentDepth - 1;
            long end = _reader.BytesRead;

            while(end > 0)
            {
                // Look for a StartObject or StartArray
                for(int i = _reader.Index; i >= 0; --i)
                {
                    _currentDepth -= DepthLookup[_reader.Buffer[i]];
                    if(_currentDepth == targetDepth)
                    {
                        _reader.Index = i;
                        return true;
                    }
                }

                // Record target is before this
                end = _reader.BytesRead - _reader.Index;

                // Read the previous batch of the file
                int nextSize = (int)Math.Min(end, batchSize);
                _reader.Seek(end - nextSize, SeekOrigin.Begin);
                _reader.EnsureSpace(nextSize, nextSize);
                _reader.Index = _reader.Length - 1;
            }

            _reader.Index = 0;
            return false;
        }

        public void Seek(long position)
        {
            // ISSUE: Don't know depth after seek
            _currentDepth = 0;

            // Move to before the container
            int marginBefore = (position < 1024 ? (int)position : 1024);
            long seekTo = position - marginBefore;

            _reader.Seek(seekTo, SeekOrigin.Begin);
            _reader.EnsureSpace(marginBefore + 4);
            _reader.Index = marginBefore;

            // Find start of value (0xF3 or 0xF4)
            for (int i = 0; i < 4; ++i)
            {
                if (_reader.Buffer[_reader.Index] > 0xF0) break;
                _reader.Index++;
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
                return SafeNegate(_currentDecodedNumber);
            }

            return (long)_currentDecodedNumber;
        }

        public unsafe double CurrentFloat()
        {
            if (TokenType != BionToken.Float) throw new BionSyntaxException($"@{BytesRead}: TokenType {TokenType} isn't a float type.");

            // Decode as an integer and coerce .NET into reinterpreting the bytes
            ulong value = _currentDecodedNumber;

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
                _currentDecodedString = CurrentString8().ToString();
            }

            return _currentDecodedString;
        }

        public String8 CurrentString8()
        {
            if (TokenType != BionToken.PropertyName && TokenType != BionToken.String) throw new BionSyntaxException($"@{BytesRead}: TokenType {TokenType} isn't a string type.");

            if (_currentCompressedStringStart != -1)
            {
                DecompressString();
                _currentCompressedStringStart = -1;
            }

            return _currentString;
        }

        public void RewriteOptimized(BufferedWriter writer, string indexPath = null)
        {
            int[] map = _compressor.OptimizeIndex();
            using (BufferedReader inner = BufferedReader.FromArray(_reader.Buffer, 0, 0))
            using (SearchIndexWriter indexWriter = (indexPath == null ? null : new SearchIndexWriter(indexPath, map.Length, 128 * 1024)))
            {
                long last = 0;

                while (this.Read())
                {
                    int length = (int)(this.BytesRead - last);
                    writer.EnsureSpace(length);

                    if (LengthLookup[(byte)_currentMarker] >= 0)
                    {
                        // Everything but compressed text: write bytes out
                        Buffer.BlockCopy(_reader.Buffer, _reader.Index - length, writer.Buffer, writer.Index, length);
                        writer.Index += length;
                    }
                    else
                    {
                        writer.Buffer[writer.Index++] = (byte)_currentMarker;

                        // Compressed Test: Rewrite the text segment
                        inner.ReadSlice(_reader.Buffer, _reader.Index - _currentLength, _reader.Index - 1);
                        _compressor.RewriteOptimized(map, inner, writer, indexWriter);

                        writer.Buffer[writer.Index++] = (byte)BionMarker.EndValue;
                    }

                    last = this.BytesRead;
                }

                if (indexWriter != null) { Console.WriteLine($"Index: {indexWriter.WordTotal:n0} total words, {indexWriter.NonDuplicateTotal:n0} non-duplicate."); }
            }
        }

        private int LengthIncludingTerminator()
        {
            int readSize = 16 * 1024;
            int lengthScanned = 0;

            int endIndex = -1;
            while (endIndex == -1)
            {
                // Read a block of bytes
                _reader.EnsureSpace(readSize);

                // Look for the EndValue marker (only *after* previously read bytes)
                endIndex = ByteVector.IndexOf((byte)BionMarker.EndValue, _reader.Buffer, _reader.Index + lengthScanned, _reader.Length);

                // Read more next time (without advancing index)
                lengthScanned = _reader.Length - _reader.Index;
                readSize *= 2;
            }

            // Return the number of characters to the terminator
            return (endIndex + 1) - _reader.Index;
        }

        private void DecompressString()
        {
            if (_decompressReader == null)
            {
                _decompressReader = BufferedReader.FromArray(_reader.Buffer, 0, 0);
                _decompressWriter = BufferedWriter.ToArray(new byte[1024]);
            }

            _decompressReader.ReadSlice(_reader.Buffer, _currentCompressedStringStart, _reader.Index - 1);
            _decompressWriter.Index = 0;

            // Decompress the content
            _compressor.Expand(_decompressReader, _decompressWriter);

            // Make a String8 referencing the full decompressed value
            _currentString = String8.Reference(_decompressWriter.Buffer, 0, _decompressWriter.Index);
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

            if (_compressor != null)
            {
                _compressor.Dispose();
                _compressor = null;
            }
        }
    }
}
