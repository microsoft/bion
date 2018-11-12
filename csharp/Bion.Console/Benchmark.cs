using Bion.Core;
using Bion.IO;
using Bion.Text;
using Bion.Vector;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Bion.Console
{
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

            //DictionaryLengths(dictionaryPath);
            //WriteWordsForLength(dictionaryPath, 52);

            //ReadBytes(compressedPath, bufferSize);
            //ReadBufferedReader(compressedPath, bufferSize);
            //Read6Bit(compressedPath, bufferSize);

            //VectorTest(compressedPath);

            //TranslateSixBit(compressedPath, compressedPath + ".blk");
            //ReadIntBlock(compressedPath + ".blk", bufferSize);

            TranslateSearchIndex(searchIndexPath, searchIndexPath + ".blk", true);
            //ReadIntBlock(searchIndexPath + ".blk", bufferSize);

            TranslateDictionaryPositions(dictionaryPath, dictionaryPath + ".blk", true);
            //ReadIntBlock(dictionaryPath + ".blk", bufferSize);

            //WriteSyntheticBlock(syntheticBlockPath, 256 * 1024 * 1024);
            //ReadIntBlock(syntheticBlockPath, bufferSize);
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
                        if (length < buffer.Length) break;
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

            using (new ConsoleWatch($"Read6Bit(\"{filePath}\", {bufferSizeBytes})", () => $"Done; {totalSize:n0} bytes"))
            {
                using (BufferedReader reader = new BufferedReader(File.OpenRead(filePath), buffer))
                {
                    while (!reader.EndOfStream)
                    {
                        NumberConverter.ReadSixBitTerminatedBlock(reader, decoded);
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

            using (new ConsoleWatch($"ReadIntBlock(\"{filePath}\", {bufferSizeBytes})", () => $"Done; {totalSize:n0} bytes"))
            {
                using (BufferedReader bufferedReader = new BufferedReader(File.OpenRead(filePath), buffer))
                using (IntBlockReader reader = new IntBlockReader(bufferedReader))
                {
                    while (true)
                    {
                        int count = reader.Next(out block);
                        totalSize += 4 * count;
                        if (count < IntBlock.BlockSize) { break; }
                    }
                }
            }

            totalSize = 0;
            using (BufferedReader bufferedReader = BufferedReader.ReadAll(filePath))
            using (new ConsoleWatch($"ReadIntBlock(\"{filePath}\", {bufferSizeBytes})", () => $"[ReadAll]; {totalSize:n0} bytes"))
            {
                using (IntBlockReader reader = new IntBlockReader(bufferedReader))
                {
                    while (true)
                    {
                        int count = reader.Next(out block);
                        totalSize += 4 * count;
                        if (count < IntBlock.BlockSize) { break; }
                    }
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
                using (NumberBlockWriter writer = new NumberBlockWriter(new BufferedWriter(File.Create(outPath)), BlockSize))
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

            using(new ConsoleWatch($"Translating \"{filePath}\" to block \"{outPath}\"...",
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

                    if (length > 128) length = 128;
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
                        if (++countWritten == 100) return;
                    }
                }
            }
        }

        private static byte[] Lengths = { 4, 8, 12, 16 };
        private static sbyte[] ShuffleMasks =
        {
            -1, -1, -1,  0, -1, -1, -1,  1, -1, -1, -1,  2, -1, -1, -1,  3,         // One Byte 
            -1, -1,  0,  1, -1, -1,  2,  3, -1, -1,  4,  5, -1, -1,  6,  7,         // Two Bytes
            -1,  0,  1,  2, -1,  3,  4,  5, -1,  6,  7,  8, -1,  9, 10, 11,         // Three Bytes
             0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14, 15          // Four Bytes
        };

        private static int[] MultiplyMasks =
        {
            1, 1, 1, 1,     // No multiply shift
            1, 1, 1, 1,     // No multiply shift
            1, 1, 1, 1,     // No multiply shift
            1, 1, 1, 1,     // No multiply shift
        };

        public static void VectorTest(string filePath)
        {
            long decodedBytes = 0;
            int[] result = new int[128];

            using (new ConsoleWatch($"VectorTest(\"{filePath}\")", () => $"Done; Decoded to {decodedBytes:n0} bytes"))
            {
                using (BufferedReader reader = new BufferedReader(File.OpenRead(filePath)))
                {
                    while (!reader.EndOfStream)
                    {
                        int count = DecodeBlock(reader, result);
                        decodedBytes += count * 4;
                    }
                }
            }

            using (BufferedReader reader = BufferedReader.ReadAll(filePath))
            {
                using (new ConsoleWatch($"VectorTest(\"{filePath}\")", () => $"RAM Done; Decoded to {decodedBytes:n0} bytes"))
                {
                    while (!reader.EndOfStream)
                    {
                        int count = DecodeBlock(reader, result);
                        decodedBytes += count * 4;
                    }
                }
            }
        }

        public unsafe static int DecodeBlock(BufferedReader reader, int[] result)
        {
            reader.EnsureSpace(1024);
            if (reader.Length - reader.Index < 256)
            {
                reader.Index = reader.Length;
                return 0;
            }

            fixed (int* resultPtr = result)
            fixed (sbyte* maskPtr = ShuffleMasks)
            fixed (int* multiplyPtr = MultiplyMasks)
            fixed (byte* bufferPtr = reader.Buffer)
            {
                int index = reader.Index;

                // Read control byte
                byte controlByte = (byte)(bufferPtr[index++] & 0x3);

                // Lookup length and mask
                byte length = Lengths[controlByte];
                Vector128<sbyte> shuffleMask = Unsafe.ReadUnaligned<Vector128<sbyte>>(&maskPtr[controlByte]);

                Vector128<int> multiplyBy = Unsafe.ReadUnaligned<Vector128<int>>(&maskPtr[controlByte]);

                //Vector128<int> addValue = Sse.Set1<int>(index);
                int* addValue = stackalloc int[4];
                for (int i = 0; i < 4; ++i)
                {
                    addValue[i] = index;
                }
                Vector128<int> addValueV = Unsafe.ReadUnaligned<Vector128<int>>(addValue);

                for (int i = 0; i < 128; i += 4)
                {
                    // Read source bytes
                    Vector128<sbyte> data = Unsafe.ReadUnaligned<Vector128<sbyte>>(&bufferPtr[index]);
                    index += length;

                    Vector128<int> vector;

                    // Shuffle to get the right bytes in each integer
                    vector = Sse.StaticCast<sbyte, int>(Ssse3.Shuffle(data, shuffleMask));

                    // Multiply to shift each int so the desired bits are at the top
                    vector = Sse41.MultiplyLow(vector, multiplyBy);

                    // Shift the desired bits to the bottom and zero the top
                    vector = Sse2.ShiftRightLogical(vector, 16);

                    // Add the delta base value
                    vector = Sse2.Add(vector, addValueV);

                    // Write the decoded integers
                    Unsafe.WriteUnaligned(&resultPtr[i], vector);
                }

                reader.Index = index;
            }

            return 128;
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
