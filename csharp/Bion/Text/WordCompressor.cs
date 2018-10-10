using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bion.Text
{
    public class WordCompressor : IDisposable
    {
        private WordIndex Words;
        private WordIndex NonWords;

        private Stream _writeToStream;

        private WordCompressor()
        {
            this.Words = new WordIndex();
            this.NonWords = new WordIndex();
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

        public void Compress(string text, BionWriter writer)
        {
            if (String.IsNullOrEmpty(text))
            {
                writer.WriteStartArray();
                writer.WriteValue(true);
                writer.WriteEndArray();
                return;
            }

            writer.WriteStartArray();
            
            int index = 0;
            int length;
            bool isWord = Char.IsLetterOrDigit(text[0]);
            writer.WriteValue(isWord);

            while(index < text.Length)
            {
                length = 1;
                while (index + length < text.Length && Char.IsLetterOrDigit(text[index + length]) == isWord) length++;

                string word = text.Substring(index, length);
                WordIndex set = (isWord ? Words : NonWords);

                int wordIndex = set.FindOrAdd(word);
                writer.WriteValue(wordIndex);

                isWord = !isWord;
                index += length;
            }

            writer.WriteEndArray();
        }

        public void Optimize(BionReader reader, BionWriter writer)
        {
            int[] wordRemapping = Words.Optimize();
            int[] nonWordRemapping = NonWords.Optimize();

            reader.Read(BionToken.StartArray);
            writer.WriteStartArray();

            reader.Read();
            bool isWord = reader.CurrentBool();
            writer.WriteValue(isWord);

            while (true)
            {
                reader.Read();
                if (reader.TokenType == BionToken.EndArray) { break; }

                long index = reader.CurrentInteger();
                int[] map = (isWord ? wordRemapping : nonWordRemapping);

                long remapped = map[index];
                writer.WriteValue(remapped);

                isWord = !isWord;
            }

            writer.WriteEndArray();
        }

        public string Decompress(BionReader reader)
        {
            StringBuilder result = new StringBuilder();

            reader.Read(BionToken.StartArray);

            reader.Read();
            bool isWord = reader.CurrentBool();

            while (true)
            {
                reader.Read();
                if (reader.TokenType == BionToken.EndArray) { break; }

                long index = reader.CurrentInteger();
                WordIndex set = (isWord ? Words : NonWords);

                result.Append(set[index]);
                isWord = !isWord;
            }

            return result.ToString();
        }

        private void Read(BionReader reader)
        {
            reader.Read(BionToken.StartObject);

            reader.Read(BionToken.PropertyName);
            if (reader.CurrentString() != "words") { throw new BionSyntaxException(reader, "words"); }
            Words.Read(reader);

            reader.Read(BionToken.PropertyName);
            if (reader.CurrentString() != "nonWords") { throw new BionSyntaxException(reader, "nonWords"); }
            NonWords.Read(reader);

            reader.Read(BionToken.EndObject);
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
            writer.WriteStartObject();

            writer.WritePropertyName("words");
            Words.Write(writer);

            writer.WritePropertyName("nonWords");
            NonWords.Write(writer);

            writer.WriteEndObject();
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
        private Dictionary<string, int> Index;

        public WordIndex()
        {
            this.Words = new List<WordEntry>();
            this.Index = new Dictionary<string, int>();
        }

        public string this[long index] => Words[(int)index].Value;

        public int FindOrAdd(string word)
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
            Words.Add(new WordEntry(word, 1));
            Index[word] = index;

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
                writer.WriteValue(entry.Value);
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

                string value = reader.CurrentString();
                Index[value] = Words.Count;
                Words.Add(new WordEntry(value));
            }
        }
    }

    public struct WordEntry
    {
        public string Value;
        public int Count;

        public WordEntry(string value, int count = 0)
        {
            this.Value = value;
            this.Count = count;
        }
    }
}
