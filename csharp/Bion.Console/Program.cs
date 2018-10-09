using Bion.Json;
using Bion.Vector;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
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

            //Stopwatch w = Stopwatch.StartNew();
            //using (JsonTextReader reader = new JsonTextReader(new StreamReader(fromPath)))
            //using (BionLookup lookup = BionLookup.OpenWrite(bionLookupPath))
            //using (BionWriter writer = new BionWriter(new FileStream(bionPath, FileMode.Create), lookup))
            //{
            //    JsonBionConverter.JsonToBion(reader, writer);
            //}

            //w.Stop();
            //System.Console.WriteLine($"Done. Converted {new FileInfo(fromPath).Length / BytesPerMB:n2}MB JSON to {new FileInfo(bionPath).Length / BytesPerMB:n2}MB BION in {w.ElapsedMilliseconds:n0}ms.");


            using (JsonTextReader jsonReader = new JsonTextReader(new StreamReader(fromPath)))
            using (BionLookup lookup = BionLookup.OpenRead(bionLookupPath))
            using (BionReader bionReader = new BionReader(new FileStream(bionPath, FileMode.Open), lookup))
            {
                JsonBionComparer.Compare(jsonReader, bionReader);
            }

            //JsonBionConverter.BionToJson(bionLookupPath, Path.ChangeExtension(bionLookupPath, ".json"));

            //ToBion(fromPath, bionPath);
            //ToJson(bionPath, jsonPath);
            //Compare(fromPath, bionPath);

            //ReadSpeed(jsonPath);
            //for (int i = 0; i < 10; ++i)
            //{
            //    ReadSpeed(bionPath);
            //}

            //for (int i = 0; i < 10; ++i)
            //{
            //    VectorTest(bionPath, false);
            //}

            //JsonStatistics stats = new JsonStatistics(args[0]);
            //System.Console.WriteLine(stats);
        }

        private static void ToBion(string fromPath, string toPath)
        {
            Stopwatch w = Stopwatch.StartNew();
            JsonBionConverter.JsonToBion(fromPath, toPath);
            w.Stop();
            System.Console.WriteLine($"Done. Converted {new FileInfo(fromPath).Length / BytesPerMB:n2}MB JSON to {new FileInfo(toPath).Length / BytesPerMB:n2}MB BION in {w.ElapsedMilliseconds:n0}ms.");
        }

        private static void ToJson(string fromPath, string toPath)
        {
            Stopwatch w = Stopwatch.StartNew();
            JsonBionConverter.BionToJson(fromPath, toPath);
            w.Stop();
            System.Console.WriteLine($"Done. Converted {new FileInfo(fromPath).Length / BytesPerMB:n2}MB BION to {new FileInfo(toPath).Length / BytesPerMB:n2}MB JSON in {w.ElapsedMilliseconds:n0}ms.");
        }

        private static void VectorTest(string filePath, bool readAll)
        {
            Stopwatch w = Stopwatch.StartNew();

            int containerCount = 0;

            long fileLength = new FileInfo(filePath).Length;
            byte[] buffer = new byte[64 * 1024];
            using (Stream stream = File.OpenRead(filePath))
            {
                long lengthDone = 0;
                int bufferLength = stream.Read(buffer);

                while (lengthDone < fileLength)
                {
                    //containerCount += ContainerCount(new Span<byte>(buffer, 0, bufferLength));
                    containerCount += ByteVector.CountGreaterThan(new Span<byte>(buffer, 0, bufferLength), 0xFD);

                    lengthDone += bufferLength;
                    if (readAll) bufferLength = stream.Read(buffer);
                }
            }

            w.Stop();
            System.Console.WriteLine($"Done. VectorTest found {containerCount:n0} containers in {filePath} ({fileLength / BytesPerMB:n2}MB) in {w.ElapsedMilliseconds:n0}ms.");
        }

        private static int ContainerCount(Span<byte> buffer)
        {
            int containerCount = 0;

            for (int i = 0; i < buffer.Length; ++i)
            {
                // Count 0xFE and 0xFF
                if (buffer[i] > 0xFD) containerCount++;
            }

            return containerCount;
        }

        private static void StreamReadSpeed(string filePath)
        {
            int _currentDepth = 0;

            sbyte[] map = Enumerable.Repeat((sbyte)0, 256).ToArray();
            map[(byte)BionToken.StartObject] = 1;
            map[(byte)BionToken.StartArray] = 1;
            map[(byte)BionToken.EndObject] = -1;
            map[(byte)BionToken.EndArray] = -1;

            Stopwatch w = Stopwatch.StartNew();
            byte[] buffer = new byte[512 * 1024];
            using (Stream stream = File.OpenRead(filePath))
            {
                int length = stream.Read(buffer);

                long fileLength = new FileInfo(filePath).Length;
                long lengthDone = 0;
                while (lengthDone < fileLength)
                {
                    //int length = stream.Read(buffer);
                    //_currentDepth += ContainerCount(new Span<byte>(buffer, 0, length));
                    //currentDepth += ByteVector.CountGreaterThan(new Span<byte>(buffer, 0, length), 0xFD);

                    for (int i = 0; i < length; ++i)
                    {
                        byte marker = buffer[i];

                        _currentDepth += map[marker];

                        //if (marker >= 0xFE)
                        //{
                        //    _currentDepth++;
                        //}

                        //if (marker >= 0xFC)
                        //{
                        //    switch (marker)
                        //    {
                        //        case 0xFF:
                        //        case 0xFE:
                        //            _currentDepth++;
                        //            break;
                        //        case 0xFD:
                        //        case 0xFC:
                        //            _currentDepth--;
                        //            break;
                        //    }
                        //}
                    }
                    lengthDone += length;
                }
            }

            w.Stop();
            System.Console.WriteLine($"Done. Read {filePath} (bytes only) ({new FileInfo(filePath).Length / BytesPerMB:n2}MB) [{_currentDepth:n0}] in {w.ElapsedMilliseconds:n0}ms.");
        }

        private static void ReadSpeed(string filePath)
        {
            Stopwatch w = Stopwatch.StartNew();
            long tokenCount = 0;

            if (filePath.EndsWith(".bion", StringComparison.OrdinalIgnoreCase))
            {
                using (BionReader reader = new BionReader(new FileStream(filePath, FileMode.Open)))
                {
                    //reader.Skip();

                    while (reader.Read())
                    {
                        tokenCount++;
                        //if (reader.TokenType == BionToken.StartArray || reader.TokenType == BionToken.StartObject) tokenCount++;
                        //if (reader.TokenType == BionToken.EndArray || reader.TokenType == BionToken.EndObject) tokenCount--;
                    }
                    //    //if(reader.TokenType == TokenType.PropertyName && reader.CurrentString() == "results")
                    //    //{
                    //    //    reader.Skip();
                    //    //}

                    //    //object value = null;
                    //    //switch (reader.TokenType)
                    //    //{
                    //    //    case BionToken.PropertyName:
                    //    //    case BionToken.String:
                    //    //        value = reader.CurrentString();
                    //    //        break;
                    //    //    case BionToken.Integer:
                    //    //        value = reader.CurrentInteger();
                    //    //        break;
                    //    //    case BionToken.Float:
                    //    //        value = reader.CurrentFloat();
                    //    //        break;
                    //    //}

                    //    tokenCount++;
                    //}
                    //tokenCount = reader.BytesRead;

                }
            }
            else
            {
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(filePath)))
                {
                    while (reader.Read())
                    {
                        //if (reader.TokenType == JsonToken.PropertyName && (string)reader.Value == "results")
                        //{
                        //    reader.Skip();
                        //}

                        tokenCount++;
                    }
                }
            }

            w.Stop();
            System.Console.WriteLine($"Done. Read {filePath} ({new FileInfo(filePath).Length / BytesPerMB:n2}MB; {tokenCount:n0} tokens) in {w.ElapsedMilliseconds:n0}ms.");
        }

        private static void Compare(string jsonPath, string BionPath)
        {
            Stopwatch w = Stopwatch.StartNew();
            JsonBionComparer.Compare(jsonPath, BionPath);
            w.Stop();
            System.Console.WriteLine($"Done. Compared {new FileInfo(jsonPath).Length / BytesPerMB:n2}MB JSON to {new FileInfo(BionPath).Length / BytesPerMB:n2}MB BION in {w.ElapsedMilliseconds:n0}ms.");
        }

        private static void RemoveWhitespace(string fromPath, string toPath)
        {
            Stopwatch w = Stopwatch.StartNew();

            using (JsonTextReader reader = new JsonTextReader(new StreamReader(fromPath)))
            using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(toPath)))
            {
                writer.Formatting = Formatting.None;
                writer.WriteToken(reader);
            }

            w.Stop();
            System.Console.WriteLine($"Done. Converted {new FileInfo(fromPath).Length / BytesPerMB:n2}MB JSON to {new FileInfo(toPath).Length / BytesPerMB:n2}MB JSON [no whitespace] in {w.ElapsedMilliseconds:n0}ms.");
        }

        private const float BytesPerMB = 1024 * 1024;
    }
}
