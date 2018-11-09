using System;

namespace Bion.IO
{
    public class NumberBlockWriter : IDisposable
    {
        private BufferedWriter _writer;

        private int[] _buffer;
        private int _bufferCount;

        public int BlockSize { get; private set; }

        public NumberBlockWriter(BufferedWriter writer, int blockSize)
        {
            if (blockSize % 4 != 0) { throw new ArgumentException("blockSize must be a multiple of 4."); }
            BlockSize = blockSize;

            _writer = writer;
            _buffer = new int[blockSize];
            _bufferCount = 0;
        }

        public void Write(int value)
        {
            _buffer[_bufferCount++] = value;
            if (_bufferCount == BlockSize) { WriteBlock(); }
        }

        private void WriteBlock()
        {
            // Ensure space for delta base, control bytes, delta base, and max width values
            _writer.EnsureSpace(4 + BlockSize / 4 + BlockSize * 4);

            //// Find minimum for block (ideally, base might sometimes be best as a bigger value)
            //int deltaBase = _buffer[0];
            //for (int i = 1; i < BlockSize; ++i)
            //{
            //    if (_buffer[i] < deltaBase) { deltaBase = _buffer[i]; }
            //}

            //_writer.Write(deltaBase);

            // Skip control bytes (for now)
            long controlStart = _writer.Index;
            _writer.Index += BlockSize / 4;

            int controlByte = 0;

            for (int i = 0; i < BlockSize; ++i)
            {
                int value = _buffer[i];// - deltaBase;
                int length = ByteLength(value);

                // Write value
                for (int j = length - 1; j >= 0; --j)
                {
                    _writer.Buffer[_writer.Index + j] = (byte)(value & 0xFF);
                    value = value >> 8;
                }

                _writer.Index += length;

                // Accumulate control byte
                controlByte = (controlByte << 2) + (length - 1);

                // Write control byte (every 4th value)
                if (i % 4 == 3)
                {
                    _writer.Buffer[controlStart + i / 4] = (byte)controlByte;
                    controlByte = 0;
                }
            }

            // Reset
            _bufferCount = 0;
        }

        private int ByteLength(int value)
        {
            int length = 0;

            do
            {
                length++;
                value = value >> 8;
            } while (value != 0);

            return length;
        }

        public void Dispose()
        {
            _writer?.Dispose();
            _writer = null;
        }
    }

    public class NumberBlockReader : IDisposable
    {
        private BufferedReader _reader;
        private int[] _buffer;

        public int BlockSize { get; private set; }

        public NumberBlockReader(BufferedReader reader, int blockSize)
        {
            if (blockSize % 4 != 0) { throw new ArgumentException("blockSize must be a multiple of 4."); }
            BlockSize = blockSize;

            _reader = reader;
            _buffer = new int[blockSize];
        }

        public bool ReadBlock(out int[] block)
        {
            if (_reader.EndOfStream)
            {
                block = null;
                return false;
            }

            _reader.EnsureSpace(4 + BlockSize / 4 + BlockSize * 4);

            // Read delta
            //int deltaBase = _reader.ReadInt32();

            // Skip over control bytes
            int controlStart = _reader.Index;
            _reader.Index += (BlockSize / 4);

            int controlByte = 0;

            for (int i = 0; i < BlockSize; ++i)
            {
                if (i % 4 == 0)
                {
                    controlByte = _reader.Buffer[controlStart + i / 4];
                }

                // Read control byte
                int byteLength = ((controlByte & 0xC0) >> 6) + 1;
                controlByte = controlByte << 2;

                // Read value bytes
                int value = 0;
                for (int j = 0; j < byteLength; ++j)
                {
                    value = value << 8;
                    value += _reader.Buffer[_reader.Index++];
                }

                //// Add delta
                //value += deltaBase;

                _buffer[i] = value;
            }

            block = _buffer;
            return true;
        }

        public void Dispose()
        {
            _reader?.Dispose();
            _reader = null;
        }
    }
}
