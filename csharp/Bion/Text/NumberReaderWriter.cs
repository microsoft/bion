using System;
using System.IO;

namespace Bion.Text
{
    public class NumberWriter : IDisposable
    {
        private Stream _stream;
        private byte[] _buffer;
        private int _index;
        public long BytesWritten { get; private set; }

        public NumberWriter(Stream stream)
        {
            _stream = stream;
            _buffer = new byte[16 * 1024];
        }

        public void WriteValue(ulong value)
        {
            if (_index + 10 >= _buffer.Length) { Flush(); }

            int indexBefore = _index;
            while(value > 0x7F)
            {
                _buffer[_index++] = (byte)(value & 0x7F);
                value = value >> 7;
            }

            _buffer[_index++] = (byte)(value | 0x80);
            BytesWritten += _index - indexBefore;
        }

        private void Flush()
        {
            if (_index > 0)
            {
                _stream.Write(_buffer, 0, _index);
                _index = 0;
            }
        }

        public void Dispose()
        {
            if(_stream != null)
            {
                Flush();
                _stream.Dispose();
                _stream = null;
            }
        }
    }

    public class NumberReader : IDisposable
    {
        private Stream _stream;
        private byte[] _buffer;
        private int _lastIndex;
        private int _index;
        private int _length;
        public long BytesRead { get; private set; }

        public NumberReader(Stream stream)
        {
            _stream = stream;
            _buffer = new byte[16 * 1024];

            // Indicate nothing read yet by making index > length.
            _index = _buffer.Length;
            _length = _buffer.Length;
        }

        public bool EndOfStream => _length < _buffer.Length && _index == _length;

        public ulong ReadNumber()
        {
            if (_index + 10 >= _length) { Read(); }

            _lastIndex = _index;
            ulong value = 0;
            int shift = 0;

            while(true)
            {
                byte next = _buffer[_index++];
                value += (ulong)(next & 0x7F) << shift;
                shift += 7;
                if (next >= 0x80) break;
            }

            BytesRead += _index - _lastIndex;
            return value;
        }

        public void UndoRead()
        {
            BytesRead -= _index - _lastIndex;
            _index = _lastIndex;
        }

        private void Read()
        {
            // If we didn't get a full block, we're out of stream
            if (_length < _buffer.Length) { return; }

            // Shift any unread suffix
            int bytesLeft = _length - _index;
            if (bytesLeft > 0) { Buffer.BlockCopy(_buffer, _index, _buffer, 0, bytesLeft); }

            // Refill the block and set the new length and index
            _length = bytesLeft + _stream.Read(_buffer, bytesLeft, _buffer.Length - bytesLeft);
            _index = 0;
        }

        public void Dispose()
        {
            if(_stream != null)
            {
                _stream.Dispose();
                _stream = null;
            }
        }
    }
}
