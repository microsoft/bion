using System;
using System.Collections.Generic;
using System.IO;
using Bion.Vector;

namespace Bion.Json.Splitter
{
    /// <summary>
    ///  JsonTokenType is the type of the next token found.
    /// </summary>
    /// <remarks>
    ///  Illegal must be zero to allow uninitialized values in the map to be illegal.
    ///  All valid separators must be at the end, to allow quickly identifying separators.
    ///  Whitespace and EndOfFile must be just before separators to allow quickly identifying (WhitespaceOrSeparator).
    /// </remarks>
    public enum JsonTokenType : byte
    {
        Illegal = 0,
        Comment,
        Null,
        False,
        True,
        Number,
        String,
        Whitespace,
        EndOfFile,
        StartObject,
        EndObject,
        StartArray,
        EndArray,
        NameSeparator,
        ValueSeparator
    }

    public struct JsonToken
    {
        public long WhitespaceStartIndex;
        public long ValueStartIndex;
        public long ValueEndIndex;
        public JsonTokenType TokenType;

        public long WhitespaceLength => ValueStartIndex - WhitespaceStartIndex;
        public long ValueLength => 1 + ValueEndIndex - ValueStartIndex;

        public JsonToken(long whitespaceStart, long valueStart, long valueEnd, JsonTokenType tokenType)
        {
            this.WhitespaceStartIndex = whitespaceStart;
            this.ValueStartIndex = valueStart;
            this.ValueEndIndex = valueEnd;
            this.TokenType = tokenType;
        }
    }

    public class BufferedReader2
    {
        private Stream _source;
        private bool _sourceAtEnd;

        private byte[] _buffer;
        private long _bytesBeforeBuffer;
        private long _bytesRead;
        private long _bytesConsumed;

        public long Position;

        public BufferedReader2(Stream source, int bufferSize = 64 * 1024)
        {
            _source = source;
            _buffer = new byte[bufferSize];
        }

        public BufferedReader2(byte[] source, int index, int length)
        {
            _sourceAtEnd = true;
            _buffer = source;
            _bytesBeforeBuffer = -index;
            _bytesRead = length;
            _bytesConsumed = 0;
            Position = 0;
        }

        public bool End => Position >= _bytesRead && !Read();

        private bool Read()
        {
            if (_sourceAtEnd) { return false; }

            byte[] toFill = _buffer;

            // Count bytes not yet consumed in the buffer
            long bytesToSave = _bytesRead - _bytesConsumed;
            long bytesToSaveIndex = _bytesConsumed - _bytesBeforeBuffer;

            // If the buffer is too small, grow it
            if (_buffer.Length < 2 * bytesToSave)
            {
                toFill = new byte[2 * _buffer.Length];
            }

            // Save the not consumed bytes at the beginning of the buffer
            if (bytesToSave > 0)
            {
                Buffer.BlockCopy(_buffer, (int)bytesToSaveIndex, toFill, 0, (int)bytesToSave);
            }

            // Update where the buffer begins
            _bytesBeforeBuffer += bytesToSave;

            // Fill the remaining buffer
            int bufferBytesAvailable = (int)(toFill.Length - bytesToSave);
            int newBytesRead = _source.Read(toFill, (int)bytesToSave, bufferBytesAvailable);

            // Update whether we're out of bytes
            _sourceAtEnd = (newBytesRead < bufferBytesAvailable);

            // Update how much we've read in total
            _bytesRead += newBytesRead;

            return true;
        }

        public void ConsumeTo(long position)
        {
            _bytesConsumed = position;
        }

        public byte Current => _buffer[Position - _bytesBeforeBuffer];
    }

    public class JsonSplitter
    {
        private const byte FirstWhitespaceOrSeparator = (byte)JsonTokenType.Whitespace;
        private const int DecodeBatchSize = 128;
        private static JsonTokenType[] _map;

        private BufferedReader2 _reader;

        public JsonToken Current { get; private set; }
        private Queue<JsonToken> _decoded;

        public JsonSplitter(BufferedReader2 reader)
        {
            InitializeMap();
            _reader = reader;
            _decoded = new Queue<JsonToken>(DecodeBatchSize);
        }

        /// <summary>
        ///  Build a map of all UTF8 bytes which may appear in JSON outside strings.
        /// </summary>
        private static void InitializeMap()
        {
            if (_map != null) { return; }

            JsonTokenType[] map = new JsonTokenType[256];

            map[(byte)' '] = JsonTokenType.Whitespace;
            map[(byte)'\t'] = JsonTokenType.Whitespace;
            map[(byte)'\r'] = JsonTokenType.Whitespace;
            map[(byte)'\n'] = JsonTokenType.Whitespace;

            map[(byte)'n'] = JsonTokenType.Null;
            map[(byte)'f'] = JsonTokenType.False;
            map[(byte)'t'] = JsonTokenType.True;

            map[(byte)'-'] = JsonTokenType.Number;
            for (char i = '0'; i <= '9'; ++i)
            {
                map[(byte)i] = JsonTokenType.Number;
            }

            map[(byte)'"'] = JsonTokenType.String;
            map[(byte)'{'] = JsonTokenType.StartObject;
            map[(byte)'}'] = JsonTokenType.EndObject;
            map[(byte)'['] = JsonTokenType.StartArray;
            map[(byte)']'] = JsonTokenType.EndArray;
            map[(byte)':'] = JsonTokenType.NameSeparator;
            map[(byte)','] = JsonTokenType.ValueSeparator;

            _map = map;
        }

        public bool Next()
        {
            if (_decoded.Count == 0)
            {
                Split();
                if (_decoded.Count == 0) { return false; }
            }

            Current = _decoded.Dequeue();
            return true;
        }

        private void Split()
        {
            // Mark all previous tokens consumed
            _reader.ConsumeTo(_reader.Position);

            while (!_reader.End && _decoded.Count < DecodeBatchSize)
            {
                JsonToken token;
                token.TokenType = JsonTokenType.EndOfFile;

                // Mark token start
                token.WhitespaceStartIndex = _reader.Position;

                // Read any whitespace
                while (!_reader.End)
                {
                    token.TokenType = _map[_reader.Current];
                    if (token.TokenType != JsonTokenType.Whitespace) { break; }
                    _reader.Position++;
                }

                // Mark value start
                token.ValueStartIndex = _reader.Position;

                if (token.TokenType == JsonTokenType.String)
                {
                    // If string, read until end quote
                    ReadString();
                }
                else
                {
                    // Read until whitespace or separator (end of anything else)
                    while (!_reader.End)
                    {
                        JsonTokenType type = _map[_reader.Current];
                        if ((byte)type >= FirstWhitespaceOrSeparator) { break; }
                        _reader.Position++;
                    }
                }

                // Mark value end
                token.ValueEndIndex = _reader.Position - 1;

                _decoded.Enqueue(token);
            }
        }

        private void ReadString()
        {
            // Consume opening quote
            _reader.Position++;

            long start = _reader.Position;

            // Find the end of the string
            while (!_reader.End)
            {
                // Find the next quote
                if (_reader.Current == (byte)'"')
                {
                    long end = _reader.Position;

                    // Count backslashes before end quote
                    _reader.Position--;
                    while (_reader.Position > start && _reader.Current == (byte)'\\')
                    {
                        _reader.Position--;
                    }

                    int backslashCount = (int)((end - _reader.Position) - 1);

                    // Reset current position
                    _reader.Position = end;

                    // If count was even, quote was end of string
                    if ((backslashCount & 1) == 0) { break; }
                }

                _reader.Position++;
            }

            // Consume closing quote
            _reader.Position++;
        }
    }
}
