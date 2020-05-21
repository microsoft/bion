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
        // <SimpleColumnMember>
        internal IColumn<DateTime> WhenFormed;
        // </SimpleColumnMember>
        // <EnumColumnMember>
        internal IColumn<byte> JoinPolicy;
        // </EnumColumnMember>
        // <RefColumnMember>
        internal RefColumn Manager;
        // </RefColumnMember>
        // <RefListColumnMember>
        internal RefListColumn Members;
        // </RefListColumnMember>
        // </ColumnMembers>

        public TeamTable(CompanyDatabase database) : base()
        {
            Database = database;

            // <ColumnConstructors>

            // <SimpleColumnConstructor>
            WhenFormed = AddColumn(nameof(WhenFormed), ColumnFactory.Build<DateTime>(DateTime.MinValue));
            // </SimpleColumnConstructor>

            // <EnumColumnConstructor>
            JoinPolicy = AddColumn(nameof(JoinPolicy), ColumnFactory.Build<byte>((byte)SecurityPolicy.Open));
            // </EnumColumnConstructor>

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
