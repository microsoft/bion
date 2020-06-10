using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'GraphTraversal'
    /// </summary>
    internal partial class GraphTraversalTable : Table<GraphTraversal>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> RunGraphIndex;
        internal IColumn<int> ResultGraphIndex;
        internal RefColumn Description;
        internal IColumn<IDictionary<string, MultiformatMessageString>> InitialState;
        internal IColumn<IDictionary<string, MultiformatMessageString>> ImmutableState;
        internal RefListColumn EdgeTraversals;
        internal IColumn<IDictionary<string, string>> Properties;

        internal GraphTraversalTable(SarifLogDatabase database) : base()
        {
            Database = database;

            RunGraphIndex = AddColumn(nameof(RunGraphIndex), ColumnFactory.Build<int>(-1));
            ResultGraphIndex = AddColumn(nameof(ResultGraphIndex), ColumnFactory.Build<int>(-1));
            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
//             InitialState = AddColumn(nameof(InitialState), ColumnFactory.Build<IDictionary<string, MultiformatMessageString>>());
//             ImmutableState = AddColumn(nameof(ImmutableState), ColumnFactory.Build<IDictionary<string, MultiformatMessageString>>());
            EdgeTraversals = AddColumn(nameof(EdgeTraversals), new RefListColumn(nameof(SarifLogDatabase.EdgeTraversal)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override GraphTraversal Get(int index)
        {
            return (index == -1 ? null : new GraphTraversal(this, index));
        }
    }
}
