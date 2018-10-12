using System;
using System.IO;

namespace Bion.Extensions
{
    public static class StreamExtensions
    {
        /// <summary>
        ///  Refill makes it easy to implement a Reader/Writer pattern with Memory&lt;byte&gt;.
        ///   - Copy any unread bytes to beginning of buffer.
        ///   - Grow buffer if needed (if nothing could be consumed before).
        ///   - Refill buffer from source stream.
        ///   - Return new length and whether stream is at end.
        /// </summary>
        /// <remarks>
        ///  Usage
        ///  =====
        ///  byte[] buffer = new byte[64 * 1024];
        ///  Memory&lt;byte&gt; left = Memory&lt;byte&gt;.Empty;
        ///  bool readerDone = false;
        ///  
        ///  while (!readerDone)
        ///  {
        ///      left = stream.Refill(left, ref readerDone, ref buffer);
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
        /// <param name="readerDone">Whether stream is at end</param>
        /// <param name="buffer">Buffer to read into and grow if needed</param>
        /// <returns>Memory&lt;byte&gt; with bytes from 'left' and remaining bytes which could be read</returns>
        public static Memory<byte> Refill(this Stream source, ReadOnlyMemory<byte> left, ref bool readerDone, ref byte[] buffer)
        {
            // If nothing could be consumed, expand the buffer
            if (left.Length == buffer.Length && !readerDone) { buffer = new byte[buffer.Length * 2]; }

            // Copy the unused content to the beginning of the buffer
            left.CopyTo(buffer);
            int newFilledLength = left.Length;

            // Fill the remainder of the buffer
            if (!readerDone)
            {
                newFilledLength += source.Read(buffer.AsSpan(left.Length));
                readerDone = newFilledLength < buffer.Length;
            }

            // Return the portion filled
            return buffer.AsMemory(0, newFilledLength);
        }

        /// <summary>
        ///  Refill for direct byte[] style use.
        ///   - Copy any unread bytes to beginning of buffer.
        ///   - Grow buffer if needed (if nothing could be consumed before).
        ///   - Refill buffer from source stream.
        ///   - Return new length and whether stream is at end.
        /// </summary>
        /// <param name="source">Stream to read from</param>
        /// <param name="index">Index of first unconsumed byte, or 0.</param>
        /// <param name="lastLength">Length of previous Refill, or 0.</param>
        /// <param name="readerDone">Whether stream is at end</param>
        /// <param name="buffer">Buffer to read into and grow if needed</param>
        public static void Refill(this Stream source, ref int index, ref int length, ref bool readerDone, ref byte[] buffer)
        {
            // Refill with Memory overload
            Memory<byte> left = buffer.AsMemory(index, length - index);
            Memory<byte> refill = source.Refill(left, ref readerDone, ref buffer);

            // Update index and length for new read
            index = 0;
            length = refill.Length;
        }
    }
}
