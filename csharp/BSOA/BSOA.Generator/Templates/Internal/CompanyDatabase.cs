// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  BSOA GENERATED Database for 'Company'
    /// </summary>
    internal partial class CompanyDatabase : Database
    {
        internal static CompanyDatabase Current { get; private set; }
        
        // <TableMemberList>
        internal CompanyTable Company { get; }
        internal EmployeeTable Employee { get; }
        //   <TableMember>
        internal TeamTable Team { get; }
        //   </TableMember>
        // </TableMemberList>

        public CompanyDatabase()
        {
            Current = this;

            // <TableConstructorList>
            Company = AddTable(nameof(Company), new CompanyTable(this));
            Employee = AddTable(nameof(Employee), new EmployeeTable(this));
            //   <TableConstructor>
            Team = AddTable(nameof(Team), new TeamTable(this));
            //   </TableConstructor>
            // </TableConstructorList>
        }
    }
}
