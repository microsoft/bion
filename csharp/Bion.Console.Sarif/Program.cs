using Bion.Json;
using Microsoft.CodeAnalysis.Sarif;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Diagnostics;
using System.IO;

namespace Bion.Console.Sarif
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = args[0];
            LoadSarif(filePath);
        }

        static SarifLog LoadSarif(string inputPath)
        {
            System.Console.WriteLine($"Timing load SARIF of '{inputPath}'...");
            long memoryBefore = GC.GetTotalMemory(true);
            Stopwatch w = Stopwatch.StartNew();

            SarifLog log = null;

            JsonSerializer serializer = new JsonSerializer();
            using (JsonReader reader = BuildReader(inputPath))
            {
                log = serializer.Deserialize<SarifLog>(reader);
            }

            w.Stop();
            long memoryAfter = GC.GetTotalMemory(true);
            System.Console.WriteLine($"Done in {w.Elapsed.TotalSeconds:n1}s; {ToSizeString(new FileInfo(inputPath).Length)} on disk; {ToSizeString((memoryAfter - memoryBefore))} in memory.");
            return log;
        }

        static JsonReader BuildReader(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            switch (extension)
            {
                case ".json":
                case ".sarif":
                    return new JsonTextReader(File.OpenText(filePath));
                case ".bson":
                    return new BsonDataReader(File.OpenRead(filePath));
                case ".bion":
                    return new BionDataReader(File.OpenRead(filePath));
                default:
                    throw new NotImplementedException($"Don't know reader type for file extension '{extension}'.");
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
