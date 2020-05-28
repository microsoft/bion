using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Team'
    /// </summary>
    public partial class Team : IRow
    {
        private TeamTable _table;
        private int _index;

        internal Team(TeamTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Team(TeamTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Team(CompanyDatabase database) : this(database.Team)
        { }

        public Team() : this(CompanyDatabase.Current)
        { }

        public Team(
            // <ParameterList>
            long employeeId,
            DateTime whenFormed,
            SecurityPolicy joinPolicy,
            GroupAttributes attributes,
            Employee manager,
            IList<Employee> members
            // </ParameterList>
        ) : this(CompanyDatabase.Current)
        {
            // <AssignmentList>
            EmployeeId = employeeId;
            WhenFormed = whenFormed;
            JoinPolicy = joinPolicy;
            Attributes = attributes;
            Manager = manager;
            Members = members;
            // </AssignmentList>
        }

        public Team(Team other)
        {
            // <OtherAssignmentList>
            EmployeeId = other.EmployeeId;
            WhenFormed = other.WhenFormed;
            JoinPolicy = other.JoinPolicy;
            Attributes = other.Attributes;
            Manager = other.Manager;
            Members = other.Members;
            // </OtherAssignmentList>
        }

        // <Columns>
        //   <SimpleColumn>
        public long EmployeeId
        {
            get => _table.EmployeeId[_index];
            set => _table.EmployeeId[_index] = value;
        }
        //   </SimpleColumn>

        //   <DateTimeColumn>
        public DateTime WhenFormed
        {
            get => _table.WhenFormed[_index];
            set => _table.WhenFormed[_index] = value;
        }
        //   </DateTimeColumn>

        //   <EnumColumn>
        public SecurityPolicy JoinPolicy
        {
            get => (SecurityPolicy)_table.JoinPolicy[_index];
            set => _table.JoinPolicy[_index] = (byte)value;
        }
        //   </EnumColumn>

        //   <FlagsEnumColumn>
        public GroupAttributes Attributes
        {
            get => (GroupAttributes)_table.Attributes[_index];
            set => _table.Attributes[_index] = (long)value;
        }
        //   </FlagsEnumColumn>

        //   <RefColumn>
        public Employee Manager
        {
            get => _table.Database.Employee.Get(_table.Manager[_index]);
            set => _table.Manager[_index] = _table.Database.Employee.LocalIndex(value);
        }
        //   </RefColumn>

        //   <RefListColumn>
        public IList<Employee> Members
        {
            get => _table.Database.Employee.List(_table.Members[_index]);
            set => _table.Database.Employee.List(_table.Members[_index]).SetTo(value);
        }
        //   </RefListColumn>
        // </Columns>

        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (EmployeeId != default(long))
                {
                    result = (result * 31) + EmployeeId.GetHashCode();
                }

                result = (result * 31) + WhenFormed.GetHashCode();
                result = (result * 31) + JoinPolicy.GetHashCode();
                result = (result * 31) + Attributes.GetHashCode();

                if (Manager != default(Employee))
                {
                    result = (result * 31) + Manager.GetHashCode();
                }

                if (Members != null)
                {
                    result = (result * 31) + Members.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            Team other = obj as Team;
            if (other == null) { return false; }

            if (this.EmployeeId != other.EmployeeId) { return false; }
            if (this.WhenFormed != other.WhenFormed) { return false; }
            if (this.JoinPolicy != other.JoinPolicy) { return false; }
            if (this.Attributes != other.Attributes) { return false; }
            if (this.Manager != other.Manager) { return false; }
            if (this.Members != other.Members) { return false; }

            return true;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (TeamTable)table;
            _index = index;
        }
        #endregion
    }
}
