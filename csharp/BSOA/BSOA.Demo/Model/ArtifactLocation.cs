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
    ///  GENERATED: BSOA Entity for 'ArtifactLocation'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ArtifactLocation : PropertyBagHolder, ISarifNode, IRow
    {
        private ArtifactLocationTable _table;
        private int _index;

        internal ArtifactLocation(ArtifactLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ArtifactLocation(ArtifactLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public ArtifactLocation(SarifLogBsoa database) : this(database.ArtifactLocation)
        { }

        public ArtifactLocation() : this(SarifLogBsoa.Current)
        { }

        public ArtifactLocation(ArtifactLocation other) : this()
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }
            _table.CopyItem(_index, other._table, other._index);
        }

        [DataMember(Name = "uri", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Uri Uri
        {
            get => _table.Uri[_index];
            set => _table.Uri[_index] = value;
        }

        [DataMember(Name = "uriBaseId", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string UriBaseId
        {
            get => _table.UriBaseId[_index];
            set => _table.UriBaseId[_index] = value;
        }

        [DataMember(Name = "index", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Message Description
        {
            get => _table.Database.Message.Get(_table.Description[_index]);
            set => _table.Description[_index] = _table.Database.Message.LocalIndex(value);
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (ArtifactLocationTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ArtifactLocation;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ArtifactLocation DeepClone()
        {
            return (ArtifactLocation)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ArtifactLocation(this);
        }
        #endregion

        //public static IEqualityComparer<ArtifactLocation> ValueComparer => ArtifactLocationEqualityComparer.Instance;
        //public bool ValueEquals(ArtifactLocation other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
