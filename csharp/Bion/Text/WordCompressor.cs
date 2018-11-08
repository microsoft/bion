using Bion.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bion.Text
{
    public class WordCompressor : IDisposable
    {
        public const int BitsPerByte = 6;

        private WordIndex _words;
        private Stream _writeToStream;
        private ulong[] _block = new ulong[128];

        private WordCompressor()
        {
            this._words = new WordIndex();
        }

        public static WordCompressor OpenWrite(string dictionaryPath)
        {
            return new WordCompressor() { _writeToStream = File.Create(dictionaryPath) };
        }

        public static WordCompressor OpenRead(string dictionaryPath)
        {
            WordCompressor compressor = new WordCompressor();
            using (BionReader reader = new BionReader(File.OpenRead(dictionaryPath)))
            {
                compressor.Read(reader);
            }

            return compressor;
        }

        public static void Compress(string fromPath, string toPath, string toDictionaryPath, bool optimize = true)
        {
            using (WordCompressor compressor = WordCompressor.OpenWrite(toDictionaryPath))
            {
                string firstWritePath = toPath;
                if(optimize) { firstWritePath = Path.ChangeExtension(toPath, ".tmp"); }

                // First Pass
                using (BufferedReader reader = new BufferedReader(File.OpenRead(fromPath)))
                using (BufferedWriter writer = new BufferedWriter(File.Create(firstWritePath)))
                {
                    compressor.Compress(reader, writer);
                }

                // Optimize Pass
                if (optimize)
                {
                    int[] map = compressor.OptimizeIndex();
                    using (BufferedReader reader = new BufferedReader(File.OpenRead(firstWritePath)))
                    using (BufferedWriter writer = new BufferedWriter(File.Create(toPath)))
                    {
                        compressor.RewriteOptimized(map, reader, writer);
                    }
                }
            }
        }

        public static void Expand(string fromPath, string toPath, string fromDictionaryPath)
        {
            using (WordCompressor compressor = WordCompressor.OpenRead(fromDictionaryPath))
            using (BufferedReader reader = new BufferedReader(File.OpenRead(fromPath)))
            using (BufferedWriter writer = new BufferedWriter(File.Create(toPath)))
            {
                compressor.Expand(reader, writer);
            }
        }

        public void Compress(BufferedReader reader, BufferedWriter writer)
        {
            if (reader.EndOfStream) { return; }

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
                    int wordIndex = _words.FindOrAdd(word);
                    NumberConverter.WriteSixBitTerminated(writer, (ulong)wordIndex);
                }
                else if(!reader.EndOfStream)
                {
                    // Reset state to re-read this word
                    reader.Index -= length;
                    isWord = !isWord;

                    // If end of buffer but not stream, request more
                    reader.EnsureSpace(length * 2);
                }
            }
        }

        public int[] OptimizeIndex()
        {
            return _words.Optimize();
        }

        public bool TryGetWordIndex(String8 word, out int index)
        {
            return _words.TryFind(word, out index);
        }

        public WordIndex.WordEntry this[int wordIndex] => _words[wordIndex];

        public void RewriteOptimized(int[] map, BufferedReader reader, BufferedWriter writer, SearchIndexWriter indexWriter = null)
        {
            long valueStart = writer.BytesWritten;
            while (!reader.EndOfStream)
            {
                ulong index = NumberConverter.ReadSixBitTerminated(reader);
                int remapped = map[index];
                indexWriter?.Add(remapped, valueStart);
                NumberConverter.WriteSixBitTerminated(writer, (ulong)remapped);
            }
        }

        public void Expand(BufferedReader reader, BufferedWriter writer)
        {
            while (!reader.EndOfStream)
            {
                int count = NumberConverter.ReadSixBitTerminatedBlock(reader, _block);
                for (int i = 0; i < count; ++i)
                {
                    int wordIndex = (int)_block[i];
                    String8 word = _words[wordIndex].Word;

                    writer.EnsureSpace(word.Length);
                    word.CopyTo(writer.Buffer, writer.Index);
                    writer.Index += word.Length;
                }
            }
        }

        private void Read(BionReader reader)
        {
            _words.Read(reader);
        }

        public void Write(Stream stream)
        {
            using (BionWriter writer = new BionWriter(stream))
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
        private String8Set Words;
        private List<int> Counts;

        private Dictionary<String8, int> Index;
        private bool Indexed;

        public WordIndex()
        {
            this.Words = new String8Set();
            this.Counts = new List<int>();
            this.Index = new Dictionary<String8, int>();
            this.Indexed = true;
        }

        public WordEntry this[int index] => new WordEntry(this, index);
        public int Count => Words.Count;

        public int FindOrAdd(String8 word)
        {
            if (!this.Indexed) { Reindex(); }

            int index;
            if(Index.TryGetValue(word, out index))
            {
                Counts[index]++;
                return index;
            }

            index = Count;
            Words.Add(word);
            Counts.Add(1);

            Index[Words[index]] = index;

            return index;
        }

        public bool TryFind(String8 word, out int index)
        {
            if (this.Indexed)
            {
                return Index.TryGetValue(word, out index);
            }
            else
            {
                index = -1;
                int countDone = 0;
                int countForLength = 1 << WordCompressor.BitsPerByte;
                do
                {
                    int countToDo = Math.Min(countForLength, Count - countDone);
                    index = Words.BinarySearch(countDone, countToDo, word);
                    countDone += countToDo;
                    countForLength = countForLength << WordCompressor.BitsPerByte;
                } while (index < 0 && countDone < Count);

                return (index >= 0);
            }
        }

        public int[] Optimize()
        {
            if (!this.Indexed) { Reindex(); }
            int[] remapping = new int[Count];

            int[] indices = new int[Count];
            for(int i = 0; i < indices.Length; ++i)
            {
                indices[i] = i;
            }

            // First, sort words in descending frequency order
            Array.Sort(indices, new CountDescendingComparer(this));

            // Next, within each set with the same byte length, sort by ordinal
            IComparer<int> wordComparer = new WordComparer(this);
            int countDone = 0;
            int countForLength = 1 << WordCompressor.BitsPerByte;
            do
            {
                int countToDo = Math.Min(countForLength, Count - countDone);
                Array.Sort(indices, countDone, countToDo, wordComparer);
                countDone += countToDo;
                countForLength = countForLength << WordCompressor.BitsPerByte;
            } while (countDone < Count);

            // Look up the old index for each word to map to the new index
            for(int i = 0; i < Count; ++i)
            {
                remapping[indices[i]] = i;
            }

            // Rebuild the word set
            String8Set newSet = new String8Set(Count, Words.LengthBytes);
            for(int i = 0; i < Count; ++i)
            {
                newSet.Add(Words[indices[i]]);
            }
            Words = newSet;

            // Clear the index (rebuild if needed later)
            Index.Clear();
            Indexed = false;

            return remapping;
        }

        public void Write(BionWriter writer)
        {
            writer.WriteStartArray();
            writer.WriteValue(Count);

            for(int i = 0; i < Count; ++i)
            {
                writer.WriteValue(Words[i]);
                writer.WriteValue(Counts[i]);
            }
            writer.WriteEndArray();
        }

        public void Read(BionReader reader)
        {
            reader.Read(BionToken.StartArray);
            reader.Read(BionToken.Integer);
            int wordCount = (int)reader.CurrentInteger();

            Words.Clear();

            Index.Clear();
            Indexed = false;

            while (true)
            {
                reader.Read();
                if (reader.TokenType == BionToken.EndArray) { break; }

                // NOTE: Not copying word. Must use a 'ReadAll' BufferedReader
                reader.Expect(BionToken.String);
                String8 value = reader.CurrentString8();

                reader.Read(BionToken.Integer);
                int count = (int)reader.CurrentInteger();

                // Add to List, but not Index. Index will be populated when first needed.
                Words.Add(value);
                Counts.Add(count);
            }

            // Set capacity exact to save RAM
            Words.SetCapacity(Words.LengthBytes);
        }

        private void Reindex()
        {
            Index.EnsureCapacity(Count);
            Index.Clear();

            for (int i = 0; i < Count; ++i)
            {
                WordEntry entry = this[i];
                Index[entry.Word] = i;
            }

            Indexed = true;
        }

        public struct WordEntry
        {
            private WordIndex _container;
            private int _index;
            public String8 Word => _container.Words[_index];
            public int Count => _container.Counts[_index];

            public WordEntry(WordIndex container, int index)
            {
                _container = container;
                _index = index;
            }

            public override string ToString()
            {
                return $"\"{Word}\" ({Count:n0})";
            }
        }

        internal class WordComparer : IComparer<int>
        {
            private WordIndex _index;

            public WordComparer(WordIndex index)
            {
                _index = index;
            }

            public int Compare(int left, int right)
            {
                return _index.Words[left].CompareTo(_index.Words[right]);
            }
        }

        internal class CountDescendingComparer : IComparer<int>
        {
            private WordIndex _index;

            public CountDescendingComparer(WordIndex index)
            {
                _index = index;
            }

            public int Compare(int left, int right)
            {
                return _index.Counts[right].CompareTo(_index.Counts[left]);
            }
        }
    }
}
