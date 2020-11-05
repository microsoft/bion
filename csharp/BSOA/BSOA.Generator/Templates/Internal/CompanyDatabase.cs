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
        // <TableMemberList>
        internal CompanyTable Company;
        internal EmployeeTable Employee;
        //   <TableMember>
        internal TeamTable Team;
        //   </TableMember>
        // </TableMemberList>

        public CompanyDatabase() : base("Company")
        {
            _lastCreated = new WeakReference<CompanyDatabase>(this);
            GetOrBuildTables();
        }

        public override void GetOrBuildTables()
        {
            // <TableConstructorList>
            Company = GetOrBuild(nameof(Company), () => new CompanyTable(this));
            Employee = GetOrBuild(nameof(Employee), () => new EmployeeTable(this));
            //   <TableConstructor>
            Team = GetOrBuild(nameof(Team), () => new TeamTable(this));
            //   </TableConstructor>
            // </TableConstructorList>
        }

        [ThreadStatic]
        private static WeakReference<CompanyDatabase> _lastCreated;

        internal static CompanyDatabase Current
        {
            get
            {
                CompanyDatabase db;
                if (_lastCreated == null || !_lastCreated.TryGetTarget(out db)) { db = new CompanyDatabase(); }
                return db;
            }
        }
    }
}
