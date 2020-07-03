// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
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

        internal FileTable(FileSystemDatabase database) : base()
        {
            Database = database;

            ParentFolderIndex = AddColumn(nameof(ParentFolderIndex), database.BuildColumn<int>(nameof(File), nameof(ParentFolderIndex), default));
            Name = AddColumn(nameof(Name), database.BuildColumn<string>(nameof(File), nameof(Name), default));
            LastModifiedUtc = AddColumn(nameof(LastModifiedUtc), database.BuildColumn<DateTime>(nameof(File), nameof(LastModifiedUtc), default));
            CreatedUtc = AddColumn(nameof(CreatedUtc), database.BuildColumn<DateTime>(nameof(File), nameof(CreatedUtc), default));
            Attributes = AddColumn(nameof(Attributes), database.BuildColumn<int>(nameof(File), nameof(Attributes), (int)default));
            Length = AddColumn(nameof(Length), database.BuildColumn<long>(nameof(File), nameof(Length), default));
        }

        public override File Get(int index)
        {
            return (index == -1 ? null : new File(this, index));
        }
    }
}
