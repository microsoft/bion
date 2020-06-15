// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using Bion.Core;
using Bion.IO;
using Bion.Text;
using Bion.Vector;

namespace Bion.Console
{
    public class MyArray : IEnumerable<int>
    {
        private readonly int[] _inner;

        public MyArray(int[] inner)
        {
            _inner = inner;
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_inner);
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return new Enumerator(_inner);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(_inner);
        }

        public class Enumerator : IEnumerator<int>
        {
            private int[] _inner;
            private int _index;
            private int _current;

            public Enumerator(int[] inner)
            {
                _inner = inner;
            }

            public int Current => _current;
            object IEnumerator.Current => _current;

            public bool MoveNext()
            {
                _current = _inner[_index];
                _index++;
                return _index < _inner.Length;
            }

            public void Reset()
            {
                _index = 0;
            }

            public void Dispose()
            { }
        }
    }

    public static class Benchmark
    {
        public const int BlockSize = 64;

        public static void Test()
        {
            string syntheticBlockPath = @"C:\Download\Sarif\Out\BlockTest.blk";

            string compressedPath = @"C:\Download\Sarif\Out\SarifSearch.All.cmp";
            string dictionaryPath = @"C:\Download\Sarif\Out\SarifSearch.All.wdx";
            string searchIndexPath = @"C:\Download\Sarif\Out\SarifSearch.All.idx";
            int bufferSize = 64 * 1024;

            ArrayTest();
            //return;

            //DictionaryLengths(dictionaryPath);
            //WriteWordsForLength(dictionaryPath, 52);

            //ReadBytes(compressedPath, bufferSize);
            //ReadBufferedReader(compressedPath, bufferSize);
            Read6Bit(compressedPath, bufferSize);

            //VectorTest(compressedPath);

            //TranslateSixBit(compressedPath, compressedPath + ".blk");
            //ReadIntBlock(compressedPath + ".blk", bufferSize);

            //TranslateSearchIndex(searchIndexPath, searchIndexPath + ".blk", true);
            //ReadIntBlock(searchIndexPath + ".blk", bufferSize);

            //TranslateDictionaryPositions(dictionaryPath, dictionaryPath + ".blk", true);
            //ReadIntBlock(dictionaryPath + ".blk", bufferSize);

            //WriteSyntheticBlock(syntheticBlockPath, 256 * 1024 * 1024);
            ReadIntBlock(syntheticBlockPath, bufferSize);
        }

        public static void ReadBytes(string filePath, int bufferSizeBytes)
        {
            byte[] buffer = new byte[bufferSizeBytes];
            long totalSize = 0;

            using (new ConsoleWatch($"ReadBytes(\"{filePath}\", {bufferSizeBytes})", () => $"Done; {totalSize:n0} bytes"))
            {
                using (FileStream stream = File.OpenRead(filePath))
                {
                    while (true)
                    {
                        int length = stream.Read(buffer, 0, buffer.Length);
                        totalSize += length;
                        if (length < buffer.Length) { break; }
                    }
                }
            }
        }

        public static void ReadBufferedReader(string filePath, int bufferSizeBytes)
        {
            byte[] buffer = new byte[bufferSizeBytes];
            long totalSize = 0;

            using (new ConsoleWatch($"ReadBufferedReader(\"{filePath}\", {bufferSizeBytes})", () => $"Done; {totalSize:n0} bytes"))
            {
                using (BufferedReader reader = new BufferedReader(File.OpenRead(filePath), buffer))
                {
                    while (!reader.EndOfStream)
                    {
                        reader.EnsureSpace(buffer.Length);
                        reader.Index = reader.Length;
                    }

                    totalSize = reader.BytesRead;
                }
            }
        }

        public static void Read6Bit(string filePath, int bufferSizeBytes)
        {
            ulong[] decoded = new ulong[bufferSizeBytes / 10];
            byte[] buffer = new byte[bufferSizeBytes];
            long totalSize = 0;
            long totalCount = 0;
            ulong total = 0;

            using (new ConsoleWatch($"Read6Bit(\"{filePath}\", {bufferSizeBytes})", () => $"Done; {totalSize:n0} bytes; {totalCount:n0} ints"))
            {
                using (BufferedReader reader = new BufferedReader(File.OpenRead(filePath), buffer))
                {
                    while (!reader.EndOfStream)
                    {
                        int count = NumberConverter.ReadSixBitTerminatedBlock(reader, decoded);
                        for (int i = 0; i < count; ++i)
                        {
                            total += decoded[i];
                        }

                        totalCount += count;
                    }

                    totalSize = reader.BytesRead;
                }
            }
        }

        public static void ReadIntBlock(string filePath, int bufferSizeBytes)
        {
            int[] block;
            byte[] buffer = new byte[bufferSizeBytes];
            long totalSize = 0;
            long totalCount = 0;

            using (new ConsoleWatch($"ReadIntBlock(\"{filePath}\", {bufferSizeBytes})", () => $"Done; {totalSize:n0} bytes"))
            {
                using (BufferedReader bufferedReader = new BufferedReader(File.OpenRead(filePath), buffer))
                using (IntBlockReader reader = new IntBlockReader(bufferedReader))
                {
                    while (true)
                    {
                        int count = reader.Next(out block);
                        for (int i = 0; i < count; ++i)
                        {
                            totalSize += block[i];
                        }

                        if (count == 0) { break; }
                    }
                }
            }

            //for (int j = 0; j < 4; ++j)
            {
                totalSize = 0;
                totalCount = 0;
                using (BufferedReader bufferedReader = BufferedReader.ReadAll(filePath))
                using (new ConsoleWatch($"ReadIntBlock(\"{filePath}\", {bufferSizeBytes})", () => $"[ReadAll]; {totalSize:n0} bytes; {totalCount:n0} ints"))
                {
                    using (IntBlockReader reader = new IntBlockReader(bufferedReader))
                    {
                        while (true)
                        {
                            int count = reader.Next(out block);
                            for (int i = 0; i < count; ++i)
                            {
                                totalSize += block[i];
                            }

                            totalCount += count;
                            if (count == 0) { break; }
                        }
                    }
                }

                totalSize = 0;
                using (BufferedReader bufferedReader = BufferedReader.ReadAll(filePath))
                using (new ConsoleWatch($"ReadIntBlock(\"{filePath}\", {bufferSizeBytes})", () => $"[ReadAll, ForEach]; {totalSize:n0} bytes"))
                {
                    using (IntBlockReader reader = new IntBlockReader(bufferedReader))
                    {
                        foreach (Memory<int> page in reader)
                        {
                            foreach(int value in page.Span)
                            {
                                totalSize += value;
                            }
                        }
                    }
                }
            }
        }

        public static void ArrayTest()
        {
            int count = 268435456;

            int[] array = new int[count];
            for (int i = 0; i < count; ++i)
            {
                array[i] = i;
            }

            long total = 0;

            using (new ConsoleWatch($"For [Array]", () => $"Done with total {total:n0}; {count:n0} ints"))
            {
                total = 0;
                for (int i = 0; i < count; ++i)
                {
                    total += array[i];
                }
            }

            using (new ConsoleWatch($"ForEach [Array]", () => $"Done with total {total:n0}; {count:n0} ints"))
            {
                total = 0;
                foreach (int i in array)
                {
                    total += i;
                }
            }

            using (new ConsoleWatch($"ForEach [MyArray]", () => $"Done with total {total:n0}; {count:n0} ints"))
            {
                total = 0;
                MyArray myArray = new MyArray(array);
                foreach (int i in myArray)
                {
                    total += i;
                }
            }
        }

        public static void TranslateSixBit(string filePath, string outPath)
        {
            int bufferSizeBytes = 65536;
            ulong[] decoded = new ulong[BlockSize];
            byte[] buffer = new byte[bufferSizeBytes];
            long totalSize = 0;

            using (new ConsoleWatch($"Translate(\"{filePath}\", {bufferSizeBytes})", () => $"Done; {totalSize:n0} bytes"))
            {
                using (IntBlockWriter writer = new IntBlockWriter(new BufferedWriter(outPath)))
                using (BufferedReader reader = new BufferedReader(File.OpenRead(filePath), buffer))
                {
                    while (!reader.EndOfStream)
                    {
                        int count = NumberConverter.ReadSixBitTerminatedBlock(reader, decoded);
                        if (count < BlockSize) { Array.Fill<ulong>(decoded, 0, count, BlockSize - count); }

                        for (int i = 0; i < BlockSize; ++i)
                        {
                            writer.Write((int)decoded[i]);
                        }
                    }

                    totalSize = reader.BytesRead;
                }
            }
        }

        public static void TranslateSearchIndex(string filePath, string outPath, bool absolute)
        {
            long[] decoded = new long[BlockSize];
            long intCount = 0;
            long bytesWritten = 0;

            using (new ConsoleWatch($"Translating \"{filePath}\" to block \"{outPath}\"...",
                () => $"{intCount:n0} entries, written to {bytesWritten:n0} bytes ({((float)(8 * bytesWritten) / (float)(intCount)):n2} bits per position)"))
            {
                using (IntBlockWriter writer = new IntBlockWriter(new BufferedWriter(outPath)))
                using (SearchIndexReader reader = new SearchIndexReader(filePath))
                {
                    for (int i = 0; i < reader.WordCount; ++i)
                    {
                        int last = 0;
                        SearchResult result = reader.Find(i);
                        while (!result.Done)
                        {
                            int count = result.Page(ref decoded);
                            for (int j = 0; j < count; ++j)
                            {
                                int current = (int)decoded[j];
                                writer.Write((absolute ? current : current - last));
                                last = current;
                            }

                            intCount += count;
                        }
                    }

                    System.Console.WriteLine(writer.Stats);
                }

                bytesWritten = new FileInfo(outPath).Length;
            }
        }

        public static void TranslateDictionaryPositions(string dictionaryPath, string blockPath, bool absolute)
        {
            int wordCount = 0;
            int totalLength = 0;
            long bytesWritten = 0;

            using (new ConsoleWatch($"Translating {dictionaryPath} positions to {blockPath}...",
                () => $"{wordCount:n0} words, total length {totalLength:n0}, written to {bytesWritten:n0} bytes ({((float)(8 * bytesWritten) / (float)(wordCount)):n2} bits per position)"))
            {
                using (IntBlockWriter writer = new IntBlockWriter(new BufferedWriter(blockPath)))
                using (WordCompressor compressor = WordCompressor.OpenRead(dictionaryPath))
                {
                    wordCount = compressor.WordCount;
                    writer.Write(0);

                    for (int wordIndex = 0; wordIndex < compressor.WordCount; ++wordIndex)
                    {
                        int length = compressor[wordIndex].Word.Length;
                        totalLength += length;

                        writer.Write((absolute ? totalLength : length));
                    }

                    System.Console.WriteLine(writer.Stats);
                }

                bytesWritten = new FileInfo(blockPath).Length;
            }
        }

        public static void DictionaryLengths(string dictionaryPath)
        {
            long total = 0;
            int wordCount = 0;
            int[] countForLength = new int[129];

            using (WordCompressor compressor = WordCompressor.OpenRead(dictionaryPath))
            {
                wordCount = compressor.WordCount;
                for (int wordIndex = 0; wordIndex < compressor.WordCount; ++wordIndex)
                {
                    int length = compressor[wordIndex].Word.Length;
                    total += length;

                    if (length > 128) { length = 128; }
                    countForLength[length]++;
                }
            }

            for (int length = 0; length < countForLength.Length; ++length)
            {
                int count = countForLength[length];
                if (count > 0)
                {
                    System.Console.WriteLine($"{length} => {count:n0}");
                }
            }

            System.Console.WriteLine($"Total {total:n0}b for {wordCount:n0} words. Avg:{((float)total / (float)wordCount):n2} bytes.");
        }

        public static void WriteWordsForLength(string dictionaryPath, int length)
        {
            int countWritten = 0;

            using (WordCompressor compressor = WordCompressor.OpenRead(dictionaryPath))
            {
                for (int wordIndex = 0; wordIndex < compressor.WordCount; ++wordIndex)
                {
                    String8 word = compressor[wordIndex].Word;
                    if (word.Length == length)
                    {
                        System.Console.WriteLine(word);
                        if (++countWritten == 100) { return; }
                    }
                }
            }
        }

        public static void WriteSyntheticBlock(string blockPath, long count)
        {
            int index = 0;
            int[] block = new int[IntBlock.BlockSize];

            long bytesWritten = 0;

            using (new ConsoleWatch($"Writing Synthetic {count:n0} ints to {blockPath}...",
                () => $"{count:n0} values, written as {bytesWritten:n0} bytes ({((float)(8 * bytesWritten) / (float)(count)):n2} bits per value)."))
            {
                using (IntBlockWriter writer = new IntBlockWriter(new BufferedWriter(blockPath)))
                {
                    for (long i = 0; i < count; ++i)
                    {
                        int value = (int)(i & 15);
                        //writer.Write(value);

                        block[index++] = value;
                        if (index == IntBlock.BlockSize)
                        {
                            writer.Write(block, 0, index);
                            index = 0;
                        }
                    }

                    System.Console.WriteLine(writer.Stats);
                }

                bytesWritten = new FileInfo(blockPath).Length;
            }
        }
    }
}
