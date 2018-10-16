namespace Bion.IO
{
    /// <summary>
    ///  NumberConverter provides methods to read and write fixed and
    ///  variable length integers.
    /// </summary>
    public class NumberConverter
    {
        /// <summary>
        ///  Write value as a variable length, 7-bit encoded integer.
        ///  The last byte has a leading one bit, the others don't.
        /// </summary>
        /// <param name="value">Value to write</param>
        /// <param name="writer">BufferedWriter to write to</param>
        public static void WriteSevenBit(ulong value, BufferedWriter writer)
        {
            writer.EnsureSpace(10);

            while (value > 0x7F)
            {
                writer.Buffer[writer.Index++] = (byte)(value & 0x7F);
                value = value >> 7;
            }

            writer.Buffer[writer.Index++] = (byte)(value | 0x80);
        }

        /// <summary>
        ///  Read the next bytes from the reader as a variable length,
        ///  7-bit encoded integer.
        /// </summary>
        /// <param name="reader">BufferedReader to read from</param>
        /// <returns>Value read</returns>
        public static ulong ReadSevenBit(BufferedReader reader)
        {
            reader.EnsureSpace(10);

            ulong value = 0;
            int current = 0, shift = 0;

            while (current <= 0x7F)
            {
                current = reader.Buffer[reader.Index++];
                value += (ulong)(current & 0x7F) << shift;
                shift += 7;
            }

            return value;
        }

        /// <summary>
        ///  Write value as a variable length, 6-bit encoded integer.
        ///  All bytes start with a zero bit. The last byte starts with
        ///  01, the others start with 00.
        /// </summary>
        /// <param name="value">Value to write</param>
        /// <param name="writer">BufferedWriter to write to</param>
        public static void WriteSixBit(ulong value, BufferedWriter writer)
        {
            writer.EnsureSpace(11);

            while (value > 0x3F)
            {
                writer.Buffer[writer.Index++] = (byte)(value & 0x3F);
                value = value >> 6;
            }

            writer.Buffer[writer.Index++] = (byte)(value | 0x40);
        }

        /// <summary>
        ///  Read the next bytes from the reader as a variable length,
        ///  6-bit encoded integer.
        /// </summary>
        /// <param name="reader">BufferedReader to read from</param>
        /// <returns>Value read</returns>
        public static ulong ReadSixBit(BufferedReader reader)
        {
            reader.EnsureSpace(11);

            ulong value = 0;
            int current = 0, shift = 0;

            while (current <= 0x3F)
            {
                current = reader.Buffer[reader.Index++];
                value += (ulong)(current & 0x3F) << shift;
                shift += 6;
            }

            return value;
        }
    }
}
