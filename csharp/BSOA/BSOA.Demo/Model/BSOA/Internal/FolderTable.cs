// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model.BSOA
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Folder'
    /// </summary>
    internal partial class FolderTable : Table<Folder>
    {
        internal FileSystemDatabase Database;

        internal IColumn<int> ParentIndex;
        internal IColumn<string> Name;

        internal FolderTable(FileSystemDatabase database) : base()
        {
            Database = database;

            ParentIndex = AddColumn(nameof(ParentIndex), database.BuildColumn<int>(nameof(Folder), nameof(ParentIndex), default));
            Name = AddColumn(nameof(Name), database.BuildColumn<string>(nameof(Folder), nameof(Name), default));
        }

        public override Folder Get(int index)
        {
            return (index == -1 ? null : new Folder(this, index));
        }
    }
}
