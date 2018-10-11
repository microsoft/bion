using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bion.Text
{
    public class WordCompressor : IDisposable
    {
        private WordIndex _words;
        private Stream _writeToStream;

        private WordCompressor()
        {
            this._words = new WordIndex();
        }

        public static WordCompressor OpenWrite(string filePath)
        {
            return new WordCompressor() { _writeToStream = File.OpenWrite(filePath) };
        }

        public static WordCompressor OpenRead(string filePath)
        {
            WordCompressor compressor = new WordCompressor();
            using (BionReader reader = new BionReader(File.OpenRead(filePath)))
            {
                compressor.Read(reader);
            }

            return compressor;
        }

        public void Compress(ReadOnlyMemory<byte> text, NumberWriter writer)
        {
            if (text.IsEmpty) return;
            
            ReadOnlySpan<byte> textSpan = text.Span;

            int index = 0;
            int length;
            bool isWord = WordSplitter.IsLetterOrDigit(textSpan[0]);
            while (index < text.Length)
            {
                length = 1;
                while (index + length < text.Length && WordSplitter.IsLetterOrDigit(textSpan[index + length]) == isWord) length++;

                String8 word = new String8(text.Slice(index, length));

                int wordIndex = _words.FindOrAdd(word);
                writer.WriteValue((ulong)wordIndex);

                isWord = !isWord;
                index += length;
            }
        }

        public void Optimize(NumberReader reader, NumberWriter writer)
        {
            int[] map = _words.Optimize();

            while (!reader.EndOfStream)
            {
                int index = (int)reader.ReadNumber();
                int remapped = map[index];
                writer.WriteValue((ulong)remapped);
            }
        }

        public int Decompress(NumberReader reader, Span<byte> buffer)
        {
            int lengthWritten = 0;

            while (!reader.EndOfStream)
            {
                int wordIndex = (int)reader.ReadNumber();
                String8 word = _words[wordIndex];

                if(lengthWritten + word.Value.Length > buffer.Length)
                {
                    reader.UndoRead((ulong)wordIndex);
                    break;
                }

                word.Value.Span.CopyTo(buffer.Slice(lengthWritten));
                lengthWritten += word.Value.Length;
            }

            return lengthWritten;
        }

        private void Read(BionReader reader)
        {
            _words.Read(reader);
        }

        public void Write(Stream stream)
        {
            using (BionWriter writer = new BionWriter(stream, lookupDictionary: null))
            {
                Write(writer);
            }
        }

        private void Write(BionWriter writer)
        {
            _words.Write(writer);
        }

        public void Dispose()
        {
            if (_writeToStream != null)
            {
                Write(_writeToStream);
                _writeToStream = null;
            }
        }

        private class WordSplitter
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
        }
    }

    public class WordIndex
    {
        private List<WordEntry> Words;
        private Dictionary<String8, int> Index;

        public WordIndex()
        {
            this.Words = new List<WordEntry>();
            this.Index = new Dictionary<String8, int>();
        }

        public String8 this[long index] => Words[(int)index].Value;

        public int FindOrAdd(String8 word)
        {
            int index;
            if(Index.TryGetValue(word, out index))
            {
                WordEntry entry = Words[index];
                entry.Count++;
                Words[index] = entry;

                return index;
            }

            index = Words.Count;
            String8 wordCopy = String8.Copy(word);

            Words.Add(new WordEntry(wordCopy, 1));
            Index[wordCopy] = index;

            return index;
        }

        public int[] Optimize()
        {
            int[] remapping = new int[Words.Count];

            // Sort words in descending frequency order
            Words.Sort((left, right) => right.Count.CompareTo(left.Count));

            // Look up the old index for each word to map to the new index
            for(int i = 0; i < Words.Count; ++i)
            {
                remapping[Index[Words[i].Value]] = i;
            }

            // Rebuild the index on the new order
            Index.Clear();
            for (int i = 0; i < Words.Count; ++i)
            {
                Index[Words[i].Value] = i;
            }

            return remapping;
        }

        public void Write(BionWriter writer)
        {
            writer.WriteStartArray();
            foreach (WordEntry entry in Words)
            {
                writer.WriteValue(entry.Value.Value.Span);
            }
            writer.WriteEndArray();
        }

        public void Read(BionReader reader)
        {
            Words.Clear();
            Index.Clear();

            reader.Read(BionToken.StartArray);

            while (true)
            {
                reader.Read();
                if (reader.TokenType == BionToken.EndArray) { break; }

                String8 value = String8.Copy(new String8(reader.CurrentBytes()));
                Index[value] = Words.Count;
                Words.Add(new WordEntry(value));
            }
        }
    }

    public struct WordEntry
    {
        public String8 Value;
        public int Count;

        public WordEntry(String8 value, int count = 0)
        {
            this.Value = value;
            this.Count = count;
        }
    }
}
