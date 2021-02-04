// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Collections;
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

        internal IColumn<NumberList<int>> Folders;
        internal IColumn<NumberList<int>> Files;

        public FileSystemTable(IDatabase database, Dictionary<string, IColumn> columns = null) : base(database, columns)
        {
            Database = (FileSystemDatabase)database;
            GetOrBuildColumns();
        }

        public override void GetOrBuildColumns()
        {
            Folders = GetOrBuild(nameof(Folders), () => (IColumn<NumberList<int>>)new RefListColumn(nameof(FileSystemDatabase.Folder)));
            Files = GetOrBuild(nameof(Files), () => (IColumn<NumberList<int>>)new RefListColumn(nameof(FileSystemDatabase.File)));
        }

        public override FileSystem Get(int index)
        {
            return (index == -1 ? null : new FileSystem(this, index));
        }
    }
}
