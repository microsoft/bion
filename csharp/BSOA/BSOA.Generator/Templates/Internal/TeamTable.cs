// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

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
            Id = AddColumn(nameof(Id), database.BuildColumn<long>(nameof(Team), nameof(Id), 99));
            // </SimpleColumnConstructor>

            // <EnumColumnConstructor>
            JoinPolicy = AddColumn(nameof(JoinPolicy), database.BuildColumn<byte>(nameof(Team), nameof(JoinPolicy), (byte)SecurityPolicy.Open));
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
