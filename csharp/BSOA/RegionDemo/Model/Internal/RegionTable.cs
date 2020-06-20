// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Region'
    /// </summary>
    internal partial class RegionTable : Table<Region>
    {
        internal TinyDatabase Database;

        internal IColumn<int> StartLine;
        internal IColumn<int> StartColumn;
        internal IColumn<int> EndLine;
        internal IColumn<int> EndColumn;
        internal RefColumn Snippet;
        internal RefColumn Message;

        internal RegionTable(TinyDatabase database) : base()
        {
            Database = database;

            StartLine = AddColumn(nameof(StartLine), database.BuildColumn<int>(nameof(Region), nameof(StartLine), 0));
            StartColumn = AddColumn(nameof(StartColumn), database.BuildColumn<int>(nameof(Region), nameof(StartColumn), 0));
            EndLine = AddColumn(nameof(EndLine), database.BuildColumn<int>(nameof(Region), nameof(EndLine), 0));
            EndColumn = AddColumn(nameof(EndColumn), database.BuildColumn<int>(nameof(Region), nameof(EndColumn), 0));
            Snippet = AddColumn(nameof(Snippet), new RefColumn(nameof(TinyDatabase.ArtifactContent)));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(TinyDatabase.Message)));
        }

        public override Region Get(int index)
        {
            return (index == -1 ? null : new Region(this, index));
        }
    }
}
