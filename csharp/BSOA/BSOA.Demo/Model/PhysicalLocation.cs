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

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'PhysicalLocation'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class PhysicalLocation : PropertyBagHolder, ISarifNode, IRow
    {
        private PhysicalLocationTable _table;
        private int _index;

        public PhysicalLocation() : this(SarifLogDatabase.Current.PhysicalLocation)
        { }

        public PhysicalLocation(SarifLog root) : this(root.Database.PhysicalLocation)
        { }

        internal PhysicalLocation(PhysicalLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal PhysicalLocation(PhysicalLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public PhysicalLocation(
            ArtifactLocation artifactLocation,
            Region region,
            Region contextRegion
        ) 
            : this(SarifLogDatabase.Current.PhysicalLocation)
        {
            ArtifactLocation = artifactLocation;
            Region = region;
            ContextRegion = contextRegion;
        }

        public PhysicalLocation(PhysicalLocation other) 
            : this(SarifLogDatabase.Current.PhysicalLocation)
        {
            ArtifactLocation = other.ArtifactLocation;
            Region = other.Region;
            ContextRegion = other.ContextRegion;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ArtifactLocation ArtifactLocation
        {
            get => _table.Database.ArtifactLocation.Get(_table.ArtifactLocation[_index]);
            set => _table.ArtifactLocation[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Region Region
        {
            get => _table.Database.Region.Get(_table.Region[_index]);
            set => _table.Region[_index] = _table.Database.Region.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Region ContextRegion
        {
            get => _table.Database.Region.Get(_table.ContextRegion[_index]);
            set => _table.ContextRegion[_index] = _table.Database.Region.LocalIndex(value);
        }

        #region IEquatable<PhysicalLocation>
        public bool Equals(PhysicalLocation other)
        {
            if (other == null) { return false; }

            if (this.ArtifactLocation != other.ArtifactLocation) { return false; }
            if (this.Region != other.Region) { return false; }
            if (this.ContextRegion != other.ContextRegion) { return false; }

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

                if (Region != default(Region))
                {
                    result = (result * 31) + Region.GetHashCode();
                }

                if (ContextRegion != default(Region))
                {
                    result = (result * 31) + ContextRegion.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PhysicalLocation);
        }

        public static bool operator ==(PhysicalLocation left, PhysicalLocation right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(PhysicalLocation left, PhysicalLocation right)
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
            _table = (PhysicalLocationTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.PhysicalLocation;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public PhysicalLocation DeepClone()
        {
            return (PhysicalLocation)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new PhysicalLocation(this);
        }
        #endregion

        public static IEqualityComparer<PhysicalLocation> ValueComparer => EqualityComparer<PhysicalLocation>.Default;
        public bool ValueEquals(PhysicalLocation other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
