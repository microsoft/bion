using Bion.Extensions;
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

        public void Flush()
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
        private bool _endOfStream;
        private byte[] _buffer;
        private int _lastIndex;
        private int _index;
        private int _length;
        
        public NumberReader(Stream stream)
        {
            _stream = stream;
            _buffer = new byte[16 * 1024];
        }

        public long BytesRead { get; private set; }
        public bool EndOfStream => _endOfStream && _index == _length;

        public ulong ReadNumber()
        {
            if (_index + 10 >= _length)
            {
                _stream.Refill(ref _index, ref _length, ref _endOfStream, ref _buffer);
            }

            _lastIndex = _index;
            ulong value = 0;
            int current = 0, shift = 0;

            while(current < 0x80)
            {
                current = _buffer[_index++];
                value += (ulong)(current & 0x7F) << shift;
                shift += 7;
            }

            BytesRead += _index - _lastIndex;
            return value;
        }

        public void UndoRead()
        {
            BytesRead -= _index - _lastIndex;
            _index = _lastIndex;
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
