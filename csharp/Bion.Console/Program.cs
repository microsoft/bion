﻿using Bion.Core;
using Bion.IO;
using Bion.Json;
using Bion.Text;
using Bion.Vector;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

namespace Bion.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string fromPath = args[0];
            string jsonPath = Path.ChangeExtension(fromPath, ".json");
            string bionPath = Path.ChangeExtension(fromPath, ".bion");
            string bionLookupPath = Path.ChangeExtension(fromPath, ".lookup.bion");

            //CompressTest(fromPath, bionPath);
            ConvertAndBackTest(fromPath, jsonPath, bionPath);

            //ReadSpeed(jsonPath);
            for (int i = 0; i < 10; ++i)
            {
                ReadSpeed(bionPath);
            }

            //for (int i = 0; i < 10; ++i)
            //{
            //    VectorTest(bionPath, false);
            //}

            //JsonStatistics stats = new JsonStatistics(args[0]);
            //System.Console.WriteLine(stats);
        }

        private static void ToBion(string fromPath, string toPath)
        {
            using (new ConsoleWatch($"Converting {fromPath} to {toPath}...",
                () => $"Done. {LengthMB(fromPath)} JSON to {LengthMB(toPath)} BION"))
            {
                JsonBionConverter.JsonToBion(fromPath, toPath);
            }
        }

        private static void ToJson(string fromPath, string toPath)
        {
            using (new ConsoleWatch($"Converting {fromPath} to {toPath}...",
                () => $"Done. {LengthMB(fromPath)} BION to {LengthMB(toPath)} JSON"))
            {
                JsonBionConverter.BionToJson(fromPath, toPath);
            }
        }

        private static void Compare(string jsonPath, string bionPath)
        {
            using (new ConsoleWatch($"Comparing {jsonPath} to {bionPath}..."))
            {
                JsonBionComparer.Compare(jsonPath, bionPath);
            }
        }

        private static bool CompareBytes(string expectedPath, string actualPath)
        {
            using (new ConsoleWatch($"Comparing {expectedPath} to {actualPath}..."))
            {
                Span<byte> expected = new byte[64 * 1024];
                Span<byte> actual = new byte[64 * 1024];

                long position = 0;
                using (FileStream expectedReader = File.OpenRead(expectedPath))
                using (FileStream actualReader = File.OpenRead(actualPath))
                {
                    while (true)
                    {
                        int expectedLength = expectedReader.Read(expected);
                        int actualLength = actualReader.Read(actual);
                        if (expectedLength != actualLength)
                        {
                            System.Console.WriteLine($"@{position:n0}, expect returned {expectedLength:n0} bytes\r\nactual returned {actualLength:n0}bytes.");
                            return false;
                        }

                        for (int i = 0; i < expectedLength; ++i)
                        {
                            if (expected[i] != actual[i])
                            {
                                System.Console.WriteLine($"@{position + i:n0};\r\nexpect: {expected[i]},\r\nactual: {actual[i]}.");
                                return false;
                            }
                        }

                        position += expectedLength;
                        if (expectedLength < expected.Length) break;
                    }

                    System.Console.WriteLine("Files Identical.");
                }
            }

            return true;
        }

        private static void RemoveWhitespace(string fromPath, string toPath)
        {
            using (new ConsoleWatch($"Writing {fromPath} without whitespace to {toPath}...",
                () => $"Done; {LengthMB(fromPath)} => {LengthMB(toPath)}"))
            {
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(fromPath)))
                using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(toPath)))
                {
                    writer.Formatting = Formatting.None;
                    writer.WriteToken(reader);
                }
            }
        }

        private static void CompressTest(string fromPath, string toPath)
        {
            byte[] readBuffer = new byte[64 * 1024];
            byte[] writeBuffer = new byte[64 * 1024];

            string dictionaryPath = Path.ChangeExtension(toPath, ".Dictionary.bion");
            string comparePath = Path.ChangeExtension(fromPath, "out.json");

            string nowsPath = Path.ChangeExtension(fromPath, ".nows.json");
            if (!File.Exists(nowsPath))
            {
                RemoveWhitespace(fromPath, nowsPath);
            }

            fromPath = nowsPath;

            using (new ConsoleWatch($"Compressing {fromPath}...",
                () => $"Done. {LengthMB(fromPath)} to {LengthMB(toPath)} + {LengthMB(dictionaryPath)} index"))
            {
                using (WordCompressor compressor = WordCompressor.OpenWrite(dictionaryPath))
                {
                    using (BufferedReader reader = new BufferedReader(File.OpenRead(fromPath), readBuffer))
                    using (BufferedWriter writer = new BufferedWriter(File.Create(toPath), writeBuffer))
                    {
                        compressor.Compress(reader, writer);
                    }

                    string tempPath = Path.ChangeExtension(toPath, ".opt.bion");
                    using (BufferedReader reader = new BufferedReader(File.OpenRead(toPath), readBuffer))
                    using (BufferedWriter writer = new BufferedWriter(File.Create(tempPath), writeBuffer))
                    {
                        compressor.Optimize(reader, writer);
                    }

                    toPath = tempPath;
                }
            }

            JsonBionConverter.BionToJson(dictionaryPath, Path.ChangeExtension(dictionaryPath, ".json"));

            int iterations = 1;
            using (new ConsoleWatch($"Decompressing {fromPath} {iterations:n0}x..."))
            {
                for (int i = 0; i < iterations; ++i)
                {
                    using (BufferedReader reader = new BufferedReader(File.OpenRead(toPath), readBuffer))
                    using (BufferedWriter writer = new BufferedWriter(File.Create(comparePath), writeBuffer))
                    using (WordCompressor compressor = WordCompressor.OpenRead(dictionaryPath))
                    {
                        compressor.Decompress(reader, writer);
                    }
                }
            }

            // Verify files identical
            CompareBytes(fromPath, comparePath);
        }

        private static void ConvertAndBackTest(string fromPath, string jsonPath, string bionPath)
        {
            ToBion(fromPath, bionPath);
            ToJson(bionPath, jsonPath);
            Compare(fromPath, bionPath);
        }

        private static void ReadSpeed(string filePath)
        {
            long tokenCount = 0;

            using (new ConsoleWatch($"Reading {filePath} ({LengthMB(filePath)})...",
                () => $"Done ({tokenCount:n0})"))
            {
                if (filePath.EndsWith(".bion", StringComparison.OrdinalIgnoreCase))
                {
                    using (BionReader reader = new BionReader(new FileStream(filePath, FileMode.Open)))
                    {
                        while (reader.Read())
                        {
                            tokenCount++;
                        }
                    }
                }
                else
                {
                    using (JsonTextReader reader = new JsonTextReader(new StreamReader(filePath)))
                    {
                        while (reader.Read())
                        {
                            tokenCount++;
                        }
                    }
                }
            }
        }

        private static void SkipSpeed(string filePath)
        {
            long tokenCount = 0;

            using (new ConsoleWatch($"Reading {filePath} ({LengthMB(filePath)})...",
                () => $"Done ({tokenCount:n0})"))
            {
                if (filePath.EndsWith(".bion", StringComparison.OrdinalIgnoreCase))
                {
                    using (BionReader reader = new BionReader(new FileStream(filePath, FileMode.Open)))
                    {
                        reader.Skip();
                        tokenCount = reader.BytesRead;
                    }
                }
                else
                {
                    using (JsonTextReader reader = new JsonTextReader(new StreamReader(filePath)))
                    {
                        reader.Read();
                        reader.Skip();
                    }
                }
            }
        }

        private static void ConvertFilesToArray(string fromPath, string toPath)
        {
            using (JsonTextReader reader = new JsonTextReader(new StreamReader(fromPath)))
            using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(toPath)))
            {
                writer.Formatting = Formatting.Indented;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName && ((string)reader.Value) == "files")
                    {
                        writer.WritePropertyName((string)reader.Value);

                        // StartObject
                        reader.Read();
                        writer.WriteStartArray();

                        while (true)
                        {
                            // Name
                            reader.Read();
                            if (reader.TokenType == JsonToken.EndObject) break;

                            // Value
                            reader.Read();
                            writer.WriteToken(reader);
                        }

                        writer.WriteEndArray();
                    }
                    else
                    {
                        switch (reader.TokenType)
                        {
                            case JsonToken.StartObject:
                                writer.WriteStartObject();
                                break;
                            case JsonToken.StartArray:
                                writer.WriteStartArray();
                                break;
                            case JsonToken.EndObject:
                                writer.WriteEndObject();
                                break;
                            case JsonToken.EndArray:
                                writer.WriteEndArray();
                                break;
                            case JsonToken.PropertyName:
                                writer.WritePropertyName((string)reader.Value);
                                break;
                            case JsonToken.String:
                                string value = (string)reader.Value;
                                writer.WriteValue(value);
                                break;
                            case JsonToken.Integer:
                                writer.WriteValue((long)reader.Value);
                                break;
                            case JsonToken.Boolean:
                                writer.WriteValue((bool)reader.Value);
                                break;
                            case JsonToken.Null:
                                writer.WriteNull();
                                break;
                            case JsonToken.Float:
                                writer.WriteValue((double)reader.Value);
                                break;
                            case JsonToken.Date:
                                writer.WriteValue((DateTime)reader.Value);
                                break;
                        }
                    }
                }
            }
        }

        private static string LengthMB(string filePath)
        {
            return $"{new FileInfo(filePath).Length / BytesPerMB:n2}MB";
        }

        private const float BytesPerMB = 1024 * 1024;
    }
}
