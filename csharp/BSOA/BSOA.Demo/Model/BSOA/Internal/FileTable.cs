// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    /// <summary>
    ///  BSOA GENERATED Table for 'File'
    /// </summary>
    internal partial class FileTable : Table<File>
    {
        internal FileSystemDatabase Database;

        internal IColumn<int> ParentFolderIndex;
        internal IColumn<string> Name;
        internal IColumn<DateTime> LastModifiedUtc;
        internal IColumn<DateTime> CreatedUtc;
        internal IColumn<int> Attributes;
        internal IColumn<long> Length;

        public FileTable(IDatabase database, Dictionary<string, IColumn> columns = null) : base(database, columns)
        {
            Database = (FileSystemDatabase)database;
            GetOrBuildColumns();
        }

        public override void GetOrBuildColumns()
        {
            ParentFolderIndex = GetOrBuild(nameof(ParentFolderIndex), () => Database.BuildColumn<int>(nameof(File), nameof(ParentFolderIndex), default));
            Name = GetOrBuild(nameof(Name), () => Database.BuildColumn<string>(nameof(File), nameof(Name), default));
            LastModifiedUtc = GetOrBuild(nameof(LastModifiedUtc), () => Database.BuildColumn<DateTime>(nameof(File), nameof(LastModifiedUtc), default));
            CreatedUtc = GetOrBuild(nameof(CreatedUtc), () => Database.BuildColumn<DateTime>(nameof(File), nameof(CreatedUtc), default));
            Attributes = GetOrBuild(nameof(Attributes), () => Database.BuildColumn<int>(nameof(File), nameof(Attributes), (int)default));
            Length = GetOrBuild(nameof(Length), () => Database.BuildColumn<long>(nameof(File), nameof(Length), default));
        }

        public override File Get(int index)
        {
            return (index == -1 ? null : new File(this, index));
        }
    }
}
