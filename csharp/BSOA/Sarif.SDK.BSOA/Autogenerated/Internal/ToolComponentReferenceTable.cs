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

        internal IColumn<string> Name;
        internal IColumn<int> Index;
        internal IColumn<string> Guid;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ToolComponentReferenceTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>(default(string)));
            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>(default(string)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, SerializedPropertyInfo>>(default(IDictionary<string, SerializedPropertyInfo>)));
        }

        public override ToolComponentReference Get(int index)
        {
            return (index == -1 ? null : new ToolComponentReference(this, index));
        }
    }
}
