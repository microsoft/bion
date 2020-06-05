using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ToolComponent'
    /// </summary>
    internal partial class ToolComponentTable : Table<ToolComponent>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Name;

        internal ToolComponentTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>(null));
        }

        public override ToolComponent Get(int index)
        {
            return (index == -1 ? null : new ToolComponent(this, index));
        }
    }
}
