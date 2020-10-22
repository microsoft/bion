// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Demo.Model.BSOA;

namespace BSOA.Demo
{
    public class FileSystemCrawler
    {
        public static FileSystem Crawl(string rootPath, bool simple = false)
        {
            FileSystem result = new FileSystem();
            result.Folders.Add(new Folder() { Name = System.IO.Path.GetFullPath(rootPath), ParentIndex = -1 });

            Crawl(new System.IO.DirectoryInfo(rootPath), result, 0, simple);

            return result;
        }

        private static void Crawl(System.IO.DirectoryInfo directory, FileSystem result, int folderIndex, bool simple = false)
        {
            foreach (System.IO.DirectoryInfo subdirectory in directory.GetDirectories())
            {
                int subfolderIndex = result.Folders.Count;
                result.Folders.Add(new Folder() { Name = subdirectory.Name, ParentIndex = folderIndex });

                Crawl(subdirectory, result, subfolderIndex, simple);
            }

            foreach (System.IO.FileInfo fi in directory.GetFiles())
            {
                File file = new File()
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
