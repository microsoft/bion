using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Team' entity.
    /// </summary>
    public partial class TeamTable : Table<Team>
    {
        internal CompanyDatabase Database;

        // <ColumnMembers>
        internal IColumn<DateTime> WhenFormed;
        internal RefColumn Manager;
        internal RefListColumn Members;
        // </ColumnMembers>

        public TeamTable(CompanyDatabase database) : base()
        {
            Database = database;

            // <ColumnConstructors>

            // <SimpleColumnConstructor>
            WhenFormed = AddColumn(nameof(WhenFormed), ColumnFactory.Build<DateTime>(DateTime.MinValue));
            // </SimpleColumnConstructor>

            // <RefColumnConstructor>
            Manager = AddColumn(nameof(Manager), new RefColumn(nameof(CompanyDatabase.Employee)));
            // </RefColumnConstructor>

            // <RefListColumnConstructor>
            Members = AddColumn(nameof(Members), new RefListColumn(nameof(CompanyDatabase.Employee)));
            // </RefListColumnConstructor>

            // </ColumnConstructors>
        }

        public override Team Get(int index)
        {
            return (index == -1 ? null : new Team(this, index));
        }
    }
}
