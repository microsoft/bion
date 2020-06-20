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

        internal IColumn<String> Id;
        internal RefColumn Label;
        internal RefColumn Location;
        internal RefListColumn Children;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal NodeTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), database.BuildColumn<String>(nameof(Node), nameof(Id), default));
            Label = AddColumn(nameof(Label), new RefColumn(nameof(SarifLogDatabase.Message)));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Children = AddColumn(nameof(Children), new RefListColumn(nameof(SarifLogDatabase.Node)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Node), nameof(Properties), default));
        }

        public override Node Get(int index)
        {
            return (index == -1 ? null : new Node(this, index));
        }
    }
}
