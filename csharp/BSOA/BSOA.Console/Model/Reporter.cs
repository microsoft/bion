// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.IO;

using BSOA.Console.Extensions;
using BSOA.Console.Model.Normal;

namespace BSOA.Console.Model
{
    public class Reporter
    {
        public static void WriteHierarchyWithSize(Folder folder, TextWriter writer, int indent = 0)
        {
            writer.WriteLine($"{folder.SizeBytes.Megabytes():n1} MB {new string('\t', indent)}{folder.Name}");

            if (folder.Subfolders != null)
            {
                foreach (Folder subfolder in folder.Subfolders)
                {
                    WriteHierarchyWithSize(subfolder, writer, indent + 1);
                }
            }
        }
    }
}
