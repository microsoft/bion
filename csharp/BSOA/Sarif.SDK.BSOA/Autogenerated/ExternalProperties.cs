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
    ///  GENERATED: BSOA Entity for 'ExternalProperties'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class ExternalProperties : PropertyBagHolder, ISarifNode, IRow
    {
        private ExternalPropertiesTable _table;
        private int _index;

        public ExternalProperties() : this(SarifLogDatabase.Current.ExternalProperties)
        { }

        public ExternalProperties(SarifLog root) : this(root.Database.ExternalProperties)
        { }

        internal ExternalProperties(ExternalPropertiesTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal ExternalProperties(ExternalPropertiesTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ExternalProperties(
            Uri schema,
            SarifVersion version,
            string guid,
            string runGuid,
            Conversion conversion,
            IList<Graph> graphs,
            PropertyBag externalizedProperties,
            IList<Artifact> artifacts,
            IList<Invocation> invocations,
            IList<LogicalLocation> logicalLocations,
            IList<ThreadFlowLocation> threadFlowLocations,
            IList<Result> results,
            IList<ToolComponent> taxonomies,
            ToolComponent driver,
            IList<ToolComponent> extensions,
            IList<ToolComponent> policies,
            IList<ToolComponent> translations,
            IList<Address> addresses,
            IList<WebRequest> webRequests,
            IList<WebResponse> webResponses,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.ExternalProperties)
        {
            Schema = schema;
            Version = version;
            Guid = guid;
            RunGuid = runGuid;
            Conversion = conversion;
            Graphs = graphs;
            ExternalizedProperties = externalizedProperties;
            Artifacts = artifacts;
            Invocations = invocations;
            LogicalLocations = logicalLocations;
            ThreadFlowLocations = threadFlowLocations;
            Results = results;
            Taxonomies = taxonomies;
            Driver = driver;
            Extensions = extensions;
            Policies = policies;
            Translations = translations;
            Addresses = addresses;
            WebRequests = webRequests;
            WebResponses = webResponses;
            Properties = properties;
        }

        public ExternalProperties(ExternalProperties other) 
            : this(SarifLogDatabase.Current.ExternalProperties)
        {
            Schema = other.Schema;
            Version = other.Version;
            Guid = other.Guid;
            RunGuid = other.RunGuid;
            Conversion = other.Conversion;
            Graphs = other.Graphs;
            ExternalizedProperties = other.ExternalizedProperties;
            Artifacts = other.Artifacts;
            Invocations = other.Invocations;
            LogicalLocations = other.LogicalLocations;
            ThreadFlowLocations = other.ThreadFlowLocations;
            Results = other.Results;
            Taxonomies = other.Taxonomies;
            Driver = other.Driver;
            Extensions = other.Extensions;
            Policies = other.Policies;
            Translations = other.Translations;
            Addresses = other.Addresses;
            WebRequests = other.WebRequests;
            WebResponses = other.WebResponses;
            Properties = other.Properties;
        }

        [DataMember(Name = "schema", IsRequired = false, EmitDefaultValue = false)]
        public Uri Schema
        {
            get => _table.Schema[_index];
            set => _table.Schema[_index] = value;
        }

        [DataMember(Name = "version", IsRequired = false, EmitDefaultValue = false)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.SarifVersionConverter))]
        public SarifVersion Version
        {
            get => (SarifVersion)_table.Version[_index];
            set => _table.Version[_index] = (int)value;
        }

        [DataMember(Name = "guid", IsRequired = false, EmitDefaultValue = false)]
        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        [DataMember(Name = "runGuid", IsRequired = false, EmitDefaultValue = false)]
        public string RunGuid
        {
            get => _table.RunGuid[_index];
            set => _table.RunGuid[_index] = value;
        }

        [DataMember(Name = "conversion", IsRequired = false, EmitDefaultValue = false)]
        public Conversion Conversion
        {
            get => _table.Database.Conversion.Get(_table.Conversion[_index]);
            set => _table.Conversion[_index] = _table.Database.Conversion.LocalIndex(value);
        }

        [DataMember(Name = "graphs", IsRequired = false, EmitDefaultValue = false)]
        public IList<Graph> Graphs
        {
            get => _table.Database.Graph.List(_table.Graphs[_index]);
            set => _table.Database.Graph.List(_table.Graphs[_index]).SetTo(value);
        }

        [DataMember(Name = "externalizedProperties", IsRequired = false, EmitDefaultValue = false)]
        public PropertyBag ExternalizedProperties
        {
            get => _table.Database.PropertyBag.Get(_table.ExternalizedProperties[_index]);
            set => _table.ExternalizedProperties[_index] = _table.Database.PropertyBag.LocalIndex(value);
        }

        [DataMember(Name = "artifacts", IsRequired = false, EmitDefaultValue = false)]
        public IList<Artifact> Artifacts
        {
            get => _table.Database.Artifact.List(_table.Artifacts[_index]);
            set => _table.Database.Artifact.List(_table.Artifacts[_index]).SetTo(value);
        }

        [DataMember(Name = "invocations", IsRequired = false, EmitDefaultValue = false)]
        public IList<Invocation> Invocations
        {
            get => _table.Database.Invocation.List(_table.Invocations[_index]);
            set => _table.Database.Invocation.List(_table.Invocations[_index]).SetTo(value);
        }

        [DataMember(Name = "logicalLocations", IsRequired = false, EmitDefaultValue = false)]
        public IList<LogicalLocation> LogicalLocations
        {
            get => _table.Database.LogicalLocation.List(_table.LogicalLocations[_index]);
            set => _table.Database.LogicalLocation.List(_table.LogicalLocations[_index]).SetTo(value);
        }

        [DataMember(Name = "threadFlowLocations", IsRequired = false, EmitDefaultValue = false)]
        public IList<ThreadFlowLocation> ThreadFlowLocations
        {
            get => _table.Database.ThreadFlowLocation.List(_table.ThreadFlowLocations[_index]);
            set => _table.Database.ThreadFlowLocation.List(_table.ThreadFlowLocations[_index]).SetTo(value);
        }

        [DataMember(Name = "results", IsRequired = false, EmitDefaultValue = false)]
        public IList<Result> Results
        {
            get => _table.Database.Result.List(_table.Results[_index]);
            set => _table.Database.Result.List(_table.Results[_index]).SetTo(value);
        }

        [DataMember(Name = "taxonomies", IsRequired = false, EmitDefaultValue = false)]
        public IList<ToolComponent> Taxonomies
        {
            get => _table.Database.ToolComponent.List(_table.Taxonomies[_index]);
            set => _table.Database.ToolComponent.List(_table.Taxonomies[_index]).SetTo(value);
        }

        [DataMember(Name = "driver", IsRequired = false, EmitDefaultValue = false)]
        public ToolComponent Driver
        {
            get => _table.Database.ToolComponent.Get(_table.Driver[_index]);
            set => _table.Driver[_index] = _table.Database.ToolComponent.LocalIndex(value);
        }

        [DataMember(Name = "extensions", IsRequired = false, EmitDefaultValue = false)]
        public IList<ToolComponent> Extensions
        {
            get => _table.Database.ToolComponent.List(_table.Extensions[_index]);
            set => _table.Database.ToolComponent.List(_table.Extensions[_index]).SetTo(value);
        }

        [DataMember(Name = "policies", IsRequired = false, EmitDefaultValue = false)]
        public IList<ToolComponent> Policies
        {
            get => _table.Database.ToolComponent.List(_table.Policies[_index]);
            set => _table.Database.ToolComponent.List(_table.Policies[_index]).SetTo(value);
        }

        [DataMember(Name = "translations", IsRequired = false, EmitDefaultValue = false)]
        public IList<ToolComponent> Translations
        {
            get => _table.Database.ToolComponent.List(_table.Translations[_index]);
            set => _table.Database.ToolComponent.List(_table.Translations[_index]).SetTo(value);
        }

        [DataMember(Name = "addresses", IsRequired = false, EmitDefaultValue = false)]
        public IList<Address> Addresses
        {
            get => _table.Database.Address.List(_table.Addresses[_index]);
            set => _table.Database.Address.List(_table.Addresses[_index]).SetTo(value);
        }

        [DataMember(Name = "webRequests", IsRequired = false, EmitDefaultValue = false)]
        public IList<WebRequest> WebRequests
        {
            get => _table.Database.WebRequest.List(_table.WebRequests[_index]);
            set => _table.Database.WebRequest.List(_table.WebRequests[_index]).SetTo(value);
        }

        [DataMember(Name = "webResponses", IsRequired = false, EmitDefaultValue = false)]
        public IList<WebResponse> WebResponses
        {
            get => _table.Database.WebResponse.List(_table.WebResponses[_index]);
            set => _table.Database.WebResponse.List(_table.WebResponses[_index]).SetTo(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<ExternalProperties>
        public bool Equals(ExternalProperties other)
        {
            if (other == null) { return false; }

            if (this.Schema != other.Schema) { return false; }
            if (this.Version != other.Version) { return false; }
            if (this.Guid != other.Guid) { return false; }
            if (this.RunGuid != other.RunGuid) { return false; }
            if (this.Conversion != other.Conversion) { return false; }
            if (this.Graphs != other.Graphs) { return false; }
            if (this.ExternalizedProperties != other.ExternalizedProperties) { return false; }
            if (this.Artifacts != other.Artifacts) { return false; }
            if (this.Invocations != other.Invocations) { return false; }
            if (this.LogicalLocations != other.LogicalLocations) { return false; }
            if (this.ThreadFlowLocations != other.ThreadFlowLocations) { return false; }
            if (this.Results != other.Results) { return false; }
            if (this.Taxonomies != other.Taxonomies) { return false; }
            if (this.Driver != other.Driver) { return false; }
            if (this.Extensions != other.Extensions) { return false; }
            if (this.Policies != other.Policies) { return false; }
            if (this.Translations != other.Translations) { return false; }
            if (this.Addresses != other.Addresses) { return false; }
            if (this.WebRequests != other.WebRequests) { return false; }
            if (this.WebResponses != other.WebResponses) { return false; }
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
                if (Schema != default(Uri))
                {
                    result = (result * 31) + Schema.GetHashCode();
                }

                if (Version != default(SarifVersion))
                {
                    result = (result * 31) + Version.GetHashCode();
                }

                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (RunGuid != default(string))
                {
                    result = (result * 31) + RunGuid.GetHashCode();
                }

                if (Conversion != default(Conversion))
                {
                    result = (result * 31) + Conversion.GetHashCode();
                }

                if (Graphs != default(IList<Graph>))
                {
                    result = (result * 31) + Graphs.GetHashCode();
                }

                if (ExternalizedProperties != default(PropertyBag))
                {
                    result = (result * 31) + ExternalizedProperties.GetHashCode();
                }

                if (Artifacts != default(IList<Artifact>))
                {
                    result = (result * 31) + Artifacts.GetHashCode();
                }

                if (Invocations != default(IList<Invocation>))
                {
                    result = (result * 31) + Invocations.GetHashCode();
                }

                if (LogicalLocations != default(IList<LogicalLocation>))
                {
                    result = (result * 31) + LogicalLocations.GetHashCode();
                }

                if (ThreadFlowLocations != default(IList<ThreadFlowLocation>))
                {
                    result = (result * 31) + ThreadFlowLocations.GetHashCode();
                }

                if (Results != default(IList<Result>))
                {
                    result = (result * 31) + Results.GetHashCode();
                }

                if (Taxonomies != default(IList<ToolComponent>))
                {
                    result = (result * 31) + Taxonomies.GetHashCode();
                }

                if (Driver != default(ToolComponent))
                {
                    result = (result * 31) + Driver.GetHashCode();
                }

                if (Extensions != default(IList<ToolComponent>))
                {
                    result = (result * 31) + Extensions.GetHashCode();
                }

                if (Policies != default(IList<ToolComponent>))
                {
                    result = (result * 31) + Policies.GetHashCode();
                }

                if (Translations != default(IList<ToolComponent>))
                {
                    result = (result * 31) + Translations.GetHashCode();
                }

                if (Addresses != default(IList<Address>))
                {
                    result = (result * 31) + Addresses.GetHashCode();
                }

                if (WebRequests != default(IList<WebRequest>))
                {
                    result = (result * 31) + WebRequests.GetHashCode();
                }

                if (WebResponses != default(IList<WebResponse>))
                {
                    result = (result * 31) + WebResponses.GetHashCode();
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
            return Equals(obj as ExternalProperties);
        }

        public static bool operator ==(ExternalProperties left, ExternalProperties right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(ExternalProperties left, ExternalProperties right)
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
            _table = (ExternalPropertiesTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.ExternalProperties;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public ExternalProperties DeepClone()
        {
            return (ExternalProperties)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new ExternalProperties(this);
        }
        #endregion

        public static IEqualityComparer<ExternalProperties> ValueComparer => EqualityComparer<ExternalProperties>.Default;
        public bool ValueEquals(ExternalProperties other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
