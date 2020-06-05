using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Team'
    /// </summary>
    internal partial class TeamTable : Table<Team>
    {
        internal CompanyDatabase Database;

        // <ColumnMemberList>
        // <SimpleColumnMember>
        internal IColumn<long> Id;
        // </SimpleColumnMember>
        // <EnumColumnMember>
        internal IColumn<byte> JoinPolicy;
        // </EnumColumnMember>
        // <RefColumnMember>
        internal RefColumn Owner;
        // </RefColumnMember>
        // <RefListColumnMember>
        internal RefListColumn Members;
        // </RefListColumnMember>
        // </ColumnMemberList>

        internal TeamTable(CompanyDatabase database) : base()
        {
            Database = database;

            // <ColumnConstructorList>

            // <SimpleColumnConstructor>
            Id = AddColumn(nameof(Id), ColumnFactory.Build<long>(99));
            // </SimpleColumnConstructor>

            // <EnumColumnConstructor>
            JoinPolicy = AddColumn(nameof(JoinPolicy), ColumnFactory.Build<byte>((byte)SecurityPolicy.Open));
            // </EnumColumnConstructor>

            // <RefColumnConstructor>
            Owner = AddColumn(nameof(Owner), new RefColumn(nameof(CompanyDatabase.Employee)));
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
