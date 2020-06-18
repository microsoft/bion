// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Node'
    /// </summary>
    internal partial class NodeTable : Table<Node>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Id;
        internal RefColumn Label;
        internal RefColumn Location;
        internal RefListColumn Children;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal NodeTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>(default(string)));
            Label = AddColumn(nameof(Label), new RefColumn(nameof(SarifLogDatabase.Message)));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Children = AddColumn(nameof(Children), new RefListColumn(nameof(SarifLogDatabase.Node)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, SerializedPropertyInfo>>(default(IDictionary<string, SerializedPropertyInfo>)));
        }

        public override Node Get(int index)
        {
            return (index == -1 ? null : new Node(this, index));
        }
    }
}
