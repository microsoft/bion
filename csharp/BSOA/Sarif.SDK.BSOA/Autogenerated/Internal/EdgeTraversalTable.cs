using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'EdgeTraversal'
    /// </summary>
    internal partial class EdgeTraversalTable : Table<EdgeTraversal>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> EdgeId;
        internal RefColumn Message;
        internal IColumn<IDictionary<string, MultiformatMessageString>> FinalState;
        internal IColumn<int> StepOverEdgeCount;
        internal IColumn<IDictionary<string, string>> Properties;

        internal EdgeTraversalTable(SarifLogDatabase database) : base()
        {
            Database = database;

            EdgeId = AddColumn(nameof(EdgeId), ColumnFactory.Build<string>());
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
//             FinalState = AddColumn(nameof(FinalState), ColumnFactory.Build<IDictionary<string, MultiformatMessageString>>());
            StepOverEdgeCount = AddColumn(nameof(StepOverEdgeCount), ColumnFactory.Build<int>());
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override EdgeTraversal Get(int index)
        {
            return (index == -1 ? null : new EdgeTraversal(this, index));
        }
    }
}
