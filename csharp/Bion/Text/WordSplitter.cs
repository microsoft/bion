using System;

namespace Bion.Text
{
    internal class WordSplitter
    {
        private static bool[] _letterOrDigitLookup;

        static WordSplitter()
        {
            _letterOrDigitLookup = new bool[256];
            Array.Fill(_letterOrDigitLookup, true, 0x30, 10);     // 0-9
            Array.Fill(_letterOrDigitLookup, true, 0x41, 26);     // A-Z
            Array.Fill(_letterOrDigitLookup, true, 0x61, 26);     // a-z
            Array.Fill(_letterOrDigitLookup, true, 0x80, 128);    // Multibyte
        }

        public static bool IsLetterOrDigit(byte b)
        {
            return _letterOrDigitLookup[b];
        }

        public static int WordLength(ReadOnlySpan<byte> text, int index, bool isWord)
        {
            int length = 1;
            while (index + length < text.Length && WordSplitter.IsLetterOrDigit(text[index + length]) == isWord) length++;
            return length;
        }
    }
}
