// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;

using BSOA.Json;
using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    public partial class FileSystem
    {
        public int FileNamesContaining(Memory<byte> value)
        {
            int matchCount = 0;

            ((INumberColumn<byte>)this.Database.File.Name).ForEach((slice) =>
            {
                Span<byte> sliceSpan = slice.Array.AsSpan().Slice(slice.Index, slice.Count);

                while (sliceSpan.Length > 0)
                {
                    int nextMatch = sliceSpan.IndexOf(value.Span);
                    if (nextMatch == -1) { break; }
                    matchCount++;
                    sliceSpan = sliceSpan.Slice(nextMatch + value.Length);
                }
            });
            return matchCount;
        }

        public void Save(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (extension)
            {
                case ".bsoa":
                    WriteBsoa(filePath);
                    break;

                default:
                    AsJson.Save(filePath, this);
                    break;
            }
        }

        public static FileSystem Load(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (extension)
            {
                case ".bsoa":
                    return ReadBsoa(filePath);

                default:
                    return AsJson.Load<FileSystem>(filePath);
            }
        }
    }
}
