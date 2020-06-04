using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: Root Database properties
    /// </summary>
    public partial class CompanyDatabase : Database
    {
        // <ColumnList>
        //  <Column>
        public long EmployeeId
        {
            get => Root[0].EmployeeId;
            set => Root[0].EmployeeId = value;
        }

        // </Column>
        public IList<Employee> Employees
        {
            get => Root[0].Employees;
            set => Root[0].Employees = value;
        }

        public IList<Team> Teams
        {
            get => Root[0].Teams;
            set => Root[0].Teams = value;
        }
        // </ColumnList>
    }
}
