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
    ///  GENERATED: BSOA Entity for 'ExternalPropertyFileReference'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ExternalPropertyFileReference : PropertyBagHolder, ISarifNode, IRow
    {
        private ExternalPropertyFileReferenceTable _table;
        private int _index;

        public ExternalPropertyFileReference() : this(SarifLogDatabase.Current.ExternalPropertyFileReference)
        { }

        public ExternalPropertyFileReference(SarifLog root) : this(root.Database.ExternalPropertyFileReference)
        { }

        internal ExternalPropertyFileReference(ExternalPropertyFileReferenceTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal ExternalPropertyFileReference(ExternalPropertyFileReferenceTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ExternalPropertyFileReference(
            ArtifactLocation location,
            string guid,
            int itemCount,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.ExternalPropertyFileReference)
        {
            Location = location;
            Guid = guid;
            ItemCount = itemCount;
            Properties = properties;
        }

        public ExternalPropertyFileReference(ExternalPropertyFileReference other) 
            : this(SarifLogDatabase.Current.ExternalPropertyFileReference)
        {
            Location = other.Location;
            Guid = other.Guid;
            ItemCount = other.ItemCount;
            Properties = other.Properties;
        }

        partial void Init();

        public ArtifactLocation Location
        {
            get => _table.Database.ArtifactLocation.Get(_table.Location[_index]);
            set => _table.Location[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        public int ItemCount
        {
            get => _table.ItemCount[_index];
            set => _table.ItemCount[_index] = value;
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ExternalPropertyFileReference>
        public bool Equals(ExternalPropertyFileReference other)
        {
            if (other == null) { return false; }

            if (this.Location != other.Location) { return false; }
            if (this.Guid != other.Guid) { return false; }
            if (this.ItemCount != other.ItemCount) { return false; }
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
                if (Location != default(ArtifactLocation))
                {
                    result = (result * 31) + Location.GetHashCode();
                }

                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (ItemCount != default(int))
                {
                    result = (result * 31) + ItemCount.GetHashCode();
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
            return Equals(obj as ExternalPropertyFileReference);
        }

        public static bool operator ==(ExternalPropertyFileReference left, ExternalPropertyFileReference right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ExternalPropertyFileReference left, ExternalPropertyFileReference right)
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
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ExternalPropertyFileReference;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ExternalPropertyFileReference DeepClone()
        {
            return (ExternalPropertyFileReference)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ExternalPropertyFileReference(this);
        }
        #endregion

        public static IEqualityComparer<ExternalPropertyFileReference> ValueComparer => EqualityComparer<ExternalPropertyFileReference>.Default;
        public bool ValueEquals(ExternalPropertyFileReference other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
