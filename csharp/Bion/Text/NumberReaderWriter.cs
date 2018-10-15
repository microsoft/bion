using Bion.Extensions;
using System;
using System.IO;

namespace Bion.Text
{
    public class VariableNumberWriter : IDisposable
    {
        internal const byte BitsPerByte = 6;
        internal const byte FirstByteMarker = 1 << BitsPerByte;
        internal const byte PerByteCutoff = (1 << BitsPerByte) - 1;

        private Stream _stream;
        private byte[] _buffer;
        private int _index;
        public long BytesWritten { get; private set; }

        public VariableNumberWriter(Stream stream)
        {
            _stream = stream;
            _buffer = new byte[16 * 1024];
        }

        public void WriteValue(ulong value)
        {
            if (_index + 10 >= _buffer.Length) { Flush(); }

            int indexBefore = _index;
            while(value > PerByteCutoff)
            {
                _buffer[_index++] = (byte)(value & PerByteCutoff);
                value = value >> BitsPerByte;
            }

            _buffer[_index++] = (byte)(value | FirstByteMarker);
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

    public class VariableNumberReader : IDisposable
    {
        private Stream _stream;
        private bool _endOfStream;
        private byte[] _buffer;
        private int _lastIndex;
        private int _index;
        private int _length;
        
        public VariableNumberReader(Stream stream)
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

            while(current <= VariableNumberWriter.PerByteCutoff)
            {
                current = _buffer[_index++];
                value += (ulong)(current & VariableNumberWriter.PerByteCutoff) << shift;
                shift += VariableNumberWriter.BitsPerByte;
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
