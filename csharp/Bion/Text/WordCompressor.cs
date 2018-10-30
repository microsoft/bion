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
            // NOTE: WordIndex must be read from a 'ReadAll' BufferedReader because it isn't copying the words.
            WordCompressor compressor = new WordCompressor();
            using (BionReader reader = new BionReader(BufferedReader.ReadAll(dictionaryPath)))
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

        public WordEntry this[int wordIndex] => _words[wordIndex];

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
        private List<WordEntry> Words;
        private Dictionary<String8, int> Index;
        private bool Indexed;

        public WordIndex()
        {
            this.Words = new List<WordEntry>();
            this.Index = new Dictionary<String8, int>();
            this.Indexed = true;
        }

        public WordEntry this[int index] => Words[index];

        public int FindOrAdd(String8 word)
        {
            if (!this.Indexed) { Reindex(); }

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

        public bool TryFind(String8 word, out int index)
        {
            if (this.Indexed)
            {
                return Index.TryGetValue(word, out index);
            }
            else
            {
                WordEntry stub = new WordEntry(word);

                index = -1;
                int countDone = 0;
                int countForLength = 1 << WordCompressor.BitsPerByte;
                do
                {
                    int countToDo = Math.Min(countForLength, Words.Count - countDone);
                    index = Words.BinarySearch(countDone, countToDo, stub, WordEntryWordComparer.Instance);
                    countDone += countToDo;
                    countForLength = countForLength << WordCompressor.BitsPerByte;
                } while (index < 0 && countDone < Words.Count);

                return (index >= 0);
            }
        }

        public int[] Optimize()
        {
            if (!this.Indexed) { Reindex(); }
            int[] remapping = new int[Words.Count];

            // First, sort words in descending frequency order
            Words.Sort((left, right) => right.Count.CompareTo(left.Count));

            // Next, within each set with the same byte length, sort by ordinal
            int countDone = 0;
            int countForLength = 1 << WordCompressor.BitsPerByte;
            do
            {
                int countToDo = Math.Min(countForLength, Words.Count - countDone);
                Words.Sort(countDone, countToDo, WordEntryWordComparer.Instance);
                countDone += countToDo;
                countForLength = countForLength << WordCompressor.BitsPerByte;
            } while (countDone < Words.Count);

            // Look up the old index for each word to map to the new index
            for(int i = 0; i < Words.Count; ++i)
            {
                remapping[Index[Words[i].Word]] = i;
            }

            // Rebuild the index on the new order
            Index.Clear();
            for (int i = 0; i < Words.Count; ++i)
            {
                Index[Words[i].Word] = i;
            }

            return remapping;
        }

        public void Write(BionWriter writer)
        {
            writer.WriteStartArray();
            writer.WriteValue(Words.Count);

            foreach (WordEntry entry in Words)
            {
                writer.WriteValue(entry.Word);
                writer.WriteValue(entry.Count);
            }
            writer.WriteEndArray();
        }

        public void Read(BionReader reader)
        {
            reader.Read(BionToken.StartArray);
            reader.Read(BionToken.Integer);
            int wordCount = (int)reader.CurrentInteger();

            Words.Clear();
            Words.Capacity = wordCount;

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
                Words.Add(new WordEntry(value, count));
            }
        }

        private void Reindex()
        {
            Index.EnsureCapacity(Words.Count);

            for (int i = 0; i < Words.Count; ++i)
            {
                WordEntry entry = Words[i];
                Index[entry.Word] = i;
            }

            Indexed = true;
        }
    }

    public struct WordEntry
    {
        public String8 Word;
        public int Count;

        public WordEntry(String8 word, int count = 0)
        {
            this.Word = word;
            this.Count = count;
        }

        public override string ToString()
        {
            return $"\"{Word}\" ({Count:n0})";
        }
    }

    internal class WordEntryWordComparer : IComparer<WordEntry>
    {
        public static readonly WordEntryWordComparer Instance = new WordEntryWordComparer();

        public int Compare(WordEntry left, WordEntry right)
        {
            return left.Word.CompareTo(right.Word);
        }
    }
}
