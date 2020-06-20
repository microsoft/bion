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

            Id = AddColumn(nameof(Id), ColumnFactory.Build<String>(default));
            Label = AddColumn(nameof(Label), new RefColumn(nameof(SarifLogDatabase.Message)));
            SourceNodeId = AddColumn(nameof(SourceNodeId), ColumnFactory.Build<String>(default));
            TargetNodeId = AddColumn(nameof(TargetNodeId), ColumnFactory.Build<String>(default));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Edge Get(int index)
        {
            return (index == -1 ? null : new Edge(this, index));
        }
    }
}
