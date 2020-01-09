using Bion.Core;
using Bion.IO;
using Bion.Json;
using Bion.Text;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

    Bion.Console addws <fromJsonPath> <toJsonPath>
      Rewrite the given JSON file with indenting and whitespace.

    Bion.Console compress <sourcePath> <compressedPath> <toDictionaryPath>
      Compress the source file using word compression without any format assumptions.

    Bion.Console expand <compressedPath> <outputPath> <fromDictionaryPath>
      Expand a word compressed file to the output path.

    Bion.Console read <bionOrJsonPath>
      Test read performance; read all, count tokens only, and skip everything.
";

        private static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                System.Console.WriteLine(Usage);
                return -2;
            }

            string fromPath;
            string mode = args[0].ToLowerInvariant();
            try
            {
                switch (mode)
                {
                    case "tobion":
                        if (args.Length < 2) { throw new UsageException("toBion requires a json input file path."); }
                        fromPath = args[1];
                        ToBion(fromPath,
                            OrDefault(args, 2, ChangePath(fromPath, ".bion", "Out")),
                            (args.Length > 3 ? args[3] : null));
                        break;

                    case "tojson":
                        if (args.Length < 3) { throw new UsageException("fromBion requires a bion input file path and a json output file path."); }
                        ToJson(args[1], args[2], (args.Length > 3 ? args[3] : null));
                        break;

                    case "nows":
                        if (args.Length < 2) { throw new UsageException("nows requires a json input path."); }
                        NoWhitespace(args[1], OrDefault(args, 2, ChangePath(args[1], ".nows.json")));
                        break;

                    case "addws":
                        if (args.Length < 2) { throw new UsageException("addws requires a json input path."); }
                        AddWhitespace(args[1], OrDefault(args, 2, ChangePath(args[1], ".addws.json")));
                        break;

                    case "compress":
                        if (args.Length < 2) { throw new UsageException("compress requires an input path."); }
                        fromPath = args[1];
                        Compress(fromPath,
                            OrDefault(args, 2, ChangePath(fromPath, ".cmp", "Out")),
                            OrDefault(args, 3, ChangePath(fromPath, ".dict.cmp", "Out")));
                        break;

                    case "expand":
                        if (args.Length < 4) { throw new UsageException("expand requires an input, output, and dictionary path."); }
                        Expand(args[1], args[2], args[3]);
                        break;

                    case "read":
                        if (args.Length < 2) { throw new UsageException("count requires an input path."); }
                        Read(args[1], (args.Length > 2 ? args[2] : null));
                        Count(args[1], (args.Length > 2 ? args[2] : null));
                        Skip(args[1], (args.Length > 2 ? args[2] : null));
                        break;

                    case "compare":
                        if (args.Length < 3) { throw new UsageException("compare requires expected and actual file paths."); }
                        return Compare(args[1], args[2]);

                    case "roundtrip":
                        if (args.Length < 2) { throw new UsageException("roundtrip requires a json input file path and a bion output file path."); }
                        fromPath = args[1];
                        string bionPath = OrDefault(args, 2, ChangePath(fromPath, ".bion", "Out"));
                        string bionDictPath = OrDefault(args, 3, ChangePath(fromPath, ".wdx", "Out"));
                        string comparePath = OrDefault(args, 4, ChangePath(fromPath, ".compare.json", "Out"));

                        ToBion(fromPath, bionPath, bionDictPath);
                        ToJson(bionPath, comparePath, bionDictPath);
                        return Compare(fromPath, comparePath);

                    case "search":
                        if (args.Length < 3) { throw new UsageException("search requires a bion file path and search string."); }
                        Search(args[1], args[2]);
                        break;

                    case "index":
                        if (args.Length < 3) { throw new UsageException("index requires an input root path and output root path."); }
                        Index(args[1], args[2]);
                        break;

                    case "searchall":
                        if (args.Length < 2) { throw new UsageException("searchAll requires a bion root path."); }
                        SearchAll(args[1]);
                        break;

                    case "test":
                        Benchmark.Test();
                        break;

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
                () => $"Done. {FileLength.MB(jsonPath)} JSON to {FileLength.MB(bionPath)} BION{(String.IsNullOrEmpty(dictionaryPath) ? "" : $" + {FileLength.MB(dictionaryPath)} dictionary")} ({FileLength.Percentage(jsonPath, bionPath, dictionaryPath)})"))
            {
                JsonBionConverter.JsonToBion(jsonPath, bionPath, dictionaryPath);
            }
        }

        private static void Index(string jsonRootPath, string bionRootPath)
        {
            using (new ConsoleWatch($"Converting under \"{jsonRootPath}\" to \"{bionRootPath}\"..."))
            {
                jsonRootPath = Path.GetFullPath(jsonRootPath);

                foreach (string jsonFilePath in Directory.EnumerateFiles(jsonRootPath, "*.*", SearchOption.AllDirectories))
                {
                    string jsonFilePathUnderRoot = jsonFilePath.Substring(jsonRootPath.Length + 1);
                    string outputPath = Path.ChangeExtension(Path.Combine(bionRootPath, jsonFilePathUnderRoot), ".bion");
                    if (!File.Exists(outputPath))
                    {
                        System.Console.WriteLine($"  {jsonFilePathUnderRoot}");
                        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
                        JsonBionConverter.JsonToBion(jsonFilePath, outputPath, Path.ChangeExtension(outputPath, ".wdx"));
                    }
                }
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

        private static void AddWhitespace(string fromJsonPath, string toJsonPath)
        {
            VerifyFileExists(fromJsonPath);

            using (new ConsoleWatch($"Writing {fromJsonPath} with formatting to {toJsonPath}...",
                () => $"Done; {FileLength.MB(fromJsonPath)} => {FileLength.MB(toJsonPath)} ({FileLength.Percentage(fromJsonPath, toJsonPath)})"))
            {
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(fromJsonPath)))
                using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(toJsonPath)))
                {
                    writer.Formatting = Formatting.Indented;
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
                () => $"Done; {(error ?? "files identical")}"))
            {
                if (Path.GetExtension(expectedPath).Equals(".json", StringComparison.OrdinalIgnoreCase)
                && Path.GetExtension(actualPath).Equals(".json", StringComparison.OrdinalIgnoreCase))
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

        private static void Read(string filePath, string fromDictionaryPath)
        {
            VerifyFileExists(filePath);
            VerifyFileExists(fromDictionaryPath);
            long tokenCount = 0;

            using (new ConsoleWatch($"Reading [Full] {filePath} ({FileLength.MB(filePath)})...",
                () => $"Done; {tokenCount:n0} tokens found in file"))
            {
                if (filePath.EndsWith(".bion", StringComparison.OrdinalIgnoreCase))
                {
                    using (WordCompressor compressor = (fromDictionaryPath == null ? null : WordCompressor.OpenRead(fromDictionaryPath)))
                    using (BionReader reader = new BionReader(File.OpenRead(filePath), compressor: compressor))
                    {
                        while (reader.Read())
                        {
                            tokenCount++;

                            switch (reader.TokenType)
                            {
                                case BionToken.PropertyName:
                                case BionToken.String:
                                    String8 value8 = reader.CurrentString8();
                                    //string valueS = reader.CurrentString();
                                    break;
                                case BionToken.Integer:
                                    long valueI = reader.CurrentInteger();
                                    break;
                                case BionToken.Float:
                                    double valueF = reader.CurrentFloat();
                                    break;
                            }
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

                            switch (reader.TokenType)
                            {
                                case JsonToken.PropertyName:
                                case JsonToken.String:
                                    string valueS = (string)reader.Value;
                                    break;
                                case JsonToken.Integer:
                                    long valueI = (long)reader.Value;
                                    break;
                                case JsonToken.Float:
                                    double valueF = (double)reader.Value;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private static void Count(string filePath, string fromDictionaryPath)
        {
            VerifyFileExists(filePath);
            VerifyFileExists(fromDictionaryPath);
            long tokenCount = 0;

            using (new ConsoleWatch($"Reading [Count] {filePath} ({FileLength.MB(filePath)})...",
                () => $"Done; {tokenCount:n0} tokens found in file"))
            {
                if (filePath.EndsWith(".bion", StringComparison.OrdinalIgnoreCase))
                {
                    using (WordCompressor compressor = (fromDictionaryPath == null ? null : WordCompressor.OpenRead(fromDictionaryPath)))
                    using (BionReader reader = new BionReader(File.OpenRead(filePath), compressor: compressor))
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

        private static void Skip(string filePath, string fromDictionaryPath)
        {
            VerifyFileExists(filePath);
            VerifyFileExists(fromDictionaryPath);

            using (new ConsoleWatch($"Reading [Skip All] {filePath} ({FileLength.MB(filePath)})..."))
            {
                if (filePath.EndsWith(".bion", StringComparison.OrdinalIgnoreCase))
                {
                    using (WordCompressor compressor = (fromDictionaryPath == null ? null : WordCompressor.OpenRead(fromDictionaryPath)))
                    using (BionReader reader = new BionReader(File.OpenRead(filePath), compressor: compressor))
                    {
                        reader.Skip();
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

        private static void Search(string filePath, string term)
        {
            byte[] term8 = null;
            Search(filePath, String8.Copy(term, ref term8));
        }

        private static void Search(string filePath, String8 term)
        {
            int iterations = 1;
            int matchCount = 0;

            string outputPath = Path.ChangeExtension(filePath, ".search.json");
            BionSearcher searcher = null;
            try
            {
                using (new ConsoleWatch($"Loading {filePath}..."))
                {
                    searcher = new BionSearcher(filePath, 3);
                }

                using (new ConsoleWatch($"Finding \"{term}\" [{iterations:n0}x]...",
                        () => $"Done. Wrote {matchCount:n0} matches to {outputPath}"))
                {
                    for (int iteration = 0; iteration < iterations; ++iteration)
                    {
                        ISearchResult result = searcher.Find(term);
                        using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(outputPath)))
                        {
                            writer.Formatting = Formatting.Indented;
                            writer.WriteStartArray();

                            matchCount = searcher.Write(writer, result, 0, -1);

                            writer.WriteEndArray();
                        }
                    }
                }
            }
            finally
            {
                searcher?.Dispose();
            }
        }

        private static void SearchAll(string bionRootPath)
        {
            List<BionSearcher> searchers = new List<BionSearcher>();
            try
            {
                using (new ConsoleWatch($"Loading Indices under {bionRootPath}...",
                    () => $"Done; {searchers.Count:n0} loaded"))
                {
                    foreach (string bionFilePath in Directory.GetFiles(bionRootPath, "*.bion", SearchOption.AllDirectories))
                    {
                        BionSearcher searcher = new BionSearcher(bionFilePath, 4);

                        lock (searchers)
                        {
                            searchers.Add(searcher);
                        }
                    }
                }

                int searchCount = 0;
                int resultLimit = 100;
                string outputRoot = "SearchOut";
                byte[] buffer = null;

                string outputPath = null;
                int matchCount = 0;

                Directory.CreateDirectory(outputRoot);

                while (true)
                {
                    System.Console.Write("> ");
                    string query = System.Console.ReadLine().Trim();
                    if (String.IsNullOrEmpty(query)) { break; }

                    String8 term = String8.Copy(query, ref buffer);

                    matchCount = 0;
                    searchCount++;
                    outputPath = Path.Combine(outputRoot, $"Results.{searchCount}.json");

                    using (new ConsoleWatch("", () => $"Done. {matchCount:n0} matches to {outputPath}"))
                    {
                        using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(outputPath)))
                        {
                            writer.Formatting = Formatting.Indented;
                            writer.WriteStartArray();

                            for (int i = 0; i < searchers.Count; ++i)
                            {
                                ISearchResult result = searchers[i].Find(term);
                                matchCount += searchers[i].Write(writer, result, 0, resultLimit - matchCount);

                                if (matchCount >= resultLimit) { break; }
                            }
                        }
                    }
                }
            }
            finally
            {
                foreach(BionSearcher searcher in searchers)
                {
                    searcher.Dispose();
                }
            }
        }

        private static string VerifyFileExists(string filePath)
        {
            if (filePath != null && !File.Exists(filePath)) { throw new UsageException($"File {filePath} not found."); }
            return filePath;
        }

        private static string OrDefault(string[] args, int index, string defaultValue)
        {
            return (args.Length > index ? args[index] : defaultValue);
        }

        private static string ChangePath(string filePath, string withExtension = null, string inFolder = null)
        {
            string path = filePath;
            if (String.IsNullOrEmpty(filePath)) { return filePath; }

            if (withExtension != null)
            {
                path = Path.ChangeExtension(path, withExtension);
            }

            if (inFolder != null)
            {
                string newDirectory = Path.Combine(Path.GetDirectoryName(path), inFolder);
                Directory.CreateDirectory(newDirectory);
                path = Path.Combine(newDirectory, Path.GetFileName(path));
            }

            return path;
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

    public struct SarifSearchResult
    {
        public long ResultPosition;
        public long RunPosition;

        public SarifSearchResult(long resultPosition, long runPosition)
        {
            ResultPosition = resultPosition;
            RunPosition = runPosition;
        }
    }
}
