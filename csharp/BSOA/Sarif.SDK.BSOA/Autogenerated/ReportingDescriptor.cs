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
    ///  GENERATED: BSOA Entity for 'ReportingDescriptor'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ReportingDescriptor : PropertyBagHolder, ISarifNode, IRow
    {
        private ReportingDescriptorTable _table;
        private int _index;

        public ReportingDescriptor() : this(SarifLogDatabase.Current.ReportingDescriptor)
        { }

        public ReportingDescriptor(SarifLog root) : this(root.Database.ReportingDescriptor)
        { }

        internal ReportingDescriptor(ReportingDescriptorTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ReportingDescriptor(ReportingDescriptorTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ReportingDescriptor(
            string id,
            IList<string> deprecatedIds,
            string guid,
            IList<string> deprecatedGuids,
            string name,
            IList<string> deprecatedNames,
            MultiformatMessageString shortDescription,
            MultiformatMessageString fullDescription,
            IDictionary<string, MultiformatMessageString> messageStrings,
            ReportingConfiguration defaultConfiguration,
            Uri helpUri,
            MultiformatMessageString help,
            IList<ReportingDescriptorRelationship> relationships,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.ReportingDescriptor)
        {
            Id = id;
            DeprecatedIds = deprecatedIds;
            Guid = guid;
            DeprecatedGuids = deprecatedGuids;
            Name = name;
            DeprecatedNames = deprecatedNames;
            ShortDescription = shortDescription;
            FullDescription = fullDescription;
            MessageStrings = messageStrings;
            DefaultConfiguration = defaultConfiguration;
            HelpUri = helpUri;
            Help = help;
            Relationships = relationships;
            Properties = properties;
        }

        public ReportingDescriptor(ReportingDescriptor other) 
            : this(SarifLogDatabase.Current.ReportingDescriptor)
        {
            Id = other.Id;
            DeprecatedIds = other.DeprecatedIds;
            Guid = other.Guid;
            DeprecatedGuids = other.DeprecatedGuids;
            Name = other.Name;
            DeprecatedNames = other.DeprecatedNames;
            ShortDescription = other.ShortDescription;
            FullDescription = other.FullDescription;
            MessageStrings = other.MessageStrings;
            DefaultConfiguration = other.DefaultConfiguration;
            HelpUri = other.HelpUri;
            Help = other.Help;
            Relationships = other.Relationships;
            Properties = other.Properties;
        }

        [DataMember(Name = "id", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Id
        {
            get => _table.Id[_index];
            set => _table.Id[_index] = value;
        }

        [DataMember(Name = "deprecatedIds", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<string> DeprecatedIds
        {
            get => _table.DeprecatedIds[_index];
            set => _table.DeprecatedIds[_index] = value;
        }

        [DataMember(Name = "guid", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        [DataMember(Name = "deprecatedGuids", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<string> DeprecatedGuids
        {
            get => _table.DeprecatedGuids[_index];
            set => _table.DeprecatedGuids[_index] = value;
        }

        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Name
        {
            get => _table.Name[_index];
            set => _table.Name[_index] = value;
        }

        [DataMember(Name = "deprecatedNames", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<string> DeprecatedNames
        {
            get => _table.DeprecatedNames[_index];
            set => _table.DeprecatedNames[_index] = value;
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

        [DataMember(Name = "messageStrings", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IDictionary<string, MultiformatMessageString> MessageStrings
        {
            get => _table.MessageStrings[_index];
            set => _table.MessageStrings[_index] = value;
        }

        [DataMember(Name = "defaultConfiguration", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ReportingConfiguration DefaultConfiguration
        {
            get => _table.Database.ReportingConfiguration.Get(_table.DefaultConfiguration[_index]);
            set => _table.DefaultConfiguration[_index] = _table.Database.ReportingConfiguration.LocalIndex(value);
        }

        [DataMember(Name = "helpUri", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Uri HelpUri
        {
            get => _table.HelpUri[_index];
            set => _table.HelpUri[_index] = value;
        }

        [DataMember(Name = "help", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MultiformatMessageString Help
        {
            get => _table.Database.MultiformatMessageString.Get(_table.Help[_index]);
            set => _table.Help[_index] = _table.Database.MultiformatMessageString.LocalIndex(value);
        }

        [DataMember(Name = "relationships", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IList<ReportingDescriptorRelationship> Relationships
        {
            get => _table.Database.ReportingDescriptorRelationship.List(_table.Relationships[_index]);
            set => _table.Database.ReportingDescriptorRelationship.List(_table.Relationships[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ReportingDescriptor>
        public bool Equals(ReportingDescriptor other)
        {
            if (other == null) { return false; }

            if (this.Id != other.Id) { return false; }
            if (this.DeprecatedIds != other.DeprecatedIds) { return false; }
            if (this.Guid != other.Guid) { return false; }
            if (this.DeprecatedGuids != other.DeprecatedGuids) { return false; }
            if (this.Name != other.Name) { return false; }
            if (this.DeprecatedNames != other.DeprecatedNames) { return false; }
            if (this.ShortDescription != other.ShortDescription) { return false; }
            if (this.FullDescription != other.FullDescription) { return false; }
            if (this.MessageStrings != other.MessageStrings) { return false; }
            if (this.DefaultConfiguration != other.DefaultConfiguration) { return false; }
            if (this.HelpUri != other.HelpUri) { return false; }
            if (this.Help != other.Help) { return false; }
            if (this.Relationships != other.Relationships) { return false; }
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
                if (Id != default(string))
                {
                    result = (result * 31) + Id.GetHashCode();
                }

                if (DeprecatedIds != default(IList<string>))
                {
                    result = (result * 31) + DeprecatedIds.GetHashCode();
                }

                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (DeprecatedGuids != default(IList<string>))
                {
                    result = (result * 31) + DeprecatedGuids.GetHashCode();
                }

                if (Name != default(string))
                {
                    result = (result * 31) + Name.GetHashCode();
                }

                if (DeprecatedNames != default(IList<string>))
                {
                    result = (result * 31) + DeprecatedNames.GetHashCode();
                }

                if (ShortDescription != default(MultiformatMessageString))
                {
                    result = (result * 31) + ShortDescription.GetHashCode();
                }

                if (FullDescription != default(MultiformatMessageString))
                {
                    result = (result * 31) + FullDescription.GetHashCode();
                }

                if (MessageStrings != default(IDictionary<string, MultiformatMessageString>))
                {
                    result = (result * 31) + MessageStrings.GetHashCode();
                }

                if (DefaultConfiguration != default(ReportingConfiguration))
                {
                    result = (result * 31) + DefaultConfiguration.GetHashCode();
                }

                if (HelpUri != default(Uri))
                {
                    result = (result * 31) + HelpUri.GetHashCode();
                }

                if (Help != default(MultiformatMessageString))
                {
                    result = (result * 31) + Help.GetHashCode();
                }

                if (Relationships != default(IList<ReportingDescriptorRelationship>))
                {
                    result = (result * 31) + Relationships.GetHashCode();
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
            return Equals(obj as ReportingDescriptor);
        }

        public static bool operator ==(ReportingDescriptor left, ReportingDescriptor right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ReportingDescriptor left, ReportingDescriptor right)
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
            _table = (ReportingDescriptorTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ReportingDescriptor;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ReportingDescriptor DeepClone()
        {
            return (ReportingDescriptor)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ReportingDescriptor(this);
        }
        #endregion

        public static IEqualityComparer<ReportingDescriptor> ValueComparer => EqualityComparer<ReportingDescriptor>.Default;
        public bool ValueEquals(ReportingDescriptor other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
