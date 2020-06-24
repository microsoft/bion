// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Employee' entity.
    /// </summary>
    internal partial class EmployeeTable : Table<Employee>
    {
        internal CompanyDatabase Database;
        internal IColumn<string> Name;

        internal EmployeeTable(CompanyDatabase database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), database.BuildColumn<string>(nameof(Employee), nameof(Name)));
        }

        public override Employee Get(int index)
        {
            return (index == -1 ? null : new Employee(this, index));
        }
    }
}
