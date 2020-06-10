using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Stack'
    /// </summary>
    internal partial class StackTable : Table<Stack>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Message;
        internal RefListColumn Frames;
        internal IColumn<IDictionary<string, string>> Properties;

        internal StackTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            Frames = AddColumn(nameof(Frames), new RefListColumn(nameof(SarifLogDatabase.StackFrame)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override Stack Get(int index)
        {
            return (index == -1 ? null : new Stack(this, index));
        }
    }
}
