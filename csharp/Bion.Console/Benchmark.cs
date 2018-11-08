using Bion.Core;
using Bion.IO;
using System;
using System.IO;

namespace Bion.Console
{
    public static class Benchmark
    {
        public const int BlockSize = 16;

        public static void Test()
        {
            string filePath = @"C:\Download\Sarif\Out\SarifSearch.All.cmp";
            string blockPath = @"C:\Download\Sarif\Out\SarifSearch.All.blk";
            int bufferSize = 64 * 1024;

            //ReadBytes(filePath, bufferSize);
            //ReadBufferedReader(filePath, bufferSize);
            //Read6Bit(filePath, bufferSize);

            TranslateSixBit(filePath, blockPath);
            //ReadNumberBlock(blockPath, bufferSize);

            TranslateSearchIndex(@"C:\Download\Sarif\Out\SarifSearch.All.idx", @"C:\Download\Sarif\Out\SarifSearch.All.idx.blk");
            //ReadNumberBlock(@"C:\Download\Sarif\Out\SarifSearch.All.idx.blk", bufferSize);
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


        public static void ReadNumberBlock(string filePath, int bufferSizeBytes)
        {
            int[] block;
            byte[] buffer = new byte[bufferSizeBytes];
            long totalSize = 0;

            using (new ConsoleWatch($"ReadNumberBlock(\"{filePath}\", {bufferSizeBytes})", () => $"Done; {totalSize:n0} bytes"))
            {
                using (BufferedReader bufferedReader = new BufferedReader(File.OpenRead(filePath), buffer))
                using (NumberBlockReader reader = new NumberBlockReader(bufferedReader, BlockSize))
                {
                    while(reader.ReadBlock(out block))
                    {
                        // Nothing
                    }

                    totalSize = bufferedReader.BytesRead;
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

        public static void TranslateSearchIndex(string filePath, string outPath)
        {
            long[] decoded = new long[BlockSize];

            using (new ConsoleWatch($"TranslateSearchIndex(\"{filePath}\", \"{outPath}\")"))
            {
                using (NumberBlockWriter writer = new NumberBlockWriter(new BufferedWriter(File.Create(outPath)), BlockSize))
                using (SearchIndexReader reader = new SearchIndexReader(filePath))
                {
                    for(int i = 0; i < reader.WordCount; ++i)
                    {
                        SearchResult result = reader.Find(i);
                        while(!result.Done)
                        {
                            int count = result.Page(ref decoded);
                            for(int j = 0; j < count; ++j)
                            {
                                writer.Write((int)decoded[j]);
                            }
                        }
                    }
                }
            }
        }
    }
}
