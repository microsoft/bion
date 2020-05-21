using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Tool' entity.
    /// </summary>
    public partial class ToolTable : Table<Tool>
    {
        internal SarifLogBsoa Database;

        internal RefColumn Driver;
        internal RefListColumn Extensionss;

        public ToolTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Driver = AddColumn(nameof(Driver), new RefColumn(nameof(SarifLogBsoa.ToolComponent)));
            Extensionss = AddColumn(nameof(Extensionss), new RefListColumn(nameof(SarifLogBsoa.ToolComponent)));
        }

        public override Tool Get(int index)
        {
            return (index == -1 ? null : new Tool(this, index));
        }
    }
}
