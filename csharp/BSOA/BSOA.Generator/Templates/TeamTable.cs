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

        // <ColumnMemberList>
        // <SimpleColumnMember>
        internal IColumn<long> EmployeeId;
        // </SimpleColumnMember>
        // <DateTimeColumnMember>
        internal IColumn<DateTime> WhenFormed;
        // </DateTimeColumnMember>
        // <EnumColumnMember>
        internal IColumn<byte> JoinPolicy;
        // </EnumColumnMember>
        // <FlagsEnumColumnMember>
        internal IColumn<long> Attributes;
        // </FlagsEnumColumnMember>
        // <RefColumnMember>
        internal RefColumn Manager;
        // </RefColumnMember>
        // <RefListColumnMember>
        internal RefListColumn Members;
        // </RefListColumnMember>
        // </ColumnMemberList>

        public TeamTable(CompanyDatabase database) : base()
        {
            Database = database;

            // <ColumnConstructorList>

            // <SimpleColumnConstructor>
            EmployeeId = AddColumn(nameof(EmployeeId), ColumnFactory.Build<long>(-1));
            // </SimpleColumnConstructor>

            // <DateTimeColumnConstructor>
            WhenFormed = AddColumn(nameof(WhenFormed), ColumnFactory.Build<DateTime>(DateTime.MinValue));
            // </DateTimeColumnConstructor>

            // <EnumColumnConstructor>
            JoinPolicy = AddColumn(nameof(JoinPolicy), ColumnFactory.Build<byte>((byte)SecurityPolicy.Open));
            // </EnumColumnConstructor>

            // <FlagsEnumColumnConstructor>
            Attributes = AddColumn(nameof(Attributes), ColumnFactory.Build<long>((long)GroupAttributes.None));
            // </FlagsEnumColumnConstructor>

            // <RefColumnConstructor>
            Manager = AddColumn(nameof(Manager), new RefColumn(nameof(CompanyDatabase.Employee)));
            // </RefColumnConstructor>

            // <RefListColumnConstructor>
            Members = AddColumn(nameof(Members), new RefListColumn(nameof(CompanyDatabase.Employee)));
            // </RefListColumnConstructor>

            // </ColumnConstructorList>
        }

        public override Team Get(int index)
        {
            return (index == -1 ? null : new Team(this, index));
        }
    }
}
