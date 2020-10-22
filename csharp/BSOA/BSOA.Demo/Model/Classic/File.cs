// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;

namespace BSOA.Demo.Model.Classic
{
    public class File
    {
        public int ParentFolderIndex { get; set; }

        public string Name { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public DateTime CreatedUtc { get; set; }
        public FileAttributes Attributes { get; set; }
        public long Length { get; set; }

        public File()
        { }

        public string Description(FileSystem db)
        {
            return $"{db.Folders[ParentFolderIndex].FullPath(db)}\\{Name} | {Length:n0} | {LastModifiedUtc:u}";
        }
    }
}
