using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'StackFrame'
    /// </summary>
    internal partial class StackFrameTable : Table<StackFrame>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Location;
        internal IColumn<string> Module;
        internal IColumn<int> ThreadId;
        internal IColumn<IList<string>> Parameters;
        internal IColumn<IDictionary<string, string>> Properties;

        internal StackFrameTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Module = AddColumn(nameof(Module), ColumnFactory.Build<string>());
            ThreadId = AddColumn(nameof(ThreadId), ColumnFactory.Build<int>());
            Parameters = AddColumn(nameof(Parameters), ColumnFactory.Build<IList<string>>());
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override StackFrame Get(int index)
        {
            return (index == -1 ? null : new StackFrame(this, index));
        }
    }
}
