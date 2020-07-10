using BSOA.Demo.Model.BSOA;
using BSOA.Json;

using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

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
                        string pathToCrawl = args[2];

                        Measure.Time($"Crawling {pathToCrawl}...", 1, () =>
                        {
                            db = FileSystemCrawler.Crawl(rootPath: pathToCrawl, simple: (args.Length > 3 ? bool.Parse(args[3]) : false));
                        });

                        Measure.Time($"Saving as {databasePath}...", 1, () =>
                        {
                            db.Save(databasePath);
                        });

                        break;

                    case "load":
                        db = Measure.LoadPerformance(databasePath, 5, AsJson.Load<FileSystem>);
                        break;

                    case "search":
                        if (args.Length < 3) { throw new UsageException("'search' requires DBPath and FileNamePart."); }
                        string searchString = args[2];

                        db = Measure.LoadPerformance(databasePath, 1, FileSystem.Load);

                        int matchCount = 0;

                        Measure.Time($"Searching for '{searchString}'...", 5, () =>
                        {
                            matchCount = 0;
                            foreach (var file in db.Files.Where((f) => f.Name.IndexOf(searchString, StringComparison.Ordinal) >= 0))
                            {
                                matchCount++;
                            }
                        });

                        Console.WriteLine($"{matchCount:n0} matches found.");

                        int innerIterations = 100;
                        Measure.Time($"Vector search for '{searchString}' ({innerIterations}x)...", 5, () =>
                        {
                            byte[] searchString8 = Encoding.UTF8.GetBytes(searchString);

                            for (int i = 0; i < innerIterations; ++i)
                            {
                                matchCount = db.FileNamesContaining(searchString8);
                            }

                        });

                        Console.WriteLine($"{matchCount:n0} matches found.");

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
