// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ToolComponentReference'
    /// </summary>
    internal partial class ToolComponentReferenceTable : Table<ToolComponentReference>
    {
        internal SarifLogDatabase Database;

        internal IColumn<String> Name;
        internal IColumn<int> Index;
        internal IColumn<String> Guid;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ToolComponentReferenceTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), database.BuildColumn<String>(nameof(ToolComponentReference), nameof(Name), default));
            Index = AddColumn(nameof(Index), database.BuildColumn<int>(nameof(ToolComponentReference), nameof(Index), -1));
            Guid = AddColumn(nameof(Guid), database.BuildColumn<String>(nameof(ToolComponentReference), nameof(Guid), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ToolComponentReference), nameof(Properties), default));
        }

        public override ToolComponentReference Get(int index)
        {
            return (index == -1 ? null : new ToolComponentReference(this, index));
        }
    }
}
