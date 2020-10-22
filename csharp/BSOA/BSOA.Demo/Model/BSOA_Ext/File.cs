// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    public partial class File
    {
        public string Description(FileSystem db)
        {
            return $"{db.Folders[ParentFolderIndex].FullPath(db)}\\{Name} | {Length:n0} | {LastModifiedUtc:u}";
        }
    }
}
