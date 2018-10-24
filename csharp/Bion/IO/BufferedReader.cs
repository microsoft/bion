using Bion.Text;
using System;
using System.IO;
using System.Text;

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
        public BufferedReader(Stream stream, int size = 64 * 1024) : this(stream, new byte[size])
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
        }

        private BufferedReader(byte[] source, int index, int length)
        {
            Buffer = source;
            Index = index;
            Length = index + length;
            _stream = null;
            _streamDone = true;
            CloseStream = false;
        }

        /// <summary>
        ///  Construct a BufferedReader to read only the provided array.
        /// </summary>
        /// <param name="source">byte[] to read</param>
        /// <returns>BufferedReader reading over array only</returns>
        public static BufferedReader FromArray(byte[] source)
        {
            return new BufferedReader(source, 0, source.Length);
        }

        /// <summary>
        ///  Construct a BufferedReader to read only the provided array part.
        /// </summary>
        /// <param name="source">byte[] to read</param>
        /// <param name="index">Index from which to read</param>
        /// <param name="length">Length to read</param>
        /// <returns>BufferedReader reading over array only</returns>
        public static BufferedReader FromArray(byte[] source, int index, int length)
        {
            return new BufferedReader(source, index, length);
        }

        /// <summary>
        ///  Construct a BufferedReader to read the UTF8 bytes of a string.
        /// </summary>
        /// <param name="text">String to convert</param>
        /// <param name="convertBuffer">Reusable buffer to convert into</param>
        /// <returns>BufferedReader reading string value only</returns>
        public static BufferedReader FromString(string text, ref byte[] convertBuffer)
        {
            int length = Encoding.UTF8.GetByteCount(text);
            if (convertBuffer == null || convertBuffer.Length < length) { convertBuffer = new byte[length]; }
            Encoding.UTF8.GetBytes(text, convertBuffer);

            return BufferedReader.FromArray(convertBuffer, 0, length);
        }

        /// <summary>
        ///  Construct a BufferedReader to read a String8 value.
        /// </summary>
        /// <param name="text">String8 to read</param>
        /// <returns>BufferedReader reading String8 value only</returns>
        public static BufferedReader FromString(String8 text)
        {
            return BufferedReader.FromArray(text.Array, text.Index, text.Length);
        }

        /// <summary>
        ///  Repoint a BufferedReader at an array segment.
        ///  Used to read from arrays without constructing BufferedReaders constantly.
        /// </summary>
        /// <param name="source">Array to read from</param>
        /// <param name="index">Index to read from</param>
        /// <param name="end">Index to read before</param>
        public void ReadSlice(byte[] source, int index, int end)
        {
            _streamDone = true;
            _stream = null;
            Buffer = source;
            Index = index;
            Length = end;
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
        /// <returns>Length available, which may be more or less than desired length.</returns>
        public int EnsureSpace(int length, int maxReadLength = -1)
        {
            int bytesLeft = (Length - Index);
            if (bytesLeft >= length) { return bytesLeft; }

            byte[] toFill = Buffer;

            // If the buffer is too small, increase it
            if (length > Buffer.Length)
            {
                toFill = new byte[Math.Max(length, Buffer.Length * 2)];
            }

            // Copy any leftover bytes
            System.Buffer.BlockCopy(Buffer, Index, toFill, 0, bytesLeft);
            Buffer = toFill;
            Length = bytesLeft;
            Index = 0;

            // Fill the remainder of the buffer
            if (!_streamDone)
            {
                int lengthToRead = toFill.Length - bytesLeft;
                if (maxReadLength > 0 && maxReadLength < lengthToRead) { lengthToRead = maxReadLength; }

                Length += Read(toFill, bytesLeft, lengthToRead, out _streamDone);
            }

            _bytesRead += Length - bytesLeft;
            return (Length - Index);
        }

        protected virtual int Read(byte[] buffer, int index, int length, out bool streamDone)
        {
            int lengthRead = _stream.Read(buffer, index, length);
            streamDone = (lengthRead < length);
            return lengthRead;
        }

        public void Seek(long offset, SeekOrigin origin)
        {
            // Do nothing if already in correct position
            if (origin == SeekOrigin.Begin && offset == BytesRead) { return; }
            
            // Seek
            _stream.Seek(offset, origin);

            // Mark stream not done and buffer empty
            _bytesRead = _stream.Position;
            _streamDone = false;
            Index = 0;
            Length = 0;
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
