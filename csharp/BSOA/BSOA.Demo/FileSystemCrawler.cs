using BSOA.Demo.Model.BSOA;

using System.IO;

namespace BSOA.Demo
{
    public class FileSystemCrawler
    {
        public static FileSystem Crawl(string rootPath, bool simple = false)
        {
            FileSystem result = new FileSystem();
            result.Folders.Add(new Folder() { Name = Path.GetFullPath(rootPath), ParentIndex = -1 });

            Crawl(new DirectoryInfo(rootPath), result, 0, simple);

            return result;
        }

        private static void Crawl(DirectoryInfo directory, FileSystem result, int folderIndex, bool simple = false)
        {
            foreach (DirectoryInfo subdirectory in directory.GetDirectories())
            {
                int subfolderIndex = result.Folders.Count;
                result.Folders.Add(new Folder() { Name = subdirectory.Name, ParentIndex = folderIndex });

                Crawl(subdirectory, result, subfolderIndex, simple);
            }

            foreach (FileInfo fi in directory.GetFiles())
            {
                Model.BSOA.File file = new Model.BSOA.File()
                {
                    ParentFolderIndex = folderIndex,
                    Name = fi.Name,
                    Length = fi.Length,
                };

                if (!simple)
                {
                    file.Attributes = fi.Attributes;
                    file.CreatedUtc = fi.CreationTimeUtc;
                    file.LastModifiedUtc = fi.LastWriteTimeUtc;
                }

                result.Files.Add(file);
            }
        }
    }
}
