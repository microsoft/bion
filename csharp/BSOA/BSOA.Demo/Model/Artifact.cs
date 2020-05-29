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
    ///  GENERATED: BSOA Entity for 'Artifact'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Artifact : PropertyBagHolder, ISarifNode, IRow
    {
        private ArtifactTable _table;
        private int _index;

        internal Artifact(ArtifactTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Artifact(ArtifactTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Artifact(SarifLogBsoa database) : this(database.Artifact)
        { }

        public Artifact() : this(SarifLogBsoa.Current)
        { }

        public Artifact(
            Message description,
            ArtifactLocation location,
            int parentIndex,
            int offset,
            int length,
            string mimeType,
            ArtifactContent contents,
            string encoding,
            string sourceLanguage,
            DateTime lastModifiedTimeUtc
        ) : this(SarifLogBsoa.Current)
        {
            Description = description;
            Location = location;
            ParentIndex = parentIndex;
            Offset = offset;
            Length = length;
            MimeType = mimeType;
            Contents = contents;
            Encoding = encoding;
            SourceLanguage = sourceLanguage;
            LastModifiedTimeUtc = lastModifiedTimeUtc;
        }

        public Artifact(Artifact other)
        {
            Description = other.Description;
            Location = other.Location;
            ParentIndex = other.ParentIndex;
            Offset = other.Offset;
            Length = other.Length;
            MimeType = other.MimeType;
            Contents = other.Contents;
            Encoding = other.Encoding;
            SourceLanguage = other.SourceLanguage;
            LastModifiedTimeUtc = other.LastModifiedTimeUtc;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public Message Description
        {
            get => _table.Database.Message.Get(_table.Description[_index]);
            set => _table.Description[_index] = _table.Database.Message.LocalIndex(value);
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ArtifactLocation Location
        {
            get => _table.Database.ArtifactLocation.Get(_table.Location[_index]);
            set => _table.Location[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        [DataMember(Name = "parentIndex", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        [DataMember(Name = "offset", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Offset
        {
            get => _table.Offset[_index];
            set => _table.Offset[_index] = value;
        }

        [DataMember(Name = "length", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public int Length
        {
            get => _table.Length[_index];
            set => _table.Length[_index] = value;
        }

        [DataMember(Name = "mimeType", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string MimeType
        {
            get => _table.MimeType[_index];
            set => _table.MimeType[_index] = value;
        }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public ArtifactContent Contents
        {
            get => _table.Database.ArtifactContent.Get(_table.Contents[_index]);
            set => _table.Contents[_index] = _table.Database.ArtifactContent.LocalIndex(value);
        }

        [DataMember(Name = "encoding", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string Encoding
        {
            get => _table.Encoding[_index];
            set => _table.Encoding[_index] = value;
        }

        [DataMember(Name = "sourceLanguage", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(null)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public string SourceLanguage
        {
            get => _table.SourceLanguage[_index];
            set => _table.SourceLanguage[_index] = value;
        }

        [DataMember(Name = "lastModifiedTimeUtc", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.DateTimeConverter))]
        public DateTime LastModifiedTimeUtc
        {
            get => _table.LastModifiedTimeUtc[_index];
            set => _table.LastModifiedTimeUtc[_index] = value;
        }

        #region IEquatable<Artifact>
        public bool Equals(Artifact other)
        {
            if (other == null) { return false; }

            if (this.Description != other.Description) { return false; }
            if (this.Location != other.Location) { return false; }
            if (this.ParentIndex != other.ParentIndex) { return false; }
            if (this.Offset != other.Offset) { return false; }
            if (this.Length != other.Length) { return false; }
            if (this.MimeType != other.MimeType) { return false; }
            if (this.Contents != other.Contents) { return false; }
            if (this.Encoding != other.Encoding) { return false; }
            if (this.SourceLanguage != other.SourceLanguage) { return false; }
            if (this.LastModifiedTimeUtc != other.LastModifiedTimeUtc) { return false; }

            return true;
        }
        #endregion

        #region Object overrides
        public override int GetHashCode()
        {
            int result = 17;

            unchecked
            {
                if (Description != default(Message))
                {
                    result = (result * 31) + Description.GetHashCode();
                }

                if (Location != default(ArtifactLocation))
                {
                    result = (result * 31) + Location.GetHashCode();
                }

                if (ParentIndex != default(int))
                {
                    result = (result * 31) + ParentIndex.GetHashCode();
                }

                if (Offset != default(int))
                {
                    result = (result * 31) + Offset.GetHashCode();
                }

                if (Length != default(int))
                {
                    result = (result * 31) + Length.GetHashCode();
                }

                if (MimeType != default(string))
                {
                    result = (result * 31) + MimeType.GetHashCode();
                }

                if (Contents != default(ArtifactContent))
                {
                    result = (result * 31) + Contents.GetHashCode();
                }

                if (Encoding != default(string))
                {
                    result = (result * 31) + Encoding.GetHashCode();
                }

                if (SourceLanguage != default(string))
                {
                    result = (result * 31) + SourceLanguage.GetHashCode();
                }

                if (LastModifiedTimeUtc != default(DateTime))
                {
                    result = (result * 31) + LastModifiedTimeUtc.GetHashCode();
                }
            }

            return result;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Artifact);
        }

        public static bool operator ==(Artifact left, Artifact right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Artifact left, Artifact right)
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
            _table = (ArtifactTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Artifact;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Artifact DeepClone()
        {
            return (Artifact)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Artifact(this);
        }
        #endregion

        public static IEqualityComparer<Artifact> ValueComparer => EqualityComparer<Artifact>.Default;
        public bool ValueEquals(Artifact other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
