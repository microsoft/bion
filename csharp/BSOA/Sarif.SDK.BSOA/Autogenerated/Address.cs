// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Address'
    /// </summary>
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
            Init();
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
            IDictionary<string, SerializedPropertyInfo> properties
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

        partial void Init();

        public int AbsoluteAddress
        {
            get => _table.AbsoluteAddress[_index];
            set => _table.AbsoluteAddress[_index] = value;
        }

        public int RelativeAddress
        {
            get => _table.RelativeAddress[_index];
            set => _table.RelativeAddress[_index] = value;
        }

        public int Length
        {
            get => _table.Length[_index];
            set => _table.Length[_index] = value;
        }

        public string Kind
        {
            get => _table.Kind[_index];
            set => _table.Kind[_index] = value;
        }

        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        public string FullyQualifiedName
        {
            get => _table.FullyQualifiedName[_index];
            set => _table.FullyQualifiedName[_index] = value;
        }

        public int OffsetFromParent
        {
            get => _table.OffsetFromParent[_index];
            set => _table.OffsetFromParent[_index] = value;
        }

        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
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

                if (Properties != default(IDictionary<string, SerializedPropertyInfo>))
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

        void IRow.Next()
        {
            _index++;
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
