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
    ///  GENERATED: BSOA Entity for 'LocationRelationship'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class LocationRelationship : PropertyBagHolder, ISarifNode, IRow
    {
        private LocationRelationshipTable _table;
        private int _index;

        public LocationRelationship() : this(SarifLogDatabase.Current.LocationRelationship)
        { }

        public LocationRelationship(SarifLog root) : this(root.Database.LocationRelationship)
        { }

        internal LocationRelationship(LocationRelationshipTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal LocationRelationship(LocationRelationshipTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public LocationRelationship(
            int target,
            IList<string> kinds,
            Message description,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.LocationRelationship)
        {
            Target = target;
            Kinds = kinds;
            Description = description;
            Properties = properties;
        }

        public LocationRelationship(LocationRelationship other) 
            : this(SarifLogDatabase.Current.LocationRelationship)
        {
            Target = other.Target;
            Kinds = other.Kinds;
            Description = other.Description;
            Properties = other.Properties;
        }

        [DataMember(Name = "target", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Target
        {
            get => _table.Target[_index];
            set => _table.Target[_index] = value;
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

        #region IEquatable<LocationRelationship>
        public bool Equals(LocationRelationship other)
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
                if (Target != default(int))
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
            return Equals(obj as LocationRelationship);
        }

        public static bool operator ==(LocationRelationship left, LocationRelationship right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(LocationRelationship left, LocationRelationship right)
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
            _table = (LocationRelationshipTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.LocationRelationship;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public LocationRelationship DeepClone()
        {
            return (LocationRelationship)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new LocationRelationship(this);
        }
        #endregion

        public static IEqualityComparer<LocationRelationship> ValueComparer => EqualityComparer<LocationRelationship>.Default;
        public bool ValueEquals(LocationRelationship other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
