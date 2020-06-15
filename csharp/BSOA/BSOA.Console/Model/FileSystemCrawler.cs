// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.IO;

using File = BSOA.Console.Model.Normal.File;
using Folder = BSOA.Console.Model.Normal.Folder;

namespace BSOA.Console.Model
{
    public class FileSystemCrawler
    {
        public static Folder Crawl(string folderPath)
        {
            return Build(null, new DirectoryInfo(folderPath));
        }

        private static Folder Build(Folder parent, DirectoryInfo folder)
        {
            Folder result = new Folder()
            {
                //Parent = parent,
                Name = folder.Name
            };

            foreach (DirectoryInfo subfolder in folder.GetDirectories())
            {
                result.Subfolders ??= new List<Folder>();
                result.Subfolders.Add(Build(result, subfolder));
            }

            foreach (FileInfo file in folder.GetFiles())
            {
                result.Files ??= new List<File>();
                result.Files.Add(Build(result, file));
            }

            return result;
        }

        private static File Build(Folder parent, FileInfo file)
        {
            return new File()
            {
                Name = file.Name,
                Extension = file.Extension,
                Length = file.Length,
                LastModifiedUtc = file.LastWriteTimeUtc,
                //Parent = parent
            };
        }
    }
}
