using System;
using System.IO;

namespace Bion.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        ///  Refill makes it easy to implement a Reader/Writer pattern with Memory&lt;byte&gt;.
        ///  Refill grows the buffer, if needed, copies unused bytes, and fills the buffer.
        /// </summary>
        /// <remarks>
        ///  Usage
        ///  =====
        ///  Memory&lt;byte&gt; buffer = new byte[64 * 1024];
        ///  Memory&lt;byte&gt; left = Memory&lt;byte&gt;.Empty;
        ///  bool readerDone = false;
        ///  
        ///  while (!readerDone)
        ///  {
        ///      left = stream.Refill(left, out readerDone, ref buffer);
        ///      left = Consume(left, readerDone);
        ///  }
        ///  
        ///  This model:
        ///    - Makes reader/writer loop simple.
        ///       - No expressions (buffer.Slice(...), length &lt; buffer.Length, ...)
        ///       - No new types required.
        ///       - Bool condition for loop and consume to tell if there is more input.
        ///    - Implementing consumers is simple.
        ///       - Args are just (Memory&lt;byte&gt; memory, bool isComplete)
        ///       - Consumers can be used for complete data or buffers which may be incomplete.
        ///       - Consumers don't have to consume all data; return unconsumed tail.
        ///    - Refill handles complexity:
        ///       - Increasing buffer when it wasn't big enough to consume anything.
        ///       - Copying unused bytes from previous call.
        ///       - Identifying if source is out of content.
        /// </remarks>
        /// <param name="source">Stream to read from</param>
        /// <param name="left">Bytes from previous read which weren't consumed</param>
        /// <param name="readerDone">True if reader has no remaining bytes after this call</param>
        /// <param name="buffer">Buffer to read into and grow if needed</param>
        /// <returns>Memory&lt;byte&gt; with bytes from 'left' and remaining bytes which could be read</returns>
        public static Memory<byte> Refill(this Stream source, ReadOnlyMemory<byte> left, out bool readerDone, ref Memory<byte> buffer)
        {
            // If nothing could be consumed, expand the buffer
            if (left.Length == buffer.Length) { buffer = new byte[buffer.Length * 2]; }

            // Copy the unused content to the beginning of the buffer
            left.CopyTo(buffer);

            // Fill the remainder of the buffer
            int newFilledLength = left.Length + source.Read(buffer.Slice(left.Length).Span);

            // The reader is done if the buffer wasn't filled
            readerDone = newFilledLength < buffer.Length;

            // Return the portion filled
            return buffer.Slice(0, newFilledLength);
        }
    }
}
