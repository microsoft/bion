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
    ///  GENERATED: BSOA Entity for 'ToolComponent'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ToolComponent : PropertyBagHolder, ISarifNode, IRow
    {
        private ToolComponentTable _table;
        private int _index;

        public ToolComponent() : this(SarifLogDatabase.Current.ToolComponent)
        { }

        public ToolComponent(SarifLog root) : this(root.Database.ToolComponent)
        { }

        internal ToolComponent(ToolComponentTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ToolComponent(ToolComponentTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ToolComponent(
            string guid,
            string name,
            string organization,
            string product,
            string productSuite,
            MultiformatMessageString shortDescription,
            MultiformatMessageString fullDescription,
            string fullName,
            string version,
            string semanticVersion,
            string dottedQuadFileVersion,
            string releaseDateUtc,
            Uri downloadUri,
            Uri informationUri,
            IDictionary<string, MultiformatMessageString> globalMessageStrings,
            IList<ReportingDescriptor> notifications,
            IList<ReportingDescriptor> rules,
            IList<ReportingDescriptor> taxa,
            IList<ArtifactLocation> locations,
            string language,
            ToolComponentContents contents,
            bool isComprehensive,
            string localizedDataSemanticVersion,
            string minimumRequiredLocalizedDataSemanticVersion,
            ToolComponentReference associatedComponent,
            TranslationMetadata translationMetadata,
            IList<ToolComponentReference> supportedTaxonomies,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.ToolComponent)
        {
            Guid = guid;
            Name = name;
            Organization = organization;
            Product = product;
            ProductSuite = productSuite;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            FullName = fullName;
            Version = version;
            SemanticVersion = semanticVersion;
            DottedQuadFileVersion = dottedQuadFileVersion;
            ReleaseDateUtc = releaseDateUtc;
            DownloadUri = downloadUri;
            InformationUri = informationUri;
            GlobalMessageStrings = globalMessageStrings;
            Notifications = notifications;
            Rules = rules;
            Taxa = taxa;
            Locations = locations;
            Language = language;
            Contents = contents;
            IsComprehensive = isComprehensive;
            LocalizedDataSemanticVersion = localizedDataSemanticVersion;
            MinimumRequiredLocalizedDataSemanticVersion = minimumRequiredLocalizedDataSemanticVersion;
            AssociatedComponent = associatedComponent;
            TranslationMetadata = translationMetadata;
            SupportedTaxonomies = supportedTaxonomies;
            Properties = properties;
        }

        public ToolComponent(ToolComponent other) 
            : this(SarifLogDatabase.Current.ToolComponent)
        {
            Guid = other.Guid;
            Name = other.Name;
            Organization = other.Organization;
            Product = other.Product;
            ProductSuite = other.ProductSuite;
            ShortDescription = other.ShortDescription;
            FullDescription = other.FullDescription;
            FullName = other.FullName;
            Version = other.Version;
            SemanticVersion = other.SemanticVersion;
            DottedQuadFileVersion = other.DottedQuadFileVersion;
            ReleaseDateUtc = other.ReleaseDateUtc;
            DownloadUri = other.DownloadUri;
            InformationUri = other.InformationUri;
            GlobalMessageStrings = other.GlobalMessageStrings;
            Notifications = other.Notifications;
            Rules = other.Rules;
            Taxa = other.Taxa;
            Locations = other.Locations;
            Language = other.Language;
            Contents = other.Contents;
            IsComprehensive = other.IsComprehensive;
            LocalizedDataSemanticVersion = other.LocalizedDataSemanticVersion;
            MinimumRequiredLocalizedDataSemanticVersion = other.MinimumRequiredLocalizedDataSemanticVersion;
            AssociatedComponent = other.AssociatedComponent;
            TranslationMetadata = other.TranslationMetadata;
            SupportedTaxonomies = other.SupportedTaxonomies;
            Properties = other.Properties;
        }

        [DataMember(Name = "guid", IsRequired = false, EmitDefaultValue = false)]
        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        [DataMember(Name = "organization", IsRequired = false, EmitDefaultValue = false)]
        public string Organization
        {
            get => _table.Organization[_index];
            set => _table.Organization[_index] = value;
        }

        [DataMember(Name = "product", IsRequired = false, EmitDefaultValue = false)]
        public string Product
        {
            get => _table.Product[_index];
            set => _table.Product[_index] = value;
        }

        [DataMember(Name = "productSuite", IsRequired = false, EmitDefaultValue = false)]
        public string ProductSuite
        {
            get => _table.ProductSuite[_index];
            set => _table.ProductSuite[_index] = value;
        }

        [DataMember(Name = "shortDescription", IsRequired = false, EmitDefaultValue = false)]
        public MultiformatMessageString ShortDescription
        {
            get => _table.Database.MultiformatMessageString.Get(_table.ShortDescription[_index]);
            set => _table.ShortDescription[_index] = _table.Database.MultiformatMessageString.LocalIndex(value);
        }

        [DataMember(Name = "fullDescription", IsRequired = false, EmitDefaultValue = false)]
        public MultiformatMessageString FullDescription
        {
            get => _table.Database.MultiformatMessageString.Get(_table.FullDescription[_index]);
            set => _table.FullDescription[_index] = _table.Database.MultiformatMessageString.LocalIndex(value);
        }

        [DataMember(Name = "fullName", IsRequired = false, EmitDefaultValue = false)]
        public string FullName
        {
            get => _table.FullName[_index];
            set => _table.FullName[_index] = value;
        }

        [DataMember(Name = "version", IsRequired = false, EmitDefaultValue = false)]
        public string Version
        {
            get => _table.Version[_index];
            set => _table.Version[_index] = value;
        }

        [DataMember(Name = "semanticVersion", IsRequired = false, EmitDefaultValue = false)]
        public string SemanticVersion
        {
            get => _table.SemanticVersion[_index];
            set => _table.SemanticVersion[_index] = value;
        }

        [DataMember(Name = "dottedQuadFileVersion", IsRequired = false, EmitDefaultValue = false)]
        public string DottedQuadFileVersion
        {
            get => _table.DottedQuadFileVersion[_index];
            set => _table.DottedQuadFileVersion[_index] = value;
        }

        [DataMember(Name = "releaseDateUtc", IsRequired = false, EmitDefaultValue = false)]
        public string ReleaseDateUtc
        {
            get => _table.ReleaseDateUtc[_index];
            set => _table.ReleaseDateUtc[_index] = value;
        }

        [DataMember(Name = "downloadUri", IsRequired = false, EmitDefaultValue = false)]
        public Uri DownloadUri
        {
            get => _table.DownloadUri[_index];
            set => _table.DownloadUri[_index] = value;
        }

        [DataMember(Name = "informationUri", IsRequired = false, EmitDefaultValue = false)]
        public Uri InformationUri
        {
            get => _table.InformationUri[_index];
            set => _table.InformationUri[_index] = value;
        }

        [DataMember(Name = "globalMessageStrings", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, MultiformatMessageString> GlobalMessageStrings
        {
            get => _table.GlobalMessageStrings[_index];
            set => _table.GlobalMessageStrings[_index] = value;
        }

        [DataMember(Name = "notifications", IsRequired = false, EmitDefaultValue = false)]
        public IList<ReportingDescriptor> Notifications
        {
            get => _table.Database.ReportingDescriptor.List(_table.Notifications[_index]);
            set => _table.Database.ReportingDescriptor.List(_table.Notifications[_index]).SetTo(value);
        }

        [DataMember(Name = "rules", IsRequired = false, EmitDefaultValue = false)]
        public IList<ReportingDescriptor> Rules
        {
            get => _table.Database.ReportingDescriptor.List(_table.Rules[_index]);
            set => _table.Database.ReportingDescriptor.List(_table.Rules[_index]).SetTo(value);
        }

        [DataMember(Name = "taxa", IsRequired = false, EmitDefaultValue = false)]
        public IList<ReportingDescriptor> Taxa
        {
            get => _table.Database.ReportingDescriptor.List(_table.Taxa[_index]);
            set => _table.Database.ReportingDescriptor.List(_table.Taxa[_index]).SetTo(value);
        }

        [DataMember(Name = "locations", IsRequired = false, EmitDefaultValue = false)]
        public IList<ArtifactLocation> Locations
        {
            get => _table.Database.ArtifactLocation.List(_table.Locations[_index]);
            set => _table.Database.ArtifactLocation.List(_table.Locations[_index]).SetTo(value);
        }

        [DataMember(Name = "language", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue("en-US")]
        public string Language
        {
            get => _table.Language[_index];
            set => _table.Language[_index] = value;
        }

        [DataMember(Name = "contents", IsRequired = false, EmitDefaultValue = false)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.EnumConverter))]
        public ToolComponentContents Contents
        {
            get => (ToolComponentContents)_table.Contents[_index];
            set => _table.Contents[_index] = (int)value;
        }

        [DataMember(Name = "isComprehensive", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(false)]
        public bool IsComprehensive
        {
            get => _table.IsComprehensive[_index];
            set => _table.IsComprehensive[_index] = value;
        }

        [DataMember(Name = "localizedDataSemanticVersion", IsRequired = false, EmitDefaultValue = false)]
        public string LocalizedDataSemanticVersion
        {
            get => _table.LocalizedDataSemanticVersion[_index];
            set => _table.LocalizedDataSemanticVersion[_index] = value;
        }

        [DataMember(Name = "minimumRequiredLocalizedDataSemanticVersion", IsRequired = false, EmitDefaultValue = false)]
        public string MinimumRequiredLocalizedDataSemanticVersion
        {
            get => _table.MinimumRequiredLocalizedDataSemanticVersion[_index];
            set => _table.MinimumRequiredLocalizedDataSemanticVersion[_index] = value;
        }

        [DataMember(Name = "associatedComponent", IsRequired = false, EmitDefaultValue = false)]
        public ToolComponentReference AssociatedComponent
        {
            get => _table.Database.ToolComponentReference.Get(_table.AssociatedComponent[_index]);
            set => _table.AssociatedComponent[_index] = _table.Database.ToolComponentReference.LocalIndex(value);
        }

        [DataMember(Name = "translationMetadata", IsRequired = false, EmitDefaultValue = false)]
        public TranslationMetadata TranslationMetadata
        {
            get => _table.Database.TranslationMetadata.Get(_table.TranslationMetadata[_index]);
            set => _table.TranslationMetadata[_index] = _table.Database.TranslationMetadata.LocalIndex(value);
        }

        [DataMember(Name = "supportedTaxonomies", IsRequired = false, EmitDefaultValue = false)]
        public IList<ToolComponentReference> SupportedTaxonomies
        {
            get => _table.Database.ToolComponentReference.List(_table.SupportedTaxonomies[_index]);
            set => _table.Database.ToolComponentReference.List(_table.SupportedTaxonomies[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ToolComponent>
        public bool Equals(ToolComponent other)
        {
            if (other == null) { return false; }

            if (this.Guid != other.Guid) { return false; }
            if (this.Name != other.Name) { return false; }
            if (this.Organization != other.Organization) { return false; }
            if (this.Product != other.Product) { return false; }
            if (this.ProductSuite != other.ProductSuite) { return false; }
            if (this.ShortDescription != other.ShortDescription) { return false; }
            if (this.FullDescription != other.FullDescription) { return false; }
            if (this.FullName != other.FullName) { return false; }
            if (this.Version != other.Version) { return false; }
            if (this.SemanticVersion != other.SemanticVersion) { return false; }
            if (this.DottedQuadFileVersion != other.DottedQuadFileVersion) { return false; }
            if (this.ReleaseDateUtc != other.ReleaseDateUtc) { return false; }
            if (this.DownloadUri != other.DownloadUri) { return false; }
            if (this.InformationUri != other.InformationUri) { return false; }
            if (this.GlobalMessageStrings != other.GlobalMessageStrings) { return false; }
            if (this.Notifications != other.Notifications) { return false; }
            if (this.Rules != other.Rules) { return false; }
            if (this.Taxa != other.Taxa) { return false; }
            if (this.Locations != other.Locations) { return false; }
            if (this.Language != other.Language) { return false; }
            if (this.Contents != other.Contents) { return false; }
            if (this.IsComprehensive != other.IsComprehensive) { return false; }
            if (this.LocalizedDataSemanticVersion != other.LocalizedDataSemanticVersion) { return false; }
            if (this.MinimumRequiredLocalizedDataSemanticVersion != other.MinimumRequiredLocalizedDataSemanticVersion) { return false; }
            if (this.AssociatedComponent != other.AssociatedComponent) { return false; }
            if (this.TranslationMetadata != other.TranslationMetadata) { return false; }
            if (this.SupportedTaxonomies != other.SupportedTaxonomies) { return false; }
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
                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (Name != default(string))
                {
                    result = (result * 31) + Name.GetHashCode();
                }

                if (Organization != default(string))
                {
                    result = (result * 31) + Organization.GetHashCode();
                }

                if (Product != default(string))
                {
                    result = (result * 31) + Product.GetHashCode();
                }

                if (ProductSuite != default(string))
                {
                    result = (result * 31) + ProductSuite.GetHashCode();
                }

                if (ShortDescription != default(MultiformatMessageString))
                {
                    result = (result * 31) + ShortDescription.GetHashCode();
                }

                if (FullDescription != default(MultiformatMessageString))
                {
                    result = (result * 31) + FullDescription.GetHashCode();
                }

                if (FullName != default(string))
                {
                    result = (result * 31) + FullName.GetHashCode();
                }

                if (Version != default(string))
                {
                    result = (result * 31) + Version.GetHashCode();
                }

                if (SemanticVersion != default(string))
                {
                    result = (result * 31) + SemanticVersion.GetHashCode();
                }

                if (DottedQuadFileVersion != default(string))
                {
                    result = (result * 31) + DottedQuadFileVersion.GetHashCode();
                }

                if (ReleaseDateUtc != default(string))
                {
                    result = (result * 31) + ReleaseDateUtc.GetHashCode();
                }

                if (DownloadUri != default(Uri))
                {
                    result = (result * 31) + DownloadUri.GetHashCode();
                }

                if (InformationUri != default(Uri))
                {
                    result = (result * 31) + InformationUri.GetHashCode();
                }

                if (GlobalMessageStrings != default(IDictionary<string, MultiformatMessageString>))
                {
                    result = (result * 31) + GlobalMessageStrings.GetHashCode();
                }

                if (Notifications != default(IList<ReportingDescriptor>))
                {
                    result = (result * 31) + Notifications.GetHashCode();
                }

                if (Rules != default(IList<ReportingDescriptor>))
                {
                    result = (result * 31) + Rules.GetHashCode();
                }

                if (Taxa != default(IList<ReportingDescriptor>))
                {
                    result = (result * 31) + Taxa.GetHashCode();
                }

                if (Locations != default(IList<ArtifactLocation>))
                {
                    result = (result * 31) + Locations.GetHashCode();
                }

                if (Language != default(string))
                {
                    result = (result * 31) + Language.GetHashCode();
                }

                if (Contents != default(ToolComponentContents))
                {
                    result = (result * 31) + Contents.GetHashCode();
                }

                if (IsComprehensive != default(bool))
                {
                    result = (result * 31) + IsComprehensive.GetHashCode();
                }

                if (LocalizedDataSemanticVersion != default(string))
                {
                    result = (result * 31) + LocalizedDataSemanticVersion.GetHashCode();
                }

                if (MinimumRequiredLocalizedDataSemanticVersion != default(string))
                {
                    result = (result * 31) + MinimumRequiredLocalizedDataSemanticVersion.GetHashCode();
                }

                if (AssociatedComponent != default(ToolComponentReference))
                {
                    result = (result * 31) + AssociatedComponent.GetHashCode();
                }

                if (TranslationMetadata != default(TranslationMetadata))
                {
                    result = (result * 31) + TranslationMetadata.GetHashCode();
                }

                if (SupportedTaxonomies != default(IList<ToolComponentReference>))
                {
                    result = (result * 31) + SupportedTaxonomies.GetHashCode();
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
            return Equals(obj as ToolComponent);
        }

        public static bool operator ==(ToolComponent left, ToolComponent right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ToolComponent left, ToolComponent right)
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
            _table = (ToolComponentTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ToolComponent;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ToolComponent DeepClone()
        {
            return (ToolComponent)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ToolComponent(this);
        }
        #endregion

        public static IEqualityComparer<ToolComponent> ValueComparer => EqualityComparer<ToolComponent>.Default;
        public bool ValueEquals(ToolComponent other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
