using Bion.Core;
using Bion.IO;
using Bion.Json;
using Bion.Text;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace Bion.Console
{
    internal class Program
    {
        private const string Usage =
@"Usage: Bion.Console <mode> <args>
    Bion.Console toBion <fromJsonPath> <toBionPath> <toDictionaryPath?>
      Convert a JSON file to the BION equivalent. Pass dictionary path to use compression.

    Bion.Console toJson <fromBionPath> <toJsonPath> <fromDictionaryPath?>
      Convert a BION file to the JSON equivalent. Pass dictionary if BION was compressed.

    Bion.Console compare <expectedPath> <actualPath>
      Compare files. Use to verify BION conversions and compression.

    Bion.Console nows <fromJsonPath> <toJsonPath>
      Rewrite the given JSON file without indenting whitespace.

    Bion.Console compress <sourcePath> <compressedPath> <toDictionaryPath>
      Compress the source file using word compression without any format assumptions.

    Bion.Console expand <compressedPath> <outputPath> <fromDictionaryPath>
      Expand a word compressed file to the output path.

    Bion.Console count <bionOrJsonPath>
      Count tokens in file; used to show read speed.
";

        private static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.WriteLine(Usage);
                return -2;
            }

            string mode = args[0].ToLowerInvariant();
            try
            {
                switch (mode)
                {
                    case "tobion":
                        if (args.Length < 3) { throw new UsageException("toBion requires a json input file path and a bion output file path."); }
                        ToBion(args[1], args[2], (args.Length > 3 ? args[3] : null));
                        break;
                    case "tojson":
                        if (args.Length < 3) { throw new UsageException("fromBion requires a bion input file path and a json output file path."); }
                        ToJson(args[1], args[2], (args.Length > 3 ? args[3] : null));
                        break;
                    case "nows":
                        if (args.Length < 3) { throw new UsageException("nows requires json input and output file paths."); }
                        NoWhitespace(args[1], args[2]);
                        break;
                    case "compress":
                        if (args.Length < 4) { throw new UsageException("compress requires an input, output, and dictionary path."); }
                        Compress(args[1], args[2], args[3]);
                        break;
                    case "expand":
                        if (args.Length < 4) { throw new UsageException("expand requires an input, output, and dictionary path."); }
                        Expand(args[1], args[2], args[3]);
                        break;
                    case "count":
                        if (args.Length < 2) { throw new UsageException("count requires an input path."); }
                        Count(args[1]);
                        break;
                    case "compare":
                        if (args.Length < 3) { throw new UsageException("compare requires expected and actual file paths."); }
                        return Compare(args[1], args[2]);
                    case "roundtrip":
                        if (args.Length < 2) { throw new UsageException("roundtrip requires a json input file path and a bion output file path."); }
                        string jsonPath = args[1];
                        string bionPath = (args.Length > 2 ? args[2] : Path.ChangeExtension(jsonPath, ".bion"));
                        string bionDictPath = (args.Length > 3 ? args[3] : Path.ChangeExtension(jsonPath, ".dict.bion"));
                        string comparePath = (args.Length > 4 ? args[4] : Path.ChangeExtension(jsonPath, ".compare.json"));

                        ToBion(jsonPath, bionPath, bionDictPath);
                        ToJson(bionPath, comparePath, bionDictPath);
                        return Compare(jsonPath, comparePath);
                    default:
                        throw new UsageException($"Unknown mode '{mode}'. Run without arguments for usage.");
                }

                return 0;
            }
            catch (UsageException ex)
            { 
                System.Console.Write(ex.Message);
                return -2;
            }
            catch (Exception ex) when (!Debugger.IsAttached)
            {
                System.Console.WriteLine("ERROR: " + ex.ToString());
                return -1;
            }
        }

        private static void ToBion(string jsonPath, string bionPath, string dictionaryPath)
        {
            VerifyFileExists(jsonPath);

            using (new ConsoleWatch($"Converting {jsonPath} to {bionPath}...",
                () => $"Done. {FileLength.MB(jsonPath)} JSON to {FileLength.MB(bionPath)} BION{(dictionaryPath == null ? "" : $" + {FileLength.MB(dictionaryPath)} dictionary")} ({FileLength.Percentage(jsonPath, bionPath, dictionaryPath)})"))
            {
                JsonBionConverter.JsonToBion(jsonPath, bionPath, dictionaryPath);
            }
        }

        private static void ToJson(string fromPath, string toPath, string dictionaryPath)
        {
            VerifyFileExists(fromPath);
            VerifyFileExists(dictionaryPath);

            using (new ConsoleWatch($"Converting {fromPath} to {toPath}...",
                () => $"Done. {FileLength.MB(fromPath)} BION to {FileLength.MB(toPath)} JSON"))
            {
                JsonBionConverter.BionToJson(fromPath, toPath, dictionaryPath);
            }
        }

        private static void NoWhitespace(string fromJsonPath, string toJsonPath)
        {
            VerifyFileExists(fromJsonPath);

            using (new ConsoleWatch($"Writing {fromJsonPath} without whitespace to {toJsonPath}...",
                () => $"Done; {FileLength.MB(fromJsonPath)} => {FileLength.MB(toJsonPath)} ({FileLength.Percentage(fromJsonPath, toJsonPath)})"))
            {
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(fromJsonPath)))
                using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(toJsonPath)))
                {
                    writer.Formatting = Formatting.None;
                    writer.WriteToken(reader);
                }
            }
        }

        private static void Compress(string fromPath, string toPath, string toDictionaryPath)
        {
            VerifyFileExists(fromPath);

            using (new ConsoleWatch($"Compressing {fromPath}...",
                () => $"Done. {FileLength.MB(fromPath)} to {FileLength.MB(toPath)} + {FileLength.MB(toDictionaryPath)} dictionary ({FileLength.Percentage(fromPath, toPath, toDictionaryPath)})"))
            {
                WordCompressor.Compress(fromPath, toPath, toDictionaryPath);
            }
        }

        private static void Expand(string fromPath, string toPath, string fromDictionaryPath)
        {
            VerifyFileExists(fromPath);
            VerifyFileExists(fromDictionaryPath);

            using (new ConsoleWatch($"Expanding {fromPath}...",
                () => $"Done. {FileLength.MB(fromPath)} + {FileLength.MB(fromDictionaryPath)} dictionary to {FileLength.MB(toPath)}"))
            {
                WordCompressor.Expand(fromPath, toPath, fromDictionaryPath);
            }
        }

        private static int Compare(string expectedPath, string actualPath)
        {
            VerifyFileExists(expectedPath);
            VerifyFileExists(actualPath);

            string error = null;

            using (new ConsoleWatch($"Comparing {expectedPath} to {actualPath}...",
                () => $"Done; {(error == null ? "files identical" : error)}"))
            {
                if (Path.GetExtension(expectedPath).Equals("json", StringComparison.OrdinalIgnoreCase)
                && Path.GetExtension(actualPath).Equals("json", StringComparison.OrdinalIgnoreCase))
                {
                    error = FileComparer.JsonEqual(expectedPath, actualPath);
                }
                else
                {
                    error = FileComparer.BinaryEqual(expectedPath, actualPath);
                }
            }

            return (error == null ? 0 : -3);
        }

        private static void Count(string filePath)
        {
            VerifyFileExists(filePath);
            long tokenCount = 0;

            using (new ConsoleWatch($"Reading {filePath} ({FileLength.MB(filePath)})...",
                () => $"Done; {tokenCount:n0} tokens found in file"))
            {
                if (filePath.EndsWith(".bion", StringComparison.OrdinalIgnoreCase))
                {
                    using (BionReader reader = new BionReader(File.OpenRead(filePath)))
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

        private static string VerifyFileExists(string filePath)
        {
            if (filePath != null && !File.Exists(filePath)) throw new UsageException($"File {filePath} not found.");
            return filePath;
        }
    }

    [Serializable]
    public class UsageException : Exception
    {
        public UsageException() { }
        public UsageException(string message) : base(message) { }
        public UsageException(string message, Exception inner) : base(message, inner) { }
        protected UsageException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
