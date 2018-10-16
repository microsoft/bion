using Bion.Core;
using Bion.IO;
using System;
using System.Collections.Generic;
using System.IO;

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

        public ReadOnlyMemory<byte> Compress(ReadOnlyMemory<byte> text, bool isComplete, BufferedWriter writer)
        {
            if (text.IsEmpty) return ReadOnlyMemory<byte>.Empty;
            
            ReadOnlySpan<byte> textSpan = text.Span;

            int index = 0;
            int length = 0;
            bool isWord = WordSplitter.IsLetterOrDigit(textSpan[0]);
            while (index < text.Length)
            {
                // Find length of current word
                length = WordSplitter.WordLength(textSpan, index, isWord);
                String8 word = String8.Reference(text.Slice(index, length));

                // If this is the last word but not the end of input, don't consume it
                if (!isComplete && index + length == text.Length) { break; }
                
                uint wordIndex = _words.FindOrAdd(word);
                NumberConverter.WriteSevenBit(wordIndex, writer);

                isWord = !isWord;
                index += length;
            }

            // Return the leftover input, if any
            return text.Slice(index);
        }

        public void Optimize(BufferedReader reader, BufferedWriter writer)
        {
            uint[] map = _words.Optimize();

            while (!reader.EndOfStream)
            {
                ulong index = NumberConverter.ReadSevenBit(reader);
                uint remapped = map[index];
                NumberConverter.WriteSevenBit(remapped, writer);
            }
        }

        public Memory<byte> Decompress(BufferedReader reader, Memory<byte> buffer, out bool readerDone)
        {
            Span<byte> span = buffer.Span;
            int lengthWritten = 0;

            while (!reader.EndOfStream)
            {
                int lastBufferIndex = reader.Index;
                ulong wordIndex = NumberConverter.ReadSevenBit(reader);
                String8 word = _words[wordIndex];

                if(lengthWritten + word.Length > buffer.Length)
                {
                    reader.Index = lastBufferIndex;
                    break;
                }

                word.CopyTo(span.Slice(lengthWritten));
                lengthWritten += word.Value.Length;
            }

            readerDone = reader.EndOfStream;
            return buffer.Slice(0, lengthWritten);
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

        public String8 this[ulong index] => Words[(int)index].Value;

        public uint FindOrAdd(String8 word)
        {
            int index;
            if(Index.TryGetValue(word, out index))
            {
                WordEntry entry = Words[index];
                entry.Count++;
                Words[index] = entry;

                return (uint)index;
            }

            index = Words.Count;
            String8 wordCopy = String8.Copy(word);

            Words.Add(new WordEntry(wordCopy, 1));
            Index[wordCopy] = index;

            return (uint)index;
        }

        public uint[] Optimize()
        {
            uint[] remapping = new uint[Words.Count];

            // Sort words in descending frequency order
            Words.Sort((left, right) => right.Count.CompareTo(left.Count));

            // Look up the old index for each word to map to the new index
            for(int i = 0; i < Words.Count; ++i)
            {
                remapping[Index[Words[i].Value]] = (uint)i;
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
                writer.WriteValue(entry.Count);
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

                reader.Expect(BionToken.String);
                String8 value = String8.Copy(reader.CurrentBytes());

                reader.Read(BionToken.Integer);
                int count = (int)reader.CurrentInteger();
                Index[value] = Words.Count;
                Words.Add(new WordEntry(value, count));
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
