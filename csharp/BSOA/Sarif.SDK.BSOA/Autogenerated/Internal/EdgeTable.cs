using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

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
        internal IColumn<IDictionary<string, string>> Properties;

        internal EdgeTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>());
            Label = AddColumn(nameof(Label), new RefColumn(nameof(SarifLogDatabase.Message)));
            SourceNodeId = AddColumn(nameof(SourceNodeId), ColumnFactory.Build<string>());
            TargetNodeId = AddColumn(nameof(TargetNodeId), ColumnFactory.Build<string>());
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override Edge Get(int index)
        {
            return (index == -1 ? null : new Edge(this, index));
        }
    }
}
