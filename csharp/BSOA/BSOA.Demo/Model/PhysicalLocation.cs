// Copyright (c) Microsoft.  All Rights Reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

using Newtonsoft.Json;

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

        internal PhysicalLocation(PhysicalLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public PhysicalLocation(PhysicalLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public PhysicalLocation(SarifLogBsoa database) : this(database.PhysicalLocation)
        { }

        public PhysicalLocation() : this(SarifLogBsoa.Current)
        { }

        public PhysicalLocation(PhysicalLocation other) : this()
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }
            _table.CopyItem(_index, other._table, other._index);
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

        //public static IEqualityComparer<PhysicalLocation> ValueComparer => PhysicalLocationEqualityComparer.Instance;
        //public bool ValueEquals(PhysicalLocation other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
