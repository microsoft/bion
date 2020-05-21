using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'ToolComponent' entity.
    /// </summary>
    public partial class ToolComponentTable : Table<ToolComponent>
    {
        internal SarifLogBsoa Database;

        internal IColumn<string> Name;

        public ToolComponentTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>());
        }

        public override ToolComponent Get(int index)
        {
            return (index == -1 ? null : new ToolComponent(this, index));
        }
    }
}
