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
    ///  GENERATED: BSOA Entity for 'SpecialLocations'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class SpecialLocations : PropertyBagHolder, ISarifNode, IRow
    {
        private SpecialLocationsTable _table;
        private int _index;

        public SpecialLocations() : this(SarifLogDatabase.Current.SpecialLocations)
        { }

        public SpecialLocations(SarifLog root) : this(root.Database.SpecialLocations)
        { }

        internal SpecialLocations(SpecialLocationsTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal SpecialLocations(SpecialLocationsTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public SpecialLocations(
            ArtifactLocation displayBase,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.SpecialLocations)
        {
            DisplayBase = displayBase;
            Properties = properties;
        }

        public SpecialLocations(SpecialLocations other) 
            : this(SarifLogDatabase.Current.SpecialLocations)
        {
            DisplayBase = other.DisplayBase;
            Properties = other.Properties;
        }

        partial void Init();

        public ArtifactLocation DisplayBase
        {
            get => _table.Database.ArtifactLocation.Get(_table.DisplayBase[_index]);
            set => _table.DisplayBase[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<SpecialLocations>
        public bool Equals(SpecialLocations other)
        {
            if (other == null) { return false; }

            if (this.DisplayBase != other.DisplayBase) { return false; }
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
                if (DisplayBase != default(ArtifactLocation))
                {
                    result = (result * 31) + DisplayBase.GetHashCode();
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
            return Equals(obj as SpecialLocations);
        }

        public static bool operator ==(SpecialLocations left, SpecialLocations right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(SpecialLocations left, SpecialLocations right)
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
        public SarifNodeKind SarifNodeKind => SarifNodeKind.SpecialLocations;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public SpecialLocations DeepClone()
        {
            return (SpecialLocations)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new SpecialLocations(this);
        }
        #endregion

        public static IEqualityComparer<SpecialLocations> ValueComparer => EqualityComparer<SpecialLocations>.Default;
        public bool ValueEquals(SpecialLocations other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
