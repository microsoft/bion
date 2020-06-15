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
        internal IColumn<string> SourceLanguage;
        internal IColumn<IDictionary<string, string>> Properties;

        internal RegionTable(SarifLogDatabase database) : base()
        {
            Database = database;

            StartLine = AddColumn(nameof(StartLine), ColumnFactory.Build<int>());
            StartColumn = AddColumn(nameof(StartColumn), ColumnFactory.Build<int>());
            EndLine = AddColumn(nameof(EndLine), ColumnFactory.Build<int>());
            EndColumn = AddColumn(nameof(EndColumn), ColumnFactory.Build<int>());
            CharOffset = AddColumn(nameof(CharOffset), ColumnFactory.Build<int>(-1));
            CharLength = AddColumn(nameof(CharLength), ColumnFactory.Build<int>());
            ByteOffset = AddColumn(nameof(ByteOffset), ColumnFactory.Build<int>(-1));
            ByteLength = AddColumn(nameof(ByteLength), ColumnFactory.Build<int>());
            Snippet = AddColumn(nameof(Snippet), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            SourceLanguage = AddColumn(nameof(SourceLanguage), ColumnFactory.Build<string>());
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override Region Get(int index)
        {
            return (index == -1 ? null : new Region(this, index));
        }
    }
}
