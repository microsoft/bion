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
    ///  GENERATED: BSOA Entity for 'Address'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Address : PropertyBagHolder, ISarifNode, IRow
    {
        private AddressTable _table;
        private int _index;

        public Address() : this(SarifLogDatabase.Current.Address)
        { }

        public Address(SarifLog root) : this(root.Database.Address)
        { }

        internal Address(AddressTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Address(AddressTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Address(
            int absoluteAddress,
            int relativeAddress,
            int length,
            string kind,
            string name,
            string fullyQualifiedName,
            int offsetFromParent,
            int index,
            int parentIndex,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.Address)
        {
            AbsoluteAddress = absoluteAddress;
            RelativeAddress = relativeAddress;
            Length = length;
            Kind = kind;
            Name = name;
            FullyQualifiedName = fullyQualifiedName;
            OffsetFromParent = offsetFromParent;
            Index = index;
            ParentIndex = parentIndex;
            Properties = properties;
        }

        public Address(Address other) 
            : this(SarifLogDatabase.Current.Address)
        {
            AbsoluteAddress = other.AbsoluteAddress;
            RelativeAddress = other.RelativeAddress;
            Length = other.Length;
            Kind = other.Kind;
            Name = other.Name;
            FullyQualifiedName = other.FullyQualifiedName;
            OffsetFromParent = other.OffsetFromParent;
            Index = other.Index;
            ParentIndex = other.ParentIndex;
            Properties = other.Properties;
        }

        [DataMember(Name = "absoluteAddress", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int AbsoluteAddress
        {
            get => _table.AbsoluteAddress[_index];
            set => _table.AbsoluteAddress[_index] = value;
        }

        [DataMember(Name = "relativeAddress", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int RelativeAddress
        {
            get => _table.RelativeAddress[_index];
            set => _table.RelativeAddress[_index] = value;
        }

        [DataMember(Name = "length", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Length
        {
            get => _table.Length[_index];
            set => _table.Length[_index] = value;
        }

        [DataMember(Name = "kind", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Kind
        {
            get => _table.Kind[_index];
            set => _table.Kind[_index] = value;
        }

        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        [DataMember(Name = "fullyQualifiedName", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FullyQualifiedName
        {
            get => _table.FullyQualifiedName[_index];
            set => _table.FullyQualifiedName[_index] = value;
        }

        [DataMember(Name = "offsetFromParent", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int OffsetFromParent
        {
            get => _table.OffsetFromParent[_index];
            set => _table.OffsetFromParent[_index] = value;
        }

        [DataMember(Name = "index", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        [DataMember(Name = "parentIndex", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Address>
        public bool Equals(Address other)
        {
            if (other == null) { return false; }

            if (this.AbsoluteAddress != other.AbsoluteAddress) { return false; }
            if (this.RelativeAddress != other.RelativeAddress) { return false; }
            if (this.Length != other.Length) { return false; }
            if (this.Kind != other.Kind) { return false; }
            if (this.Name != other.Name) { return false; }
            if (this.FullyQualifiedName != other.FullyQualifiedName) { return false; }
            if (this.OffsetFromParent != other.OffsetFromParent) { return false; }
            if (this.Index != other.Index) { return false; }
            if (this.ParentIndex != other.ParentIndex) { return false; }
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
                if (AbsoluteAddress != default(int))
                {
                    result = (result * 31) + AbsoluteAddress.GetHashCode();
                }

                if (RelativeAddress != default(int))
                {
                    result = (result * 31) + RelativeAddress.GetHashCode();
                }

                if (Length != default(int))
                {
                    result = (result * 31) + Length.GetHashCode();
                }

                if (Kind != default(string))
                {
                    result = (result * 31) + Kind.GetHashCode();
                }

                if (Name != default(string))
                {
                    result = (result * 31) + Name.GetHashCode();
                }

                if (FullyQualifiedName != default(string))
                {
                    result = (result * 31) + FullyQualifiedName.GetHashCode();
                }

                if (OffsetFromParent != default(int))
                {
                    result = (result * 31) + OffsetFromParent.GetHashCode();
                }

                if (Index != default(int))
                {
                    result = (result * 31) + Index.GetHashCode();
                }

                if (ParentIndex != default(int))
                {
                    result = (result * 31) + ParentIndex.GetHashCode();
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
            return Equals(obj as Address);
        }

        public static bool operator ==(Address left, Address right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Address left, Address right)
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
            _table = (AddressTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Address;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Address DeepClone()
        {
            return (Address)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Address(this);
        }
        #endregion

        public static IEqualityComparer<Address> ValueComparer => EqualityComparer<Address>.Default;
        public bool ValueEquals(Address other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
