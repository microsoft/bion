// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

using BSOA.Json;

namespace BSOA.Demo.Model.Classic
{
    public class FileSystem
    {
        public IList<Folder> Folders { get; set; }
        public IList<File> Files { get; set; }

        public FileSystem()
        {
            Folders = new List<Folder>();
            Files = new List<File>();
        }

        public void Save(string filePath)
        {
            AsJson.Save(filePath, this);
        }

        public static FileSystem Load(string filePath)
        {
            return AsJson.Load<FileSystem>(filePath);
        }
    }
}
