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
    ///  GENERATED: BSOA Entity for 'SpecialLocations'
    /// </summary>
    [DataContract]
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
        }

        internal SpecialLocations(SpecialLocationsTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public SpecialLocations(
            ArtifactLocation displayBase,
            IDictionary<string, string> properties
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

        [DataMember(Name = "displayBase", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ArtifactLocation DisplayBase
        {
            get => _table.Database.ArtifactLocation.Get(_table.DisplayBase[_index]);
            set => _table.DisplayBase[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        internal override IDictionary<string, string> Properties
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

                if (Properties != default(IDictionary<string, string>))
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

        void IRow.Reset(ITable table, int index)
        {
            _table = (SpecialLocationsTable)table;
            _index = index;
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
