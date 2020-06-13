using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Tool'
    /// </summary>
    internal partial class ToolTable : Table<Tool>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Driver;
        internal RefListColumn Extensions;
        internal IColumn<IDictionary<string, string>> Properties;

        internal ToolTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Driver = AddColumn(nameof(Driver), new RefColumn(nameof(SarifLogDatabase.ToolComponent)));
            Extensions = AddColumn(nameof(Extensions), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override Tool Get(int index)
        {
            return (index == -1 ? null : new Tool(this, index));
        }
    }
}