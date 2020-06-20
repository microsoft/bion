// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'Team'
    /// </summary>
    public partial class Team : IRow, IEquatable<Team>
    {
        private TeamTable _table;
        private int _index;

        public Team() : this(CompanyDatabase.Current.Team)
        { }

        public Team(Company root) : this(root.Database.Team)
        { }

        internal Team(TeamTable table) : this(table, table.Count)
        {
            table.Add();
        }
        
        internal Team(TeamTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Team(
            // <ArgumentList>
            //  <Argument>
            long id,
            //  </Argument>
            SecurityPolicy joinPolicy,
            Employee owner,
            IList<Employee> members
            // </ArgumentList>
        ) 
            : this(CompanyDatabase.Current.Team)
        {
            // <AssignmentList>
            //  <Assignment>
            Id = id;
            //  </Assignment>
            JoinPolicy = joinPolicy;
            Owner = owner;
            Members = members;
            // </AssignmentList>
        }

        public Team(Team other) 
            : this(CompanyDatabase.Current.Team)
        {
            // <OtherAssignmentList>
            //  <OtherAssignment>
            Id = other.Id;
            //  </OtherAssignment>
            JoinPolicy = other.JoinPolicy;
            Owner = other.Owner;
            Members = other.Members;
            // </OtherAssignmentList>
        }

        // <ColumnList>
        //   <SimpleColumn>
        public long Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        //   </SimpleColumn>
        //   <EnumColumn>
        public SecurityPolicy JoinPolicy
        {
            get => (SecurityPolicy)_table.JoinPolicy[_index];
            set => _table.JoinPolicy[_index] = (byte)value;
        }

        //   </EnumColumn>
        //   <RefColumn>
        public Employee Owner
        {
            get => _table.Database.Employee.Get(_table.Owner[_index]);
            set => _table.Owner[_index] = _table.Database.Employee.LocalIndex(value);
        }

        //   </RefColumn>
        //   <RefListColumn>
        public IList<Employee> Members
        {
            get => _table.Database.Employee.List(_table.Members[_index]);
            set => _table.Database.Employee.List(_table.Members[_index]).SetTo(value);
        }

        //   </RefListColumn>
        // </ColumnList>

        #region IEquatable<Team>
        public bool Equals(Team other)
        {
            if (other == null) { return false; }

            // <EqualsList>
            //  <Equals>
            if (this.Id != other.Id) { return false; }
            //  </Equals>
            if (this.JoinPolicy != other.JoinPolicy) { return false; }
            if (this.Owner != other.Owner) { return false; }
            if (this.Members != other.Members) { return false; }
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
                if (Id != default(long))
                {
                    result = (result * 31) + Id.GetHashCode();
                }

                //  </GetHashCode>
                if (JoinPolicy != default(SecurityPolicy))
                {
                    result = (result * 31) + JoinPolicy.GetHashCode();
                }

                if (Owner != default(Employee))
                {
                    result = (result * 31) + Owner.GetHashCode();
                }

                if (Members != default(IList<Employee>))
                {
                    result = (result * 31) + Members.GetHashCode();
                }
                // </GetHashCodeList>
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Team);
        }

        public static bool operator ==(Team left, Team right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Team left, Team right)
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

        void IRow.Next()
        {
            _index++;
        }
        #endregion
    }
}
