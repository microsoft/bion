using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Tool'
    /// </summary>
    internal partial class ToolTable : Table<Tool>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Driver;
        internal RefListColumn Extensions;

        internal ToolTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Driver = AddColumn(nameof(Driver), new RefColumn(nameof(SarifLogDatabase.ToolComponent)));
            Extensions = AddColumn(nameof(Extensions), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
        }

        public override Tool Get(int index)
        {
            return (index == -1 ? null : new Tool(this, index));
        }
    }
}
