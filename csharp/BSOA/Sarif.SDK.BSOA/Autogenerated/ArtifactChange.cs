// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'ArtifactChange'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ArtifactChange : PropertyBagHolder, ISarifNode, IRow
    {
        private ArtifactChangeTable _table;
        private int _index;

        public ArtifactChange() : this(SarifLogDatabase.Current.ArtifactChange)
        { }

        public ArtifactChange(SarifLog root) : this(root.Database.ArtifactChange)
        { }

        internal ArtifactChange(ArtifactChangeTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ArtifactChange(ArtifactChangeTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ArtifactChange(
            ArtifactLocation artifactLocation,
            IList<Replacement> replacements,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.ArtifactChange)
        {
            ArtifactLocation = artifactLocation;
            Replacements = replacements;
            Properties = properties;
        }

        public ArtifactChange(ArtifactChange other) 
            : this(SarifLogDatabase.Current.ArtifactChange)
        {
            ArtifactLocation = other.ArtifactLocation;
            Replacements = other.Replacements;
            Properties = other.Properties;
        }

        [DataMember(Name = "artifactLocation", IsRequired = false, EmitDefaultValue = false)]
        public ArtifactLocation ArtifactLocation
        {
            get => _table.Database.ArtifactLocation.Get(_table.ArtifactLocation[_index]);
            set => _table.ArtifactLocation[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        [DataMember(Name = "replacements", IsRequired = false, EmitDefaultValue = false)]
        public IList<Replacement> Replacements
        {
            get => _table.Database.Replacement.List(_table.Replacements[_index]);
            set => _table.Database.Replacement.List(_table.Replacements[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ArtifactChange>
        public bool Equals(ArtifactChange other)
        {
            if (other == null) { return false; }

            if (this.ArtifactLocation != other.ArtifactLocation) { return false; }
            if (this.Replacements != other.Replacements) { return false; }
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
                if (ArtifactLocation != default(ArtifactLocation))
                {
                    result = (result * 31) + ArtifactLocation.GetHashCode();
                }

                if (Replacements != default(IList<Replacement>))
                {
                    result = (result * 31) + Replacements.GetHashCode();
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
            return Equals(obj as ArtifactChange);
        }

        public static bool operator ==(ArtifactChange left, ArtifactChange right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ArtifactChange left, ArtifactChange right)
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
            _table = (ArtifactChangeTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ArtifactChange;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ArtifactChange DeepClone()
        {
            return (ArtifactChange)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ArtifactChange(this);
        }
        #endregion

        public static IEqualityComparer<ArtifactChange> ValueComparer => EqualityComparer<ArtifactChange>.Default;
        public bool ValueEquals(ArtifactChange other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
