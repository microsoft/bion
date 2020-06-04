using System;
using System.Collections;

using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Employee'
    /// </summary>
    public partial class Employee : IRow
    {
        private EmployeeTable _table;
        private int _index;

        internal Employee(EmployeeTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Employee(EmployeeTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Employee(CompanyDatabase database) : this(database.Employee)
        { }

        public Employee() : this(CompanyDatabase.Current)
        { }

        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (EmployeeTable)table;
            _index = index;
        }
        #endregion
    }
}
