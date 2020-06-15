// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace BSOA.Console.Model.Normal
{
    public class File
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        //public Folder Parent { get; set; }
        public long Length { get; set; }
        public DateTime LastModifiedUtc { get; set; }
    }
}
