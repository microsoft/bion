// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text;

using Bion.IO;
using Bion.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test.Text
{
    [TestClass]
    public class WordSplitterTests
    {
        [TestMethod]
        public void WordSplitter_Basics()
        {
            Assert.AreEqual("System|.|Collections|.|Generic", SplitAndJoin("System.Collections.Generic"));
            Assert.AreEqual("ceaf4681|-|50dc|-|4e09|-|8e4f|-|a2c8d4ca5938", SplitAndJoin("ceaf4681-50dc-4e09-8e4f-a2c8d4ca5938"));
            Assert.AreEqual("This| |is| |a|\r\n|test|, |really|.", SplitAndJoin("This is a\r\ntest, really."));
        }

        private static string SplitAndJoin(string content)
        {
            StringBuilder result = new StringBuilder();

            byte[] buffer = new byte[content.Length];
            using (BufferedReader reader = BufferedReader.FromString(content, ref buffer))
            {
                bool isWord = WordSplitter.IsLetterOrDigit(reader.Buffer[reader.Index]);
                int length = 0;
                while (!reader.EndOfStream)
                {
                    // Read the next word
                    length = WordSplitter.NextWordLength(reader, isWord);
                    String8 word = String8.Reference(reader.Buffer, reader.Index, length);

                    // Set state to read next word
                    reader.Index += length;
                    isWord = !isWord;

                    if (reader.Index < reader.Length || reader.EndOfStream)
                    {
                        // If this is word is definitely complete, write it
                        if (result.Length > 0) { result.Append("|"); }
                        result.Append(word.ToString());
                    }
                    else if (!reader.EndOfStream)
                    {
                        // Reset state to re-read this word
                        reader.Index -= length;
                        isWord = !isWord;

                        // If end of buffer but not stream, request more
                        reader.EnsureSpace(length * 2);
                    }
                }
            }

            return result.ToString();
        }
    }
}
