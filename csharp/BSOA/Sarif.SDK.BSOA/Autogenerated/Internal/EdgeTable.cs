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

        internal IColumn<string> Id;
        internal RefColumn Label;
        internal IColumn<string> SourceNodeId;
        internal IColumn<string> TargetNodeId;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal EdgeTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>(default));
            Label = AddColumn(nameof(Label), new RefColumn(nameof(SarifLogDatabase.Message)));
            SourceNodeId = AddColumn(nameof(SourceNodeId), ColumnFactory.Build<string>(default));
            TargetNodeId = AddColumn(nameof(TargetNodeId), ColumnFactory.Build<string>(default));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Edge Get(int index)
        {
            return (index == -1 ? null : new Edge(this, index));
        }
    }
}
