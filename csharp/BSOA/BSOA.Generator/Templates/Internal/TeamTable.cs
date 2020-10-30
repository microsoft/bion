// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Collections;
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
        internal IColumn<int> Owner;
        // </RefColumnMember>
        // <RefListColumnMember>
        internal IColumn<NumberList<int>> Members;
        // </RefListColumnMember>
        // </ColumnMemberList>

        internal TeamTable(IDatabase database, Dictionary<string, IColumn> columns = null) : base(database, columns)
        {
            Database = (CompanyDatabase)database;
            GetOrBuildColumns();
        }

        public override void GetOrBuildColumns()
        {
            // <ColumnConstructorList>

            // <SimpleColumnConstructor>
            Id = GetOrBuild(nameof(Id), () => Database.BuildColumn<long>(nameof(Team), nameof(Id), 99));
            // </SimpleColumnConstructor>

            // <EnumColumnConstructor>
            JoinPolicy = GetOrBuild(nameof(JoinPolicy), () => Database.BuildColumn<byte>(nameof(Team), nameof(JoinPolicy), (byte)SecurityPolicy.Open));
            // </EnumColumnConstructor>

            // <RefColumnConstructor>
            Owner = GetOrBuild(nameof(Owner), () => new RefColumn(nameof(CompanyDatabase.Employee)));
            // </RefColumnConstructor>

            // <RefListColumnConstructor>
            Members = GetOrBuild(nameof(Members), () => new RefListColumn(nameof(CompanyDatabase.Employee)));
            // </RefListColumnConstructor>

            // </ColumnConstructorList>
        }

        public override Team Get(int index)
        {
            return (index == -1 ? null : new Team(this, index));
        }
    }
}
