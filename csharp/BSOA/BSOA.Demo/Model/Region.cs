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
    ///  GENERATED: BSOA Entity for 'Region'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Region : PropertyBagHolder, ISarifNode, IRow
    {
        private RegionTable _table;
        private int _index;

        internal Region(RegionTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Region(RegionTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Region(SarifLogBsoa database) : this(database.Region)
        { }

        public Region() : this(SarifLogBsoa.Current)
        { }

        public Region(Region other) : this()
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }
            _table.CopyItem(_index, other._table, other._index);
        }

        [DataMember(Name = "startLine", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int StartLine
        {
            get => _table.StartLine[_index];
            set => _table.StartLine[_index] = value;
        }

        [DataMember(Name = "startColumn", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int StartColumn
        {
            get => _table.StartColumn[_index];
            set => _table.StartColumn[_index] = value;
        }

        [DataMember(Name = "endLine", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int EndLine
        {
            get => _table.EndLine[_index];
            set => _table.EndLine[_index] = value;
        }

        [DataMember(Name = "endColumn", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int EndColumn
        {
            get => _table.EndColumn[_index];
            set => _table.EndColumn[_index] = value;
        }

        [DataMember(Name = "byteOffset", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ByteOffset
        {
            get => _table.ByteOffset[_index];
            set => _table.ByteOffset[_index] = value;
        }

        [DataMember(Name = "byteLength", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ByteLength
        {
            get => _table.ByteLength[_index];
            set => _table.ByteLength[_index] = value;
        }

        [DataMember(Name = "charOffset", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int CharOffset
        {
            get => _table.CharOffset[_index];
            set => _table.CharOffset[_index] = value;
        }

        [DataMember(Name = "charLength", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int CharLength
        {
            get => _table.CharLength[_index];
            set => _table.CharLength[_index] = value;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ArtifactContent Snippet
        {
            get => _table.Database.ArtifactContent.Get(_table.Snippet[_index]);
            set => _table.Snippet[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        [DataMember(Name = "sourceLanguage", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SourceLanguage
        {
            get => _table.SourceLanguage[_index];
            set => _table.SourceLanguage[_index] = value;
        }

        #region IRow
        ITable IRow.Table => _table;
        int IRow.Index => _index;

        void IRow.Reset(ITable table, int index)
        {
            _table = (RegionTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Region;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Region DeepClone()
        {
            return (Region)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Region(this);
        }
        #endregion

        //public static IEqualityComparer<Region> ValueComparer => RegionEqualityComparer.Instance;
        //public bool ValueEquals(Region other) => ValueComparer.Equals(this, other);
        //public int ValueGetHashCode() => ValueComparer.GetHashCode(this);
    }
}
