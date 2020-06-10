using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

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
        internal IColumn<IDictionary<string, string>> Properties;

        internal NodeTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>());
            Label = AddColumn(nameof(Label), new RefColumn(nameof(SarifLogDatabase.Message)));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Children = AddColumn(nameof(Children), new RefListColumn(nameof(SarifLogDatabase.Node)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override Node Get(int index)
        {
            return (index == -1 ? null : new Node(this, index));
        }
    }
}
