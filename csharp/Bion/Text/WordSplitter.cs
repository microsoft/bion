// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using Bion.IO;

namespace Bion.Text
{
    public class WordSplitter
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

        public static int NextWordLength(BufferedReader reader, bool isWord)
        {
            int length = 1;
            while (reader.Index + length < reader.Length && WordSplitter.IsLetterOrDigit(reader.Buffer[reader.Index + length]) == isWord) length++;
            return length;
        }
    }
}
