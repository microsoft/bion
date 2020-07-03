// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    /// <summary>
    ///  BSOA GENERATED Table for 'FileSystem'
    /// </summary>
    internal partial class FileSystemTable : Table<FileSystem>
    {
        internal FileSystemDatabase Database;

        internal RefListColumn Folders;
        internal RefListColumn Files;

        internal FileSystemTable(FileSystemDatabase database) : base()
        {
            Database = database;

            Folders = AddColumn(nameof(Folders), new RefListColumn(nameof(FileSystemDatabase.Folder)));
            Files = AddColumn(nameof(Files), new RefListColumn(nameof(FileSystemDatabase.File)));
        }

        public override FileSystem Get(int index)
        {
            return (index == -1 ? null : new FileSystem(this, index));
        }
    }
}
