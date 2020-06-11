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
    ///  GENERATED: BSOA Entity for 'ReportingDescriptorRelationship'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ReportingDescriptorRelationship : PropertyBagHolder, ISarifNode, IRow
    {
        private ReportingDescriptorRelationshipTable _table;
        private int _index;

        public ReportingDescriptorRelationship() : this(SarifLogDatabase.Current.ReportingDescriptorRelationship)
        { }

        public ReportingDescriptorRelationship(SarifLog root) : this(root.Database.ReportingDescriptorRelationship)
        { }

        internal ReportingDescriptorRelationship(ReportingDescriptorRelationshipTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ReportingDescriptorRelationship(ReportingDescriptorRelationshipTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ReportingDescriptorRelationship(
            ReportingDescriptorReference target,
            IList<string> kinds,
            Message description,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.ReportingDescriptorRelationship)
        {
            Target = target;
            Kinds = kinds;
            Description = description;
            Properties = properties;
        }

        public ReportingDescriptorRelationship(ReportingDescriptorRelationship other) 
            : this(SarifLogDatabase.Current.ReportingDescriptorRelationship)
        {
            Target = other.Target;
            Kinds = other.Kinds;
            Description = other.Description;
            Properties = other.Properties;
        }

        [DataMember(Name = "target", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ReportingDescriptorReference Target
        {
            get => _table.Database.ReportingDescriptorReference.Get(_table.Target[_index]);
            set => _table.Target[_index] = _table.Database.ReportingDescriptorReference.LocalIndex(value);
        }

        [DataMember(Name = "kinds", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<string> Kinds
        {
            get => _table.Kinds[_index];
            set => _table.Kinds[_index] = value;
        }

        [DataMember(Name = "description", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Message Description
        {
            get => _table.Database.Message.Get(_table.Description[_index]);
            set => _table.Description[_index] = _table.Database.Message.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ReportingDescriptorRelationship>
        public bool Equals(ReportingDescriptorRelationship other)
        {
            if (other == null) { return false; }

            if (this.Target != other.Target) { return false; }
            if (this.Kinds != other.Kinds) { return false; }
            if (this.Description != other.Description) { return false; }
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
                if (Target != default(ReportingDescriptorReference))
                {
                    result = (result * 31) + Target.GetHashCode();
                }

                if (Kinds != default(IList<string>))
                {
                    result = (result * 31) + Kinds.GetHashCode();
                }

                if (Description != default(Message))
                {
                    result = (result * 31) + Description.GetHashCode();
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
            return Equals(obj as ReportingDescriptorRelationship);
        }

        public static bool operator ==(ReportingDescriptorRelationship left, ReportingDescriptorRelationship right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ReportingDescriptorRelationship left, ReportingDescriptorRelationship right)
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
            _table = (ReportingDescriptorRelationshipTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ReportingDescriptorRelationship;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ReportingDescriptorRelationship DeepClone()
        {
            return (ReportingDescriptorRelationship)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ReportingDescriptorRelationship(this);
        }
        #endregion

        public static IEqualityComparer<ReportingDescriptorRelationship> ValueComparer => EqualityComparer<ReportingDescriptorRelationship>.Default;
        public bool ValueEquals(ReportingDescriptorRelationship other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
