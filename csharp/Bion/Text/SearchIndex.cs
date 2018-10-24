using Bion.IO;
using System;
using System.IO;

namespace Bion
{
    /// <summary>
    ///  SearchIndexWriter writes a word index, which lists the positions where each
    ///  occurrence of a set of words was found.
    /// </summary>
    /// <remarks>
    ///  The binary file format is:
    ///    - For each word, write the delta of this occurrence after the last one as a 7-bit encoded terminated integer.
    ///    - Next, write a four-byte integer with the absolute offset where the matches for each word begin.
    ///    - Last, write a four-byte integer with the total word count in the index.
    /// </remarks>
    public class SearchIndexWriter : IDisposable
    {
        private string OutputPath;
        private string WorkingPath;
        private int BlockCount;
        private byte[] WriterBuffer;

        private int WordCount;
        private int[] FirstWordMatch;
        private int[] LastWordMatch;
        private long[] MatchPositions;
        private int[] NextMatchIndex;
        private int Count;

        public long WordTotal { get; private set; }
        public long NonDuplicateTotal { get; private set; }

        public SearchIndexWriter(string outputPath, int wordCount, int size)
        {
            OutputPath = outputPath;
            WorkingPath = Path.ChangeExtension(outputPath, ".Working");
            
            WriterBuffer = new byte[4096];

            WordCount = wordCount;
            FirstWordMatch = new int[wordCount];
            LastWordMatch = new int[wordCount];
            MatchPositions = new long[size];
            NextMatchIndex = new int[size];

            Directory.CreateDirectory(WorkingPath);

            Reset();
        }

        private void Reset()
        {
            Count = 0;

            Array.Fill(FirstWordMatch, -1);
            Array.Fill(LastWordMatch, -1);
        }

        /// <summary>
        ///  Add an entry for the given word at the given file position.
        /// </summary>
        /// <param name="wordIndex">Index of Word (from Word Compressor list)</param>
        /// <param name="position">Byte offset where word appears</param>
        public void Add(uint wordIndex, long position)
        {
            WordTotal++;
            int matchIndex = Count;

            int last = LastWordMatch[wordIndex];
            if (last != -1)
            {
                if (MatchPositions[last] == position)
                {
                    return;
                }
                else
                {
                    NextMatchIndex[last] = matchIndex;
                }
            }
            else
            {
                FirstWordMatch[wordIndex] = matchIndex;
            }

            MatchPositions[matchIndex] = position;
            NextMatchIndex[matchIndex] = -1;
            LastWordMatch[wordIndex] = matchIndex;

            Count++;
            NonDuplicateTotal++;

            if (Count == MatchPositions.Length)
            {
                Flush();
            }
        }

        private void Flush()
        {
            if (Count == 0) { return; }

            string filePath = Path.Combine(WorkingPath, $"{BlockCount}.idx");
            using (SearchIndexSliceWriter writer = new SearchIndexSliceWriter(new BufferedWriter(File.Create(filePath), WriterBuffer), WordCount))
            {
                for (int wordIndex = 0; wordIndex < WordCount; ++wordIndex)
                {
                    int matchIndex = FirstWordMatch[wordIndex];
                    while (matchIndex != -1)
                    {
                        writer.WritePosition(MatchPositions[matchIndex]);
                        matchIndex = NextMatchIndex[matchIndex];
                    }

                    writer.NextWord();
                }
            }

            BlockCount++;
            Reset();
        }

        private void Merge()
        {
            if (BlockCount == 1)
            {
                if (File.Exists(OutputPath)) { File.Delete(OutputPath); }
                File.Move(Path.Combine(WorkingPath, "0.idx"), OutputPath);
                Directory.Delete(WorkingPath, true);
                return;
            }

            SearchIndexSliceWriter writer = new SearchIndexSliceWriter(new BufferedWriter(File.Create(OutputPath), WriterBuffer), WordCount);
            SearchIndexReader[] readers = new SearchIndexReader[BlockCount];
            try
            {
                for (int i = 0; i < readers.Length; ++i)
                {
                    readers[i] = new SearchIndexReader(Path.Combine(WorkingPath, $"{i}.idx"));
                }

                long[] positions = null;

                for (uint wordIndex = 0; wordIndex < WordCount; ++wordIndex)
                {
                    for (int readerIndex = 0; readerIndex < readers.Length; ++readerIndex)
                    {
                        int count = readers[readerIndex].OffsetsForWord(wordIndex, ref positions);
                        for (int i = 0; i < count; ++i)
                        {
                            writer.WritePosition(positions[i]);
                        }
                    }

                    writer.NextWord();
                }
            }
            finally
            {
                for (int i = 0; i < readers.Length; ++i)
                {
                    if (readers[i] != null)
                    {
                        readers[i].Dispose();
                        readers[i] = null;
                    }
                }

                if (writer != null)
                {
                    writer.Dispose();
                    writer = null;
                }

                Directory.Delete(WorkingPath, true);
            }
        }

        public void Dispose()
        {
            Flush();
            Merge();
        }
    }

    internal class SearchIndexSliceWriter : IDisposable
    {
        private BufferedWriter _writer;
        private int _wordCount;
        private int[] _firstPositionPerWord;

        private long _lastPosition;
        private int _currentWordIndex;

        public SearchIndexSliceWriter(BufferedWriter writer, int wordCount)
        {
            _writer = writer;
            _wordCount = wordCount;
            _firstPositionPerWord = new int[wordCount];

            _currentWordIndex = -1;
            NextWord();
        }

        /// <summary>
        ///  Write the next position for the current word.
        /// </summary>
        /// <param name="position">Position in file where word occurs</param>
        public void WritePosition(long position)
        {
            if (_lastPosition == -1)
            {
                NumberConverter.WriteSevenBitTerminated(_writer, (ulong)position);
            }
            else if (position < _lastPosition)
            {
                throw new ArgumentException($"WritePosition must be given positions in ascending order. Position {position:n0} was less than previous position {_lastPosition:n0}.");
            }
            else if (position != _lastPosition)
            {
                NumberConverter.WriteSevenBitTerminated(_writer, (ulong)(position - _lastPosition));
            }

            _lastPosition = position;
        }

        /// <summary>
        ///  Indicate we're writing matches for the next word now.
        /// </summary>
        public void NextWord()
        {
            _lastPosition = -1;
            _currentWordIndex++;
            if (_currentWordIndex < _wordCount)
            {
                _firstPositionPerWord[_currentWordIndex] = (int)_writer.BytesWritten;
            }
        }

        private void WriteIndexMap()
        {
            if (_currentWordIndex < _wordCount) { throw new InvalidOperationException($"SearchIndexSliceWriter Dispose called before all {_wordCount:n0} words had positions written. Only {_currentWordIndex:n0} words have been written."); }

            // Write the position where the matches for each word begin
            NumberConverter.WriteIntBlock(_writer, _firstPositionPerWord, _wordCount);

            // Write the word count
            _writer.Write(_wordCount);
        }

        public void Dispose()
        {
            if (_writer != null)
            {
                // Write the map at the end of the index
                WriteIndexMap();

                _writer.Dispose();
                _writer = null;
            }
        }
    }

    /// <summary>
    ///  SearchIndexReader reads a word index, containing the positions of matches
    ///  for each word.
    /// </summary>
    public class SearchIndexReader : IDisposable
    {
        private BufferedReader _reader;
        private int[] _firstMatchOffset;

        /// <summary>
        ///  Build a reader for the given index.
        ///  This will load a small amount of data identifying where the matches
        ///  for each word begin in the index.
        /// </summary>
        /// <param name="indexPath">File Path of index to load</param>
        public SearchIndexReader(string indexPath)
        {
            _reader = new BufferedReader(File.OpenRead(indexPath));

            // Read word count (last four bytes)
            _reader.Seek(-4, SeekOrigin.End);
            int wordCount = _reader.ReadInt32();

            // Read start offset for each word's matches (just before count)
            _firstMatchOffset = new int[wordCount + 1];
            _reader.Seek(-4 * (wordCount + 1), SeekOrigin.End);
            _firstMatchOffset[wordCount] = (int)_reader.BytesRead;
            NumberConverter.ReadIntBlock(_reader, _firstMatchOffset, wordCount);
        }

        /// <summary>
        ///  Find all of the file offsets for the word with the given index
        ///  and write them to the provided array. The array will be reallocated
        ///  if it isn't large enough.
        /// </summary>
        /// <param name="wordIndex">Index of word to find matches for</param>
        /// <param name="buffer">Array to write match positions to</param>
        /// <returns>Number of matches written to array</returns>
        public int OffsetsForWord(uint wordIndex, ref long[] buffer)
        {
            // Find start and end of matches
            long startOffset = _firstMatchOffset[wordIndex];
            long endOffset = _firstMatchOffset[wordIndex + 1];
            int length = (int)(endOffset - startOffset);

            // Ensure buffer definitely long enough
            if (buffer == null || buffer.Length < length) { buffer = new long[length]; }

            // Read the match bytes
            _reader.Seek(startOffset, SeekOrigin.Begin);
            _reader.EnsureSpace(length);

            // Decode to non-relative longs
            int count = 0;
            long last = 0;
            while (_reader.BytesRead < endOffset)
            {
                long value = last + (long)NumberConverter.ReadSevenBitTerminated(_reader);
                buffer[count++] = value;
                last = value;
            }

            return count;
        }

        public void Dispose()
        {
            if (_reader != null)
            {
                _reader.Dispose();
                _reader = null;
            }
        }
    }
}
