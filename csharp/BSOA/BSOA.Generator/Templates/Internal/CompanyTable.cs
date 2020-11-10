// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Collections;
using BSOA.Column;
using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Company'
    /// </summary>
    public partial class CompanyTable : Table<Company>
    {
        internal CompanyDatabase Database;

        internal IColumn<long> Id;
        internal IColumn<byte> JoinPolicy;
        internal IColumn<int> Owner;
        internal IColumn<NumberList<int>> Members;
        internal IColumn<NumberList<int>> Teams;

        internal CompanyTable(IDatabase database, Dictionary<string, IColumn> columns = null) : base(database, columns)
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
            Owner = GetOrBuild(nameof(Owner), () => (IColumn<int>)new RefColumn(nameof(CompanyDatabase.Employee)));
            // </RefColumnConstructor>
            // <RefListColumnConstructor>
            Members = GetOrBuild(nameof(Members), () => (IColumn<NumberList<int>>)new RefListColumn(nameof(CompanyDatabase.Employee)));
            // </RefListColumnConstructor>
            // </ColumnConstructorList>
        }

        public override Company Get(int index)
        {
            return (index == -1 ? null : new Company(this, index));
        }
    }
}
