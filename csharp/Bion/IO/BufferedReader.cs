using Bion.Extensions;
using System;
using System.IO;

namespace Bion.IO
{
    /// <summary>
    ///  BufferedReader reads from a stream via a resizable buffer which is
    ///  exposed directly to consumers for performance.
    /// </summary>
    public class BufferedReader : IDisposable
    {
        private Stream _stream;
        private bool _streamDone;
        private long _bytesRead;

        /// <summary>
        ///  Current byte buffer.
        ///  Read from Buffer[Index] and increment Index for content Read.
        /// </summary>
        public byte[] Buffer;

        /// <summary>
        ///  Index of next byte in Buffer to read.
        /// </summary>
        public int Index;

        /// <summary>
        ///  Length in bytes of valid range in buffer.
        /// </summary>
        public int Length;

        /// <summary>
        ///  Whether to close the stream in Dispose.
        /// </summary>
        public bool CloseStream;

        /// <summary>
        ///  Construct a BufferedReader on the desired stream with the
        ///  requested buffer size.
        /// </summary>
        /// <param name="stream">Stream to read</param>
        /// <param name="size">Initial buffer size</param>
        public BufferedReader(Stream stream, int size = 16 * 1024) : this(stream, new byte[size])
        { }

        /// <summary>
        ///  Construct a BufferedReader on the desired stream with the
        ///  provided buffer.
        /// </summary>
        /// <param name="stream">Stream to read</param>
        /// <param name="buffer">Initial buffer to use</param>
        public BufferedReader(Stream stream, byte[] buffer)
        {
            _stream = stream;
            Buffer = buffer;
            CloseStream = true;
            EnsureSpace(1);
        }

        /// <summary>
        ///  True if stream has ended and all buffered bytes were consumed.
        /// </summary>
        public bool EndOfStream => (_streamDone && Index == Length);

        /// <summary>
        ///  Total number of bytes read and consumed by reader.
        /// </summary>
        public long BytesRead => (_bytesRead - (Length - Index));

        /// <summary>
        ///  Ensure at least the specified length is available and unread on the buffer.
        ///  This will grow the buffer and read more data if required.
        /// </summary>
        /// <param name="length">Length in bytes required in buffer</param>
        /// <returns>True if length is now available, False if stream ends in fewer bytes than requested.</returns>
        public bool EnsureSpace(int length)
        {
            int bytesLeft = (Length - Index);
            if (bytesLeft >= length) { return true; }

            _stream.Refill(ref Index, ref Length, ref _streamDone, ref Buffer, length);
            _bytesRead += (Length - Index) - bytesLeft;
            return (Length - Index) > length;
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                if (CloseStream) { _stream.Dispose(); }
                _stream = null;
            }
        }
    }
}
