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
                        string bionDictPath = OrDefault(args, 3, ChangePath(fromPath, ".dict.bion", "Out"));
                        string comparePath = OrDefault(args, 4, ChangePath(fromPath, ".compare.json", "Out"));

                        ToBion(fromPath, bionPath, bionDictPath);
                        ToJson(bionPath, comparePath, bionDictPath);
                        return Compare(fromPath, comparePath);

                    case "search":
                        if (args.Length < 3) { throw new UsageException("search requires a bion file path and search string."); }
                        Search(args[1], args[2], (args.Length > 3 ? int.Parse(args[3]) : 1));
                        break;

                    case "searchreal":
                        if (args.Length < 3) { throw new UsageException("search requires a bion file path and search string."); }
                        SearchReal(args[1], "snippet", args[2]);
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

        private static void Search(string filePath, string term, int iterations = 1)
        {
            long[] matchPositions = new long[256];
            int matchCount = -1;

            using (new ConsoleWatch($"Finding '{term}' in '{filePath}' {iterations:n0}x..."))
            {
                WordCompressor compressor = null;
                SearchIndexReader indexReader = null;

                try
                {
                    using (new ConsoleWatch("Loading Word Index..."))
                    {
                        for (int i = 0; i < iterations; ++i)
                        {
                            compressor = WordCompressor.OpenRead(Path.ChangeExtension(filePath, ".dict.bion"));
                        }
                    }

                    using (new ConsoleWatch("Loading Search Index..."))
                    {
                        for (int i = 0; i < iterations; ++i)
                        {
                            indexReader = new SearchIndexReader(Path.ChangeExtension(filePath, ".idx"));
                        }
                    }

                    using (new ConsoleWatch("Finding Matches..."))
                    {
                        byte[] convertBuffer = null;
                        int wordIndex;
                        matchCount = 0;

                        for (int i = 0; i < iterations; ++i)
                        {
                            if (compressor.TryGetWordIndex(String8.Copy(term, ref convertBuffer), out wordIndex))
                            {
                                SearchResult matches = indexReader.Find(wordIndex);
                                while (!matches.Done)
                                {
                                    matchCount += matches.Page(ref matchPositions);
                                }
                            }
                        }
                    }
                }
                finally
                {
                    compressor?.Dispose();
                    indexReader?.Dispose();
                }
            }

            if (matchCount == -1)
            {
                System.Console.WriteLine($"'{term}' not found.");
            }
            else
            {
                System.Console.WriteLine($"'{term}' found {matchCount:n0} times. First Offset: {matchPositions?[0]:n0}.");
            }
        }

        private static void SearchReal(string filePath, string propertyName, string term)
        {
            byte[] propertyName8 = null, term8 = null;
            SearchReal(filePath, String8.Copy(propertyName, ref propertyName8), String8.Copy(term, ref term8));
        }

        private static void SearchReal(string filePath, String8 propertyName, String8 term)
        {
            int iterations = 100;

            int matchCount = 0;
            long[] termPositions = new long[256];

            WordCompressor compressor = null;
            SearchIndexReader searchIndexReader = null;
            ContainerIndex containerIndex = null;
            BionReader bionReader = null;

            ContainerEntry lastRunResults = ContainerEntry.Empty;

            string outputPath = Path.ChangeExtension(filePath, ".search.json");

            try
            {
                using (new ConsoleWatch("Loading Dictionary, Search Index..."))
                {
                    compressor = Memory.Log("Dictionary", () => WordCompressor.OpenRead(Path.ChangeExtension(filePath, ".dict.bion")));
                    containerIndex = Memory.Log("ContainerIndex", () => ContainerIndex.OpenRead(Path.ChangeExtension(filePath, ".cdx")));
                    searchIndexReader = Memory.Log("SearchIndex", () => new SearchIndexReader(Path.ChangeExtension(filePath, ".idx")));
                    bionReader = Memory.Log("BionReader", () => new BionReader(File.OpenRead(filePath), containerIndex: containerIndex, compressor: compressor));
                }

                using (new ConsoleWatch($"Finding \"{propertyName}\":\"{term}\" [{iterations:n0}x]...",
                        () => $"Done. Wrote {matchCount:n0} matches to {outputPath}"))
                {
                    for (int iteration = 0; iteration < iterations; ++iteration)
                    {
                        matchCount = 0;

                        using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(outputPath)))
                        {
                            writer.Formatting = Formatting.Indented;
                            writer.WriteStartArray();

                            // Look up words
                            int propertyNameIndex;
                            if (!compressor.TryGetWordIndex(propertyName, out propertyNameIndex))
                            {
                                System.Console.WriteLine($"\"{propertyName}\" not found.");
                            }

                            int containsWordIndex;
                            if (!compressor.TryGetWordIndex(term, out containsWordIndex))
                            {
                                System.Console.WriteLine($"\"{term}\" not found.");
                            }

                            // Find matches
                            long lastResultPosition = -1;

                            SearchResult wordMatches = searchIndexReader.Find(containsWordIndex);
                            while (!wordMatches.Done)
                            {
                                int count = wordMatches.Page(ref termPositions);
                                for (int i = 0; i < count; ++i)
                                {
                                    long position = termPositions[i];

                                    ContainerEntry nearest = containerIndex.NearestIndexedContainer(position);
                                    ContainerEntry runResultsArray = containerIndex.AncestorAtDepth(nearest, 4);
                                    if (!runResultsArray.Equals(lastRunResults))
                                    {
                                        // TODO: Write run
                                        lastRunResults = runResultsArray;
                                    }

                                    // If a container under the results array was found, we have the result
                                    if (!nearest.Equals(runResultsArray))
                                    {
                                        ContainerEntry result = containerIndex.AncestorAtDepth(nearest, 5);

                                        if (result.StartByteOffset != lastResultPosition)
                                        {
                                            matchCount++;
                                            lastResultPosition = result.StartByteOffset;
                                            bionReader.Seek(result.StartByteOffset);
                                            JsonBionConverter.BionToJson(bionReader, writer);
                                        }
                                    }
                                    else
                                    {
                                        // If not, seek to the last result before the desired position
                                        ContainerEntry closest = containerIndex.LastChildBefore(runResultsArray, position);
                                        bionReader.Seek(closest.EndByteOffset);

                                        // Skip results until we read the desired one
                                        long resultPosition = bionReader.BytesRead;
                                        long nextPosition = resultPosition;

                                        while (nextPosition < position)
                                        {
                                            bionReader.Skip();
                                            resultPosition = nextPosition;
                                            nextPosition = bionReader.BytesRead;
                                        }

                                        if (resultPosition != lastResultPosition)
                                        {
                                            matchCount++;
                                            lastResultPosition = resultPosition;
                                            bionReader.Seek(resultPosition);
                                            JsonBionConverter.BionToJson(bionReader, writer);
                                        }
                                    }
                                }
                            }

                            writer.WriteEndArray();
                        }
                    }
                }
            }
            finally
            {
                compressor?.Dispose();
                searchIndexReader?.Dispose();
                bionReader?.Dispose();
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
