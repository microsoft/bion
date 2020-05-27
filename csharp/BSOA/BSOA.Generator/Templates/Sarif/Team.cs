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

        //public static IEqualityComparer<Team> ValueComparer => TeamEqualityComparer.Instance;
        //public bool ValueEquals(Team other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
