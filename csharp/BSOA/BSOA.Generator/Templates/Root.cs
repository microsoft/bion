using System;
using System.Collections.Generic;

using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Entity for root 'CompanyDatabase' item properties.
    /// </summary>
    internal partial class Root : IRow, IEquatable<Root>
    {
        private RootTable _table;
        private int _index;

        internal Root(RootTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Root(RootTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Root(CompanyDatabase database) : this(database.Root)
        { }

        public Root() : this(CompanyDatabase.Current)
        { }

        // <ColumnList>
        public long EmployeeId
        {
            get => _table.EmployeeId[_index];
            set => _table.EmployeeId[_index] = value;
        }

        public IList<Team> Teams
        {
            get => _table.Database.Team.List(_table.Teams[_index]);
            set => _table.Database.Team.List(_table.Teams[_index]).SetTo(value);
        }

        //   <RefListColumn>
        public IList<Employee> Employees
        {
            get => _table.Database.Employee.List(_table.Employees[_index]);
            set => _table.Database.Employee.List(_table.Employees[_index]).SetTo(value);
        }

        //   </RefListColumn>
        // </ColumnList>

        #region IEquatable<CompanyDatabaseItem>
        public bool Equals(Root other)
        {
            if (other == null) { return false; }

            // <EqualsList>
            //  <Equals>
            if (this.Teams != other.Teams) { return false; }
            //  </Equals>
            if (this.Employees != other.Employees) { return false; }
            // </EqualsList>

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                // <GetHashCodeList>
                //  <GetHashCode>
                if (Teams != default(IList<Team>))
                {
                    result = (result * 31) + Teams.GetHashCode();
                }

                //  </GetHashCode>
                if (Employees != default(IList<Employee>))
                {
                    result = (result * 31) + Employees.GetHashCode();
                }
                // </GetHashCodeList>
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Root);
        }

        public static bool operator ==(Root left, Root right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Root left, Root right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return !object.ReferenceEquals(right, null);
            }

            return !left.Equals(right);
        }
        #endregion

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (RootTable)table;
            _index = index;
        }
        #endregion
    }
}
