// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;

namespace Bion.IO
{
    /// <summary>
    ///  BufferedWriter writes to a stream via a resizable buffer which is 
    ///  exposed directly to producers for performance.
    /// </summary>
    public class BufferedWriter : IDisposable
    {
        private Stream _stream;
        private long _bytesWritten;

        /// <summary>
        ///  Current byte buffer.
        ///  Write to Buffer[Index] and increment Index for content Written.
        /// </summary>
        public byte[] Buffer;

        /// <summary>
        ///  Index of next byte in Buffer to write.
        /// </summary>
        public int Index;

        /// <summary>
        ///  Total bytes written to file and pending in buffer.
        /// </summary>
        public long BytesWritten => (_bytesWritten + Index);

        /// <summary>
        ///  Whether to close the stream in Dispose.
        /// </summary>
        public bool CloseStream;

        /// <summary>
        ///  Construct a BufferedWriter on the desired stream with the
        ///  requested buffer size.
        /// </summary>
        /// <param name="stream">Stream to write to</param>
        /// <param name="size">Initial buffer size</param>
        public BufferedWriter(Stream stream, int size = 1024) : this(stream, new byte[size])
        { }

        /// <summary>
        ///  Construct a BufferedWriter on the desired stream with the
        ///  provided buffer.
        /// </summary>
        /// <param name="stream">Stream to write to</param>
        /// <param name="buffer">Initial buffer</param>
        public BufferedWriter(Stream stream, byte[] buffer)
        {
            _stream = stream;
            Buffer = buffer;
            CloseStream = true;
        }

        /// <summary>
        ///  Construct a BufferedWriter for the given file path.
        /// </summary>
        /// <param name="filePath">File Path to create</param>
        public BufferedWriter(string filePath) : this(File.Create(filePath))
        { }

        /// <summary>
        ///  Return a BufferedWriter which will write to an in-memory byte[] only.
        ///  It will use the passed array, but may have to resize it.
        ///  writer.Buffer[0, writer.Index] will contain the result.
        /// </summary>
        /// <returns>BufferedWriter keeping result in memory</returns>
        public static BufferedWriter ToArray(byte[] buffer)
        {
            return new BufferedWriter(null, buffer) { CloseStream = false };
        }

        /// <summary>
        ///  Ensure at least the specified length is available in the buffer to write.
        ///  This will flush and grow the buffer if required.
        /// </summary>
        /// <param name="length">Length in bytes required to write</param>
        public void EnsureSpace(int length)
        {
            int bytesLeft = Buffer.Length - Index;
            if(bytesLeft < length)
            {
                Flush();

                bytesLeft = Buffer.Length - Index;
                if (bytesLeft < length)
                {
                    int newLength = Buffer.Length * 2;
                    if (Index + length > newLength) { newLength = Index + length; }

                    byte[] newBuffer = new byte[newLength];
                    System.Buffer.BlockCopy(Buffer, 0, newBuffer, 0, Index);
                    Buffer = newBuffer;
                }
            }
        }

        /// <summary>
        ///  Flush any unwritten bytes to the stream.
        /// </summary>
        public void Flush()
        {
            if(Index > 0 && _stream != null)
            {
                _stream.Write(Buffer, 0, Index);
                _bytesWritten += Index;
                Index = 0;
            }
        }

        public void Dispose()
        {
            if(_stream != null)
            {
                Flush();
                _stream.Flush();
                if (CloseStream) { _stream.Dispose(); }
                _stream = null;
            }
        }
    }
}
