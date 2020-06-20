// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Region'
    /// </summary>
    internal partial class RegionTable : Table<Region>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> StartLine;
        internal IColumn<int> StartColumn;
        internal IColumn<int> EndLine;
        internal IColumn<int> EndColumn;
        internal IColumn<int> CharOffset;
        internal IColumn<int> CharLength;
        internal IColumn<int> ByteOffset;
        internal IColumn<int> ByteLength;
        internal RefColumn Snippet;
        internal RefColumn Message;
        internal IColumn<String> SourceLanguage;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal RegionTable(SarifLogDatabase database) : base()
        {
            Database = database;

            StartLine = AddColumn(nameof(StartLine), database.BuildColumn<int>(nameof(Region), nameof(StartLine), default));
            StartColumn = AddColumn(nameof(StartColumn), database.BuildColumn<int>(nameof(Region), nameof(StartColumn), default));
            EndLine = AddColumn(nameof(EndLine), database.BuildColumn<int>(nameof(Region), nameof(EndLine), default));
            EndColumn = AddColumn(nameof(EndColumn), database.BuildColumn<int>(nameof(Region), nameof(EndColumn), default));
            CharOffset = AddColumn(nameof(CharOffset), database.BuildColumn<int>(nameof(Region), nameof(CharOffset), -1));
            CharLength = AddColumn(nameof(CharLength), database.BuildColumn<int>(nameof(Region), nameof(CharLength), default));
            ByteOffset = AddColumn(nameof(ByteOffset), database.BuildColumn<int>(nameof(Region), nameof(ByteOffset), -1));
            ByteLength = AddColumn(nameof(ByteLength), database.BuildColumn<int>(nameof(Region), nameof(ByteLength), default));
            Snippet = AddColumn(nameof(Snippet), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            SourceLanguage = AddColumn(nameof(SourceLanguage), database.BuildColumn<String>(nameof(Region), nameof(SourceLanguage), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Region), nameof(Properties), default));
        }

        public override Region Get(int index)
        {
            return (index == -1 ? null : new Region(this, index));
        }
    }
}
