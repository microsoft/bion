// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Edge'
    /// </summary>
    internal partial class EdgeTable : Table<Edge>
    {
        internal SarifLogDatabase Database;

        internal IColumn<String> Id;
        internal RefColumn Label;
        internal IColumn<String> SourceNodeId;
        internal IColumn<String> TargetNodeId;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal EdgeTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), database.BuildColumn<String>(nameof(Edge), nameof(Id), default));
            Label = AddColumn(nameof(Label), new RefColumn(nameof(SarifLogDatabase.Message)));
            SourceNodeId = AddColumn(nameof(SourceNodeId), database.BuildColumn<String>(nameof(Edge), nameof(SourceNodeId), default));
            TargetNodeId = AddColumn(nameof(TargetNodeId), database.BuildColumn<String>(nameof(Edge), nameof(TargetNodeId), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Edge), nameof(Properties), default));
        }

        public override Edge Get(int index)
        {
            return (index == -1 ? null : new Edge(this, index));
        }
    }
}
