// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  BSOA GENERATED Database for 'Company'
    /// </summary>
    internal partial class CompanyDatabase : Database
    {
        [ThreadStatic]
        private static WeakReference<CompanyDatabase> _lastCreated;

        internal static CompanyDatabase Current => (_lastCreated.TryGetTarget(out CompanyDatabase value) ? value : new CompanyDatabase());
        
        // <TableMemberList>
        internal CompanyTable Company { get; }
        internal EmployeeTable Employee { get; }
        //   <TableMember>
        internal TeamTable Team { get; }
        //   </TableMember>
        // </TableMemberList>

        public CompanyDatabase()
        {
            _lastCreated = new WeakReference<CompanyDatabase>(this);

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
