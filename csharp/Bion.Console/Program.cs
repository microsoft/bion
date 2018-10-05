using Bion.Json;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

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

            //JsonBionConverter.BionToJson(bionLookupPath, Path.ChangeExtension(bionLookupPath, ".json"));

            //ToBion(fromPath, bionPath);
            //ToJson(bionPath, jsonPath);
            //Compare(fromPath, bionPath);

            ReadSpeed(jsonPath);
            for (int i = 0; i < 5; ++i)
            {
                ReadSpeed(bionPath);
            }

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

        private static void ReadSpeed(string filePath)
        {
            Stopwatch w = Stopwatch.StartNew();
            long tokenCount = 0;

            if (filePath.EndsWith(".bion", StringComparison.OrdinalIgnoreCase))
            {
                using (BionReader reader = new BionReader(new BufferedStream(new FileStream(filePath, FileMode.Open))))
                {
                    while (reader.Read())
                    {
                        //if(reader.TokenType == TokenType.PropertyName && reader.CurrentString() == "results")
                        //{
                        //    reader.Skip();
                        //}

                        //object value = null;
                        //switch (reader.TokenType)
                        //{
                        //    case TokenType.PropertyName:
                        //    case TokenType.String:
                        //        value = reader.CurrentString();
                        //        break;
                        //    case TokenType.InlineInteger:
                        //    case TokenType.Integer:
                        //        value = reader.CurrentLong();
                        //        break;
                        //    case TokenType.Float:
                        //        value = reader.CurrentFloat();
                        //        break;
                        //}

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
