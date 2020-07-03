using BSOA.Demo.Model.BSOA;
using BSOA.Json;

using System;
using System.Linq;
using System.Runtime.Serialization;

namespace BSOA.Demo
{
    class Program
    {
        public const string Usage = @"BSOA.Demo <mode> <dbPath> <args>
  BSOA.Demo crawl  <DBPath> <RootPathToCrawl>
  BSOA.Demo load   <DBPath>
  BSOA.Demo search <DBPath> <FileNamePart>";

        static int Main(string[] args)
        {
            try
            {
                if (args.Length < 2) { throw new UsageException("Not enough arguments."); }
                string mode = args[0].ToLowerInvariant();
                string databasePath = args[1];

                FileSystem db = null;

                switch (mode)
                {
                    case "crawl":
                        if (args.Length < 3) { throw new UsageException("'crawl' requires DBPath and PathToCrawl."); }

                        Measure.Time("Crawl", 1, () =>
                        {
                            db = FileSystemCrawler.Crawl(rootPath: args[2], simple: (args.Length > 3 ? bool.Parse(args[3]) : false));
                        });

                        Measure.Time("Save (JSON)", 1, () =>
                        {
                            AsJson.Save(databasePath, db);
                        });

                        db.WriteBsoa(System.IO.Path.ChangeExtension(databasePath, ".bsoa"));

                        break;

                    case "load":
                        db = Measure.LoadPerformance(databasePath, 5, AsJson.Load<FileSystem>);
                        break;

                    case "search":
                        if (args.Length < 3) { throw new UsageException("'search' requires DBPath and FileNamePart."); }
                        string searchString = args[2];

                        //db = Measure.LoadPerformance(databasePath, 1, AsJson.Load<FileSystem>);
                        db = Measure.LoadPerformance(databasePath, 1, (path) => FileSystem.ReadBsoa(System.IO.Path.ChangeExtension(path, ".bsoa")));

                        Measure.Time("Search", 1, () =>
                        {
                            int matchCount = 0;

                            foreach (var file in db.Files.Where((f) => f.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)))
                            {
                                matchCount++;
                                if (matchCount < 15)
                                {
                                    Console.WriteLine($" -> {file.Description(db)}");
                                }
                            }

                            Console.WriteLine($"{matchCount:n0} matches found.");
                        });

                        break;

                    default:
                        Console.WriteLine($"Unknown mode '{mode}'");
                        Console.WriteLine(Usage);
                        return -2;
                }

                return 0;
            }
            catch (UsageException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(Usage);
                return -2;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex}");
                return -1;
            }
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
