// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

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
        internal RefColumn Owner;
        internal RefListColumn Members;
        internal RefListColumn Teams;

        internal CompanyTable(CompanyDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<long>(99));
            JoinPolicy = AddColumn(nameof(JoinPolicy), ColumnFactory.Build<byte>((byte)SecurityPolicy.Open));
            Owner = AddColumn(nameof(Owner), new RefColumn(nameof(CompanyDatabase.Employee)));
            Members = AddColumn(nameof(Members), new RefListColumn(nameof(CompanyDatabase.Employee)));
            Teams = AddColumn(nameof(Teams), new RefListColumn(nameof(CompanyDatabase.Team)));
        }

        public override Company Get(int index)
        {
            return (index == -1 ? null : new Company(this, index));
        }
    }
}
