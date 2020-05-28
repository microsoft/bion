// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Team'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Team : PropertyBagHolder, ISarifNode, IRow
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
            // <ArgumentList>
            //  <Argument>
            long employeeId,
            //  </Argument>
            DateTime whenFormed,
            SecurityPolicy joinPolicy,
            GroupAttributes attributes,
            Employee manager,
            IList<Employee> members
        // </ArgumentList>
        ) : this(CompanyDatabase.Current)
        {
            // <AssignmentList>
            //  <Assignment>
            EmployeeId = employeeId;
            //  </Assignment>
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
            //  <OtherAssignment>
            EmployeeId = other.EmployeeId;
            //  </OtherAssignment>
            WhenFormed = other.WhenFormed;
            JoinPolicy = other.JoinPolicy;
            Attributes = other.Attributes;
            Manager = other.Manager;
            Members = other.Members;
            // </OtherAssignmentList>
        }

        // <Columns>
        //   <SimpleColumn>
        [DataMember(Name = "employeeId", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public long EmployeeId
        {
            get => _table.EmployeeId[_index];
            set => _table.EmployeeId[_index] = value;
        }
        //   </SimpleColumn>

        //   <DateTimeColumn>
        [DataMember(Name = "whenFormed", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.DateTimeConverter))]
        public DateTime WhenFormed
        {
            get => _table.WhenFormed[_index];
            set => _table.WhenFormed[_index] = value;
        }
        //   </DateTimeColumn>

        //   <EnumColumn>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.EnumConverter))]
        public SecurityPolicy JoinPolicy
        {
            get => (SecurityPolicy)_table.JoinPolicy[_index];
            set => _table.JoinPolicy[_index] = (byte)value;
        }
        //   </EnumColumn>

        //   <EnumColumn>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.FlagsEnumConverter))]
        public GroupAttributes Attributes
        {
            get => (GroupAttributes)_table.Attributes[_index];
            set => _table.Attributes[_index] = (long)value;
        }
        //   </EnumColumn>

        //   <RefColumn>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Employee Manager
        {
            get => _table.Database.Employee.Get(_table.Manager[_index]);
            set => _table.Manager[_index] = _table.Database.Employee.LocalIndex(value);
        }
        //   </RefColumn>

        //   <RefListColumn>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public IList<Employee> Members
        {
            get => _table.Database.Employee.List(_table.Members[_index]);
            set => _table.Database.Employee.List(_table.Members[_index]).SetTo(value);
        }
        //   </RefListColumn>
        // </Columns>

        #region IEquatable<Team>
        public bool Equals(Team other)
        {
            if (other == null) { return false; }

            // <EqualsList>
            //  <Equals>
            if (this.EmployeeId != other.EmployeeId) { return false; }
            //  </Equals>
            if (this.WhenFormed != other.WhenFormed) { return false; }
            if (this.JoinPolicy != other.JoinPolicy) { return false; }
            if (this.Attributes != other.Attributes) { return false; }
            if (this.Manager != other.Manager) { return false; }
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
                if (EmployeeId != default(long))
                {
                    result = (result * 31) + EmployeeId.GetHashCode();
                }

                //  </GetHashCode>
                if (WhenFormed != default(DateTime))
                {
                    result = (result * 31) + WhenFormed.GetHashCode();
                }

                if (JoinPolicy != default(SecurityPolicy))
                {
                    result = (result * 31) + JoinPolicy.GetHashCode();
                }

                if (Attributes != default(GroupAttributes))
                {
                    result = (result * 31) + Attributes.GetHashCode();
                }

                if (Manager != default(Employee))
                {
                    result = (result * 31) + Manager.GetHashCode();
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
            return (left == null ? right == null : left.Equals(right));
        }

        public static bool operator !=(Team left, Team right)
        {
            return (left == null ? right != null : !(left.Equals(right)));
        }
        #endregion

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (TeamTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Team;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Team DeepClone()
        {
            return (Team)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Team(this);
        }
        #endregion

        public static IEqualityComparer<Team> ValueComparer => EqualityComparer<Team>.Default;
        public bool ValueEquals(Team other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
