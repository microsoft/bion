// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

using BSOA.Collections;
using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  BSOA GENERATED Entity for 'Team'
    /// </summary>
    public partial class Team : IRow<Team>, IEquatable<Team>
    {
        private TeamTable _table;
        private int _index;

        public Team() : this(CompanyDatabase.Current.Team)
        { }

        public Team(Company root) : this(root.Database.Team)
        { }

        public Team(Company root, Team other) : this(root.Database.Team)
        {
            CopyFrom(other);
        }

        internal Team(CompanyDatabase database, Team other) : this(database.Team)
        {
            CopyFrom(other);
        }

        internal Team(TeamTable table) : this(table, table.Add()._index)
        {
            Init();
        }

        internal Team(TeamTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        partial void Init();

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
        private TypedList<Employee> _members;
        public IList<Employee> Members
        {
            get
            {
                if (_members == null) { _members = TypedList<Employee>.Get(_table.Database.Employee, _table.Members, _index); }
                return _members;
            }
            set
            {
                TypedList<Employee>.Set(_table.Database.Employee, _table.Members, _index, value);
                _members = null;
            }
        }

        //   </RefListColumn>
        // </ColumnList>

        #region IEquatable<Team>
        public bool Equals(Team other)
        {
            if (other == null) { return false; }

            // <EqualsList>
            //  <Equals>
            if (!object.Equals(this.Id, other.Id)) { return false; }
            //  </Equals>
            if (!object.Equals(this.JoinPolicy, other.JoinPolicy)) { return false; }
            if (!object.Equals(this.Owner, other.Owner)) { return false; }
            if (!object.Equals(this.Members, other.Members)) { return false; }
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
        ITable IRow<Team>.Table => _table;
        int IRow<Team>.Index => _index;

        void IRow<Team>.Remap(ITable table, int index)
        {
            _table = (TeamTable)table;
            _index = index;
        }

        public void CopyFrom(Team other)
        {
            // <OtherAssignmentList>
            //  <OtherAssignment>
            Id = other.Id;
            //  </OtherAssignment>
            JoinPolicy = other.JoinPolicy;
            //  <RefOtherAssignment>
            Owner = Employee.Copy(_table.Database, other.Owner);
            //  </RefOtherAssignment>
            //  <RefListOtherAssignment>
            Members = other.Members?.Select((item) => Employee.Copy(_table.Database, item)).ToList();
            //  </RefListOtherAssignment>
            // </OtherAssignmentList>
        }

        internal static Team Copy(CompanyDatabase database, Team other)
        {
            return (other == null ? null : new Team(database, other));
        }
        #endregion
    }
}
