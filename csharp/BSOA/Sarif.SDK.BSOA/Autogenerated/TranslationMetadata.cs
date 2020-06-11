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
    ///  GENERATED: BSOA Entity for 'TranslationMetadata'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class TranslationMetadata : PropertyBagHolder, ISarifNode, IRow
    {
        private TranslationMetadataTable _table;
        private int _index;

        public TranslationMetadata() : this(SarifLogDatabase.Current.TranslationMetadata)
        { }

        public TranslationMetadata(SarifLog root) : this(root.Database.TranslationMetadata)
        { }

        internal TranslationMetadata(TranslationMetadataTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal TranslationMetadata(TranslationMetadataTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public TranslationMetadata(
            string name,
            string fullName,
            MultiformatMessageString shortDescription,
            MultiformatMessageString fullDescription,
            Uri downloadUri,
            Uri informationUri,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.TranslationMetadata)
        {
            Name = name;
            FullName = fullName;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            DownloadUri = downloadUri;
            InformationUri = informationUri;
            Properties = properties;
        }

        public TranslationMetadata(TranslationMetadata other) 
            : this(SarifLogDatabase.Current.TranslationMetadata)
        {
            Name = other.Name;
            FullName = other.FullName;
            ShortDescription = other.ShortDescription;
            FullDescription = other.FullDescription;
            DownloadUri = other.DownloadUri;
            InformationUri = other.InformationUri;
            Properties = other.Properties;
        }

        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        [DataMember(Name = "fullName", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string FullName
        {
            get => _table.FullName[_index];
            set => _table.FullName[_index] = value;
        }

        [DataMember(Name = "shortDescription", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MultiformatMessageString ShortDescription
        {
            get => _table.Database.MultiformatMessageString.Get(_table.ShortDescription[_index]);
            set => _table.ShortDescription[_index] = _table.Database.MultiformatMessageString.LocalIndex(value);
        }

        [DataMember(Name = "fullDescription", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MultiformatMessageString FullDescription
        {
            get => _table.Database.MultiformatMessageString.Get(_table.FullDescription[_index]);
            set => _table.FullDescription[_index] = _table.Database.MultiformatMessageString.LocalIndex(value);
        }

        [DataMember(Name = "downloadUri", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri DownloadUri
        {
            get => _table.DownloadUri[_index];
            set => _table.DownloadUri[_index] = value;
        }

        [DataMember(Name = "informationUri", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri InformationUri
        {
            get => _table.InformationUri[_index];
            set => _table.InformationUri[_index] = value;
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<TranslationMetadata>
        public bool Equals(TranslationMetadata other)
        {
            if (other == null) { return false; }

            if (this.Name != other.Name) { return false; }
            if (this.FullName != other.FullName) { return false; }
            if (this.ShortDescription != other.ShortDescription) { return false; }
            if (this.FullDescription != other.FullDescription) { return false; }
            if (this.DownloadUri != other.DownloadUri) { return false; }
            if (this.InformationUri != other.InformationUri) { return false; }
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
                if (Name != default(string))
                {
                    result = (result * 31) + Name.GetHashCode();
                }

                if (FullName != default(string))
                {
                    result = (result * 31) + FullName.GetHashCode();
                }

                if (ShortDescription != default(MultiformatMessageString))
                {
                    result = (result * 31) + ShortDescription.GetHashCode();
                }

                if (FullDescription != default(MultiformatMessageString))
                {
                    result = (result * 31) + FullDescription.GetHashCode();
                }

                if (DownloadUri != default(Uri))
                {
                    result = (result * 31) + DownloadUri.GetHashCode();
                }

                if (InformationUri != default(Uri))
                {
                    result = (result * 31) + InformationUri.GetHashCode();
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
            return Equals(obj as TranslationMetadata);
        }

        public static bool operator ==(TranslationMetadata left, TranslationMetadata right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(TranslationMetadata left, TranslationMetadata right)
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
            _table = (TranslationMetadataTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.TranslationMetadata;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public TranslationMetadata DeepClone()
        {
            return (TranslationMetadata)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new TranslationMetadata(this);
        }
        #endregion

        public static IEqualityComparer<TranslationMetadata> ValueComparer => EqualityComparer<TranslationMetadata>.Default;
        public bool ValueEquals(TranslationMetadata other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
