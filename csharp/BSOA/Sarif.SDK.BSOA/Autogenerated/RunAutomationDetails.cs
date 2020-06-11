// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'RunAutomationDetails'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class RunAutomationDetails : PropertyBagHolder, ISarifNode, IRow
    {
        private RunAutomationDetailsTable _table;
        private int _index;

        public RunAutomationDetails() : this(SarifLogDatabase.Current.RunAutomationDetails)
        { }

        public RunAutomationDetails(SarifLog root) : this(root.Database.RunAutomationDetails)
        { }

        internal RunAutomationDetails(RunAutomationDetailsTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal RunAutomationDetails(RunAutomationDetailsTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public RunAutomationDetails(
            Message description,
            string id,
            string guid,
            string correlationGuid,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.RunAutomationDetails)
        {
            Description = description;
            Id = id;
            Guid = guid;
            CorrelationGuid = correlationGuid;
            Properties = properties;
        }

        public RunAutomationDetails(RunAutomationDetails other) 
            : this(SarifLogDatabase.Current.RunAutomationDetails)
        {
            Description = other.Description;
            Id = other.Id;
            Guid = other.Guid;
            CorrelationGuid = other.CorrelationGuid;
            Properties = other.Properties;
        }

        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        public Message Description
        {
            get => _table.Database.Message.Get(_table.Description[_index]);
            set => _table.Description[_index] = _table.Database.Message.LocalIndex(value);
        }

        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        public string Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        [DataMember(Name = "guid", IsRequired = false, EmitDefaultValue = false)]
        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        [DataMember(Name = "correlationGuid", IsRequired = false, EmitDefaultValue = false)]
        public string CorrelationGuid
        {
            get => _table.CorrelationGuid[_index];
            set => _table.CorrelationGuid[_index] = value;
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<RunAutomationDetails>
        public bool Equals(RunAutomationDetails other)
        {
            if (other == null) { return false; }

            if (this.Description != other.Description) { return false; }
            if (this.Id != other.Id) { return false; }
            if (this.Guid != other.Guid) { return false; }
            if (this.CorrelationGuid != other.CorrelationGuid) { return false; }
            if (this.Properties != other.Properties) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Description != default(Message))
                {
                    result = (result * 31) + Description.GetHashCode();
                }

                if (Id != default(string))
                {
                    result = (result * 31) + Id.GetHashCode();
                }

                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (CorrelationGuid != default(string))
                {
                    result = (result * 31) + CorrelationGuid.GetHashCode();
                }

                if (Properties != default(IDictionary<string, string>))
                {
                    result = (result * 31) + Properties.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RunAutomationDetails);
        }

        public static bool operator ==(RunAutomationDetails left, RunAutomationDetails right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(RunAutomationDetails left, RunAutomationDetails right)
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
            _table = (RunAutomationDetailsTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.RunAutomationDetails;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public RunAutomationDetails DeepClone()
        {
            return (RunAutomationDetails)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new RunAutomationDetails(this);
        }
        #endregion

        public static IEqualityComparer<RunAutomationDetails> ValueComparer => EqualityComparer<RunAutomationDetails>.Default;
        public bool ValueEquals(RunAutomationDetails other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
