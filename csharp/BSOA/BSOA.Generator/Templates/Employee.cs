// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Reflection;

using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Employee'
    /// </summary>
    public partial class Employee : IRow
    {
        private readonly EmployeeTable _table;
        private readonly int _index;

        public Employee() : this(CompanyDatabase.Current.Employee)
        { }

        public Employee(Company root) : this(root.Database.Employee)
        { }

        public Employee(Company root, Employee other) : this(root.Database.Employee, other)
        { }

        internal Employee(EmployeeTable table) : this(table, table.Add()._index)
        { }

        internal Employee(EmployeeTable table, Employee other) : this(table ?? CompanyDatabase.Current.Employee)
        {
            CopyFrom(other);
        }

        internal Employee(EmployeeTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        #region IRow
        ITable IRow<Employee>.Table => _table;
        int IRow<Employee>.Index => _index;

        public void CopyFrom(Employee other)
        {
            Name = other.Name;
        }
        #endregion
    }
}
