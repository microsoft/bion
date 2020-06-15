// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace BSOA.Console.Model.Normal
{
    public class Folder
    {
        public string Name { get; set; }
        //public Folder Parent { get; set; }

        public IList<File> Files { get; set; }
        public IList<Folder> Subfolders { get; set; }

        public long SizeBytes => (Subfolders?.Sum((sub) => sub.SizeBytes) ?? 0) + (Files?.Sum((file) => file.Length) ?? 0);

        public Folder()
        { }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
