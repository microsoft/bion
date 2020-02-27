using Bion.Json;
using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Streams;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Map;
using Microsoft.CodeAnalysis.Sarif.Writers;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Map
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                {
                    Console.WriteLine(@"Usage: Map <jsonFilePath> <mode> [args]
  Build <ratio>                   - Build JSON map from source document; file path is document, not map
  Basics <jsonPath?>              - Show size and element count under item.
  ArrayLengths <jsonPath?>        - Show the size of each element in an array (reveal outliers)
  Tree <threshold?> <jsonPath?>   - Show tree of elements over threshold (% of parent or absolute size)
  TreeToDepth <depth> <jsonPath?> - Show tree to a depth limit
  Extract <jsonPath> <toFile>     - Write element at JsonPath to a new file
  Indent <toFile>                 - Write indented copy of file 
  Minify <toFile>                 - Write minified copy of file
  Convert <fromFile> <toFile>     - Convert to/from JSON, BSON
  Parse <filePath>                - Parse JSON/BSON files and time
  Consolidate <toFile>            - Write SarifConsolidator-compressed copy of file
  LoadSarif <filePath>            - Load into SARIF OM from JSON/BSON
  ");
                    return;
                }

                string jsonFilePath = args[0];
                string mapFilePath = Path.ChangeExtension(jsonFilePath, ".map.json");
                string mode = args[1];

                // Handle modes which don't use a file map
                switch (mode.ToLowerInvariant())
                {
                    case "indent":
                        JsonIndent(
                            jsonFilePath,
                            (args.Length > 2 ? args[2] : Path.ChangeExtension(jsonFilePath, ".indent.json")));
                        return;

                    case "minify":
                        JsonMinify(
                            jsonFilePath,
                            (args.Length > 2 ? args[2] : Path.ChangeExtension(jsonFilePath, ".min.json")));
                        return;

                    case "convert":
                        Convert(
                            jsonFilePath,
                            (args.Length > 2 ? args[2] : Path.ChangeExtension(jsonFilePath, ".bson")));
                        return;

                    case "consolidate":
                        Consolidate(
                            jsonFilePath,
                            (args.Length > 2 ? args[2] : Path.ChangeExtension(jsonFilePath, ".trim.sarif")));
                        return;

                    case "parse":
                        Parse(jsonFilePath);
                        return;

                    case "loadsarif":
                        LoadSarif(jsonFilePath);
                        return;

                    case "build":
                        Build(jsonFilePath, (args.Length > 2 ? double.Parse(args[2]) : 0.01));
                        return;
                }

                // Build map if it is outdated or missing
                if (!File.Exists(mapFilePath) || (File.GetLastWriteTimeUtc(jsonFilePath) > File.GetLastWriteTimeUtc(mapFilePath)))
                {
                    Build(jsonFilePath);
                }

                // Load map
                JsonMapNode root = JsonConvert.DeserializeObject<JsonMapNode>(File.ReadAllText(mapFilePath));

                switch (mode.ToLowerInvariant())
                {
                    case "basics":
                        WriteBasics(
                            root,
                            (args.Length > 2 ? args[2] : ""),
                            Console.Out);
                        break;

                    case "arraylengths":
                        WriteArrayEntryLengths(
                            root,
                            (args.Length > 2 ? args[2] : ""),
                            Console.Out);
                        break;

                    case "tree":
                        WriteTree(
                            root,
                            (args.Length > 2 ? double.Parse(args[2]) : 0),
                            (args.Length > 3 ? args[3] : ""),
                            Console.Out);
                        break;

                    case "treetodepth":
                        WriteTreeToDepth(
                            root,
                            (args.Length > 2 ? int.Parse(args[2]) : -1),
                            (args.Length > 3 ? args[3] : ""),
                            Console.Out);
                        break;

                    case "extract":
                        Extract(
                            jsonFilePath,
                            root,
                            (args.Length > 2 ? args[2] : ""),
                            (args.Length > 3 ? args[3] : Path.ChangeExtension(jsonFilePath, ".extract.json")));
                        break;

                    default:
                        throw new ArgumentException($"Unknown Mode \"{mode}\".");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled exception: {ex}");
            }
        }

        static void TestLz4(string inputPath)
        {
            string lz4Path = inputPath + ".lz4";
            string outPath = inputPath + ".out";

            using (Stream source = File.OpenRead(inputPath))
            using (Stream target = LZ4Stream.Encode(File.Create(lz4Path), LZ4Level.L09_HC))
            {
                source.CopyTo(target);
            }

            using (Stream source = LZ4Stream.Decode(File.OpenRead(lz4Path)))
            using (Stream target = File.Create(outPath))
            {
                source.CopyTo(target);
            }

            bool success = AreFilesIdentical(inputPath, outPath);
            if (success)
            {
                Console.WriteLine($"LZ4 roundtripped file was identical to original file.");
            }
        }

        static bool AreFilesIdentical(string expectedPath, string actualPath)
        {
            int bufferSize = 8 * 1024;
            byte[] expectBuffer = new byte[bufferSize];
            byte[] actualBuffer = new byte[bufferSize];

            using (Stream expect = File.OpenRead(expectedPath))
            using (Stream actual = File.OpenRead(actualPath))
            {
                int expectLength = expect.Read(expectBuffer, 0, bufferSize);
                int actualLength = actual.Read(actualBuffer, 0, bufferSize);

                for (int i = 0; i < Math.Min(expectLength, actualLength); ++i)
                {
                    if (expectBuffer[i] != actualBuffer[i])
                    {
                        Console.WriteLine($"{expectedPath} and {actualPath} are different at position {expect.Position - (expectLength - i):n0} Expect: {expectBuffer[i]}, Actual {actualBuffer[i]}.");
                        return false;
                    }
                }

                if (expectLength != actualLength)
                {
                    Console.WriteLine($"{expectedPath} and {actualPath} were different lengths.");
                    return false;
                }
            }

            return true;
        }

        static void JsonIndent(string inputPath, string outputPath)
        {
            Console.WriteLine($"Indenting '{inputPath}' to '{outputPath}'...");
            Stopwatch w = Stopwatch.StartNew();

            using (JsonTextReader reader = new JsonTextReader(File.OpenText(inputPath)))
            using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(outputPath)))
            {
                writer.Formatting = Formatting.Indented;
                writer.WriteToken(reader);
            }

            Console.WriteLine($"Done in {w.Elapsed.TotalSeconds:n1}s; {ToSizeString(new FileInfo(inputPath).Length)} => {ToSizeString(new FileInfo(outputPath).Length)}");
        }

        static void JsonMinify(string inputPath, string outputPath)
        {
            Console.WriteLine($"Minifying '{inputPath}' to '{outputPath}'...");
            Stopwatch w = Stopwatch.StartNew();

            using (JsonTextReader reader = new JsonTextReader(File.OpenText(inputPath)))
            using (JsonTextWriter writer = new JsonTextWriter(File.CreateText(outputPath)))
            {
                writer.Formatting = Formatting.None;
                writer.WriteToken(reader);
            }

            Console.WriteLine($"Done in {w.Elapsed.TotalSeconds:n1}s; {ToSizeString(new FileInfo(inputPath).Length)} => {ToSizeString(new FileInfo(outputPath).Length)}");
        }

        static void Convert(string inputPath, string outputPath)
        {
            Console.WriteLine($"Converting '{inputPath}' to '{outputPath}'...");
            Stopwatch w = Stopwatch.StartNew();

            using (Disposal disposal = new Disposal())
            using (JsonReader reader = BuildReader(inputPath, disposal))
            using (JsonWriter writer = BuildWriter(outputPath, disposal))
            {
                writer.WriteToken(reader);
            }

            Console.WriteLine($"Done in {w.Elapsed.TotalSeconds:n1}s; {ToSizeString(new FileInfo(inputPath).Length)} => {ToSizeString(new FileInfo(outputPath).Length)}");
        }

        static void Parse(string inputPath)
        {
            Console.WriteLine($"Timing Parse for '{inputPath}'...");
            Stopwatch w = Stopwatch.StartNew();

            using (Disposal disposal = new Disposal())
            using (JsonReader reader = BuildReader(inputPath, disposal))
            {
                while (reader.Read())
                { }
            }

            Console.WriteLine($"Done in {w.Elapsed.TotalSeconds:n1}s; {ToSizeString(new FileInfo(inputPath).Length)}");
        }

        static void Consolidate(string inputPath, string outputPath)
        {
            Console.WriteLine($"Running Sarif Consolidator on '{inputPath}'...");
            Stopwatch w = Stopwatch.StartNew();

            SarifLog log = SarifLog.LoadDeferred(inputPath);
            SarifConsolidator consolidator = new SarifConsolidator(log.Runs[0]);

            foreach (Result result in log.Runs[0].Results)
            {
                consolidator.Trim(result);
            }

            log.Save(outputPath);

            Console.WriteLine($"Done in {w.Elapsed.TotalSeconds:n1}s; {ToSizeString(new FileInfo(inputPath).Length)} => {ToSizeString(new FileInfo(outputPath).Length)}");
        }

        static SarifLog LoadSarif(string inputPath)
        {
            Console.WriteLine($"Timing load SARIF of '{inputPath}'...");
            long memoryBefore = GC.GetTotalMemory(true);
            Stopwatch w = Stopwatch.StartNew();

            SarifLog log = null;

            JsonSerializer serializer = new JsonSerializer();
            using (Disposal disposal = new Disposal())
            using (JsonReader reader = BuildReader(inputPath, disposal))
            {
                log = serializer.Deserialize<SarifLog>(reader);
            }

            w.Stop();
            long memoryAfter = GC.GetTotalMemory(true);
            Console.WriteLine($"Done in {w.Elapsed.TotalSeconds:n1}s; {ToSizeString(new FileInfo(inputPath).Length)} on disk; {ToSizeString((memoryAfter - memoryBefore))} in memory.");
            return log;
        }

        static JsonReader BuildReader(string filePath, Disposal disposal)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            Stream inStream = disposal.Add(File.OpenRead(filePath));

            switch (extension)
            {
                case ".zip":
                    inStream = disposal.Add(new DeflateStream(inStream, CompressionMode.Decompress));
                    extension = Path.GetExtension((filePath.Substring(0, filePath.Length - ".zip".Length))).ToLowerInvariant();
                    break;
                case ".lz4":
                    inStream = disposal.Add(LZ4Stream.Decode(inStream));
                    extension = Path.GetExtension((filePath.Substring(0, filePath.Length - ".lz4".Length))).ToLowerInvariant();
                    break;
            }

            switch (extension)
            {
                case ".json":
                case ".sarif":
                    return disposal.Add(new JsonTextReader(disposal.Add(new StreamReader(inStream))));
                case ".bson":
                    return disposal.Add(new BsonDataReader(inStream));
                case ".bion":
                    return disposal.Add(new BionDataReader(inStream));
                default:
                    throw new NotImplementedException($"Don't know reader type for file extension '{extension}'.");
            }
        }

        static JsonWriter BuildWriter(string filePath, Disposal disposal)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            Stream outStream = disposal.Add(File.Create(filePath));

            switch (extension)
            {
                case ".zip":
                    outStream = disposal.Add(new DeflateStream(outStream, CompressionMode.Compress));
                    extension = Path.GetExtension((filePath.Substring(0, filePath.Length - ".zip".Length))).ToLowerInvariant();
                    break;
                case ".lz4":
                    outStream = disposal.Add(LZ4Stream.Encode(outStream, LZ4Level.L09_HC));
                    extension = Path.GetExtension((filePath.Substring(0, filePath.Length - ".lz4".Length))).ToLowerInvariant();
                    break;
            }

            switch (extension)
            {
                case ".json":
                case ".sarif":
                    return disposal.Add(new JsonTextWriter(disposal.Add(new StreamWriter(outStream))));
                case ".bson":
                    return disposal.Add(new BsonDataWriter(outStream));
                case ".bion":
                    return disposal.Add(new BionDataWriter(outStream));
                default:
                    throw new NotImplementedException($"Don't know writer type for file extension '{extension}'.");
            }
        }

        static void Build(string jsonFilePath, double ratio = 0.01)
        {
            string mapPath = Path.ChangeExtension(jsonFilePath, ".map.json");

            Console.WriteLine($"Building {ratio:p1} map of \"{jsonFilePath}\"...");
            Stopwatch w = Stopwatch.StartNew();

            JsonMapNode root = JsonMapBuilder.Build(jsonFilePath, new JsonMapSettings(ratio, (10 * JsonMapSettings.Megabyte) * (ratio / 0.01)));
            File.WriteAllText(mapPath, JsonConvert.SerializeObject(root, Formatting.None));

            Console.WriteLine($"Done in {w.Elapsed.TotalSeconds:n1}s; map is {ToSizeString(new FileInfo(mapPath).Length)}");
            Console.WriteLine();
        }

        static void WriteBasics(JsonMapNode root, string jsonPath, TextWriter writer)
        {
            JsonMapNode current = root.FindByPath(jsonPath);
            writer.WriteLine($"{jsonPath} is {ToSizeString(current.Length)} and has {current.Count:n0} elements");
        }

        static void WriteArrayEntryLengths(JsonMapNode root, string jsonPath, TextWriter writer)
        {
            JsonMapNode current = root.FindByPath(jsonPath);

            if (current.ArrayStarts == null)
            {
                throw new ArgumentException($"No ArrayStarts in map for node at \"{jsonPath}\".");
            }

            if (current.Every != 1)
            {
                writer.WriteLine($"Warning: Array Starts only included for every {current.Every} elements; true count {current.Count:n0}");
            }

            writer.WriteLine($"{jsonPath} is {ToSizeString(current.Length)} and has {current.Count:n0} elements with lengths:");

            // ArrayStarts are converted to absolute in JsonMapNode
            // Difference between [i] and [i - 1] is the length of [i - 1]
            for (int i = 1; i < current.ArrayStarts.Count; ++i)
            {
                writer.WriteLine($"{current.ArrayStarts[i] - current.ArrayStarts[i - 1]:n0}");
            }

            // Last element length is approximately from the last start to the array end
            if (current.Count > 0)
            {
                writer.WriteLine($"{current.End - current.ArrayStarts[current.ArrayStarts.Count - 1] - 1:n0}");
            }
        }

        static void WriteTree(JsonMapNode current, double sizeThreshold, string name, TextWriter writer, int indent = 0)
        {
            // At root, ...
            if (indent == 0)
            {
                // Find the desired start element if a json path was passed
                current = current.FindByPath(name);

                // Write the column names for subsequent output
                writer.WriteLine($"{Pad("Name", 40)} {PadRight("Count", 15)} {PadRight("Bytes", 15)}");

                // Call this element 'Root' in the output
                if (String.IsNullOrEmpty(name)) { name = "[Root]"; }
            }

            // Write basics for this element
            writer.WriteLine($"{Pad(new string(' ', indent * 2) + name, 40)} {PadRight(current.Count.ToString("n0"), 15)} {PadRight(current.Length.ToString("n0"), 15)}");

            // Recurse for children over the display threshold
            if (current.Nodes != null)
            {
                foreach (KeyValuePair<string, JsonMapNode> child in current.Nodes.OrderByDescending(kvp => kvp.Value.Length))
                {
                    if (IsOverThreshold(current, child.Value, sizeThreshold))
                    {
                        WriteTree(child.Value, sizeThreshold, child.Key, writer, indent + 1);
                    }
                }
            }
        }

        static void WriteTreeToDepth(JsonMapNode current, int depthLimit, string name, TextWriter writer, int indent = 0)
        {
            // At root, ...
            if (indent == 0)
            {
                // Find the desired start element if a json path was passed
                current = current.FindByPath(name);

                // Write the column names for subsequent output
                writer.WriteLine($"{Pad("Name", 40)} {PadRight("Count", 15)} {PadRight("Bytes", 15)}");

                // Call this element 'Root' in the output
                if (String.IsNullOrEmpty(name)) { name = "[Root]"; }
            }

            // Write basics for this element
            writer.WriteLine($"{Pad(new string(' ', indent * 2) + name, 40)} {PadRight(current.Count.ToString("n0"), 15)} {PadRight(current.Length.ToString("n0"), 15)}");

            // Recurse for children to depth limit
            if (current.Nodes != null && (indent < depthLimit || depthLimit < 0))
            {
                foreach (KeyValuePair<string, JsonMapNode> child in current.Nodes.OrderByDescending(kvp => kvp.Value.Length))
                {
                    WriteTreeToDepth(child.Value, depthLimit, child.Key, writer, indent + 1);
                }
            }
        }

        static void Extract(string jsonFilePath, JsonMapNode root, string jsonPath, string outputPath)
        {
            JsonMapNode current = root.FindByPath(jsonPath);

            Console.WriteLine($"Extracting \"{jsonPath}\" from \"{jsonFilePath}\" into \"{outputPath}\"...");

            byte[] buffer = new byte[64 * 1024];
            long lengthToCopy = current.Length;
            using (FileStream source = File.OpenRead(jsonFilePath))
            using (FileStream target = File.Create(outputPath))
            {
                source.Seek(current.Start, SeekOrigin.Begin);

                while (lengthToCopy > 0)
                {
                    int lengthRead = source.Read(buffer, 0, (int)Math.Min(buffer.Length, lengthToCopy));
                    target.Write(buffer, 0, lengthRead);
                    lengthToCopy -= lengthRead;
                }
            }

            Console.WriteLine($"Done. {ToSizeString(current.Length)} extracted.");
        }

        static string Pad(string value, int length)
        {
            if (value == null || value.Length > length) { return value; }
            return value + new string(' ', length - value.Length);
        }

        static string PadRight(string value, int length)
        {
            if (value == null || value.Length > length) { return value; }
            return new string(' ', length - value.Length) + value;
        }

        static bool IsOverThreshold(JsonMapNode parent, JsonMapNode child, double threshold)
        {
            if (threshold <= 0)
            {
                return true;
            }
            else if (threshold <= 1)
            {
                // Threshold under 1: Percentage of parent
                return child.Length >= parent.Length * threshold;
            }
            else
            {
                // Threshold over 1: Absolute size
                return child.Length >= threshold;
            }
        }

        public static string ToSizeString(long lengthBytes)
        {
            if (lengthBytes < 1024)
            {
                return $"{lengthBytes:n0} B";
            }
            else if (lengthBytes < 1024 * 1024)
            {
                return $"{((double)lengthBytes / 1024):n1} KB";
            }
            else if (lengthBytes < 1024 * 1024 * 1024)
            {
                return $"{((double)lengthBytes / (1024 * 1024)):n2} MB";
            }
            else
            {
                return $"{((double)lengthBytes / (1024 * 1024 * 1024)):n2} GB";
            }
        }
    }
}
