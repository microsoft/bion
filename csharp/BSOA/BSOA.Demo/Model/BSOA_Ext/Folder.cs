// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text;

namespace BSOA.Demo.Model.BSOA
{
    public partial class Folder
    {
        public string FullPath(FileSystem fileSystem)
        {
            StringBuilder path = new StringBuilder();
            FullPath(fileSystem, path);
            return path.ToString();
        }

        private void FullPath(FileSystem fileSystem, StringBuilder path)
        {
            if (ParentIndex != -1)
            {
                fileSystem.Folders[ParentIndex].FullPath(fileSystem, path);
                path.Append("\\");
            }

            path.Append(Name);
        }
    }
}
