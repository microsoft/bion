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
    ///  GENERATED: BSOA Entity for 'Run'
    /// </summary>
    [DataContract]
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Run : PropertyBagHolder, ISarifNode, IRow
    {
        private RunTable _table;
        private int _index;

        public Run() : this(SarifLogDatabase.Current.Run)
        { }

        public Run(SarifLog root) : this(root.Database.Run)
        { }

        internal Run(RunTable table) : this(table, table.Count)
        {
            table.Add();
        }

        internal Run(RunTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Run(
            Tool tool,
            IList<Invocation> invocations,
            Conversion conversion,
            string language,
            IList<VersionControlDetails> versionControlProvenance,
            IDictionary<string, ArtifactLocation> originalUriBaseIds,
            IList<Artifact> artifacts,
            IList<LogicalLocation> logicalLocations,
            IList<Graph> graphs,
            IList<Result> results,
            RunAutomationDetails automationDetails,
            IList<RunAutomationDetails> runAggregates,
            string baselineGuid,
            IList<string> redactionTokens,
            string defaultEncoding,
            string defaultSourceLanguage,
            IList<string> newlineSequences,
            ColumnKind columnKind,
            ExternalPropertyFileReferences externalPropertyFileReferences,
            IList<ThreadFlowLocation> threadFlowLocations,
            IList<ToolComponent> taxonomies,
            IList<Address> addresses,
            IList<ToolComponent> translations,
            IList<ToolComponent> policies,
            IList<WebRequest> webRequests,
            IList<WebResponse> webResponses,
            SpecialLocations specialLocations,
            IDictionary<string, string> properties
        ) 
            : this(SarifLogDatabase.Current.Run)
        {
            Tool = tool;
            Invocations = invocations;
            Conversion = conversion;
            Language = language;
            VersionControlProvenance = versionControlProvenance;
            OriginalUriBaseIds = originalUriBaseIds;
            Artifacts = artifacts;
            LogicalLocations = logicalLocations;
            Graphs = graphs;
            Results = results;
            AutomationDetails = automationDetails;
            RunAggregates = runAggregates;
            BaselineGuid = baselineGuid;
            RedactionTokens = redactionTokens;
            DefaultEncoding = defaultEncoding;
            DefaultSourceLanguage = defaultSourceLanguage;
            NewlineSequences = newlineSequences;
            ColumnKind = columnKind;
            ExternalPropertyFileReferences = externalPropertyFileReferences;
            ThreadFlowLocations = threadFlowLocations;
            Taxonomies = taxonomies;
            Addresses = addresses;
            Translations = translations;
            Policies = policies;
            WebRequests = webRequests;
            WebResponses = webResponses;
            SpecialLocations = specialLocations;
            Properties = properties;
        }

        public Run(Run other) 
            : this(SarifLogDatabase.Current.Run)
        {
            Tool = other.Tool;
            Invocations = other.Invocations;
            Conversion = other.Conversion;
            Language = other.Language;
            VersionControlProvenance = other.VersionControlProvenance;
            OriginalUriBaseIds = other.OriginalUriBaseIds;
            Artifacts = other.Artifacts;
            LogicalLocations = other.LogicalLocations;
            Graphs = other.Graphs;
            Results = other.Results;
            AutomationDetails = other.AutomationDetails;
            RunAggregates = other.RunAggregates;
            BaselineGuid = other.BaselineGuid;
            RedactionTokens = other.RedactionTokens;
            DefaultEncoding = other.DefaultEncoding;
            DefaultSourceLanguage = other.DefaultSourceLanguage;
            NewlineSequences = other.NewlineSequences;
            ColumnKind = other.ColumnKind;
            ExternalPropertyFileReferences = other.ExternalPropertyFileReferences;
            ThreadFlowLocations = other.ThreadFlowLocations;
            Taxonomies = other.Taxonomies;
            Addresses = other.Addresses;
            Translations = other.Translations;
            Policies = other.Policies;
            WebRequests = other.WebRequests;
            WebResponses = other.WebResponses;
            SpecialLocations = other.SpecialLocations;
            Properties = other.Properties;
        }

        [DataMember(Name = "tool", IsRequired = false, EmitDefaultValue = false)]
        public Tool Tool
        {
            get => _table.Database.Tool.Get(_table.Tool[_index]);
            set => _table.Tool[_index] = _table.Database.Tool.LocalIndex(value);
        }

        [DataMember(Name = "invocations", IsRequired = false, EmitDefaultValue = false)]
        public IList<Invocation> Invocations
        {
            get => _table.Database.Invocation.List(_table.Invocations[_index]);
            set => _table.Database.Invocation.List(_table.Invocations[_index]).SetTo(value);
        }

        [DataMember(Name = "conversion", IsRequired = false, EmitDefaultValue = false)]
        public Conversion Conversion
        {
            get => _table.Database.Conversion.Get(_table.Conversion[_index]);
            set => _table.Conversion[_index] = _table.Database.Conversion.LocalIndex(value);
        }

        [DataMember(Name = "language", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue("en-US")]
        public string Language
        {
            get => _table.Language[_index];
            set => _table.Language[_index] = value;
        }

        [DataMember(Name = "versionControlProvenance", IsRequired = false, EmitDefaultValue = false)]
        public IList<VersionControlDetails> VersionControlProvenance
        {
            get => _table.Database.VersionControlDetails.List(_table.VersionControlProvenance[_index]);
            set => _table.Database.VersionControlDetails.List(_table.VersionControlProvenance[_index]).SetTo(value);
        }

        [DataMember(Name = "originalUriBaseIds", IsRequired = false, EmitDefaultValue = false)]
        public IDictionary<string, ArtifactLocation> OriginalUriBaseIds
        {
            get => _table.OriginalUriBaseIds[_index];
            set => _table.OriginalUriBaseIds[_index] = value;
        }

        [DataMember(Name = "artifacts", IsRequired = false, EmitDefaultValue = false)]
        public IList<Artifact> Artifacts
        {
            get => _table.Database.Artifact.List(_table.Artifacts[_index]);
            set => _table.Database.Artifact.List(_table.Artifacts[_index]).SetTo(value);
        }

        [DataMember(Name = "logicalLocations", IsRequired = false, EmitDefaultValue = false)]
        public IList<LogicalLocation> LogicalLocations
        {
            get => _table.Database.LogicalLocation.List(_table.LogicalLocations[_index]);
            set => _table.Database.LogicalLocation.List(_table.LogicalLocations[_index]).SetTo(value);
        }

        [DataMember(Name = "graphs", IsRequired = false, EmitDefaultValue = false)]
        public IList<Graph> Graphs
        {
            get => _table.Database.Graph.List(_table.Graphs[_index]);
            set => _table.Database.Graph.List(_table.Graphs[_index]).SetTo(value);
        }

        [DataMember(Name = "results", IsRequired = false, EmitDefaultValue = false)]
        public IList<Result> Results
        {
            get => _table.Database.Result.List(_table.Results[_index]);
            set => _table.Database.Result.List(_table.Results[_index]).SetTo(value);
        }

        [DataMember(Name = "automationDetails", IsRequired = false, EmitDefaultValue = false)]
        public RunAutomationDetails AutomationDetails
        {
            get => _table.Database.RunAutomationDetails.Get(_table.AutomationDetails[_index]);
            set => _table.AutomationDetails[_index] = _table.Database.RunAutomationDetails.LocalIndex(value);
        }

        [DataMember(Name = "runAggregates", IsRequired = false, EmitDefaultValue = false)]
        public IList<RunAutomationDetails> RunAggregates
        {
            get => _table.Database.RunAutomationDetails.List(_table.RunAggregates[_index]);
            set => _table.Database.RunAutomationDetails.List(_table.RunAggregates[_index]).SetTo(value);
        }

        [DataMember(Name = "baselineGuid", IsRequired = false, EmitDefaultValue = false)]
        public string BaselineGuid
        {
            get => _table.BaselineGuid[_index];
            set => _table.BaselineGuid[_index] = value;
        }

        [DataMember(Name = "redactionTokens", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> RedactionTokens
        {
            get => _table.RedactionTokens[_index];
            set => _table.RedactionTokens[_index] = value;
        }

        [DataMember(Name = "defaultEncoding", IsRequired = false, EmitDefaultValue = false)]
        public string DefaultEncoding
        {
            get => _table.DefaultEncoding[_index];
            set => _table.DefaultEncoding[_index] = value;
        }

        [DataMember(Name = "defaultSourceLanguage", IsRequired = false, EmitDefaultValue = false)]
        public string DefaultSourceLanguage
        {
            get => _table.DefaultSourceLanguage[_index];
            set => _table.DefaultSourceLanguage[_index] = value;
        }

        [DataMember(Name = "newlineSequences", IsRequired = false, EmitDefaultValue = false)]
        public IList<string> NewlineSequences
        {
            get => _table.NewlineSequences[_index];
            set => _table.NewlineSequences[_index] = value;
        }

        [DataMember(Name = "columnKind", IsRequired = false, EmitDefaultValue = false)]
        [JsonConverter(typeof(Microsoft.CodeAnalysis.Sarif.Readers.EnumConverter))]
        public ColumnKind ColumnKind
        {
            get => (ColumnKind)_table.ColumnKind[_index];
            set => _table.ColumnKind[_index] = (int)value;
        }

        [DataMember(Name = "externalPropertyFileReferences", IsRequired = false, EmitDefaultValue = false)]
        public ExternalPropertyFileReferences ExternalPropertyFileReferences
        {
            get => _table.Database.ExternalPropertyFileReferences.Get(_table.ExternalPropertyFileReferences[_index]);
            set => _table.ExternalPropertyFileReferences[_index] = _table.Database.ExternalPropertyFileReferences.LocalIndex(value);
        }

        [DataMember(Name = "threadFlowLocations", IsRequired = false, EmitDefaultValue = false)]
        public IList<ThreadFlowLocation> ThreadFlowLocations
        {
            get => _table.Database.ThreadFlowLocation.List(_table.ThreadFlowLocations[_index]);
            set => _table.Database.ThreadFlowLocation.List(_table.ThreadFlowLocations[_index]).SetTo(value);
        }

        [DataMember(Name = "taxonomies", IsRequired = false, EmitDefaultValue = false)]
        public IList<ToolComponent> Taxonomies
        {
            get => _table.Database.ToolComponent.List(_table.Taxonomies[_index]);
            set => _table.Database.ToolComponent.List(_table.Taxonomies[_index]).SetTo(value);
        }

        [DataMember(Name = "addresses", IsRequired = false, EmitDefaultValue = false)]
        public IList<Address> Addresses
        {
            get => _table.Database.Address.List(_table.Addresses[_index]);
            set => _table.Database.Address.List(_table.Addresses[_index]).SetTo(value);
        }

        [DataMember(Name = "translations", IsRequired = false, EmitDefaultValue = false)]
        public IList<ToolComponent> Translations
        {
            get => _table.Database.ToolComponent.List(_table.Translations[_index]);
            set => _table.Database.ToolComponent.List(_table.Translations[_index]).SetTo(value);
        }

        [DataMember(Name = "policies", IsRequired = false, EmitDefaultValue = false)]
        public IList<ToolComponent> Policies
        {
            get => _table.Database.ToolComponent.List(_table.Policies[_index]);
            set => _table.Database.ToolComponent.List(_table.Policies[_index]).SetTo(value);
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

        [DataMember(Name = "specialLocations", IsRequired = false, EmitDefaultValue = false)]
        public SpecialLocations SpecialLocations
        {
            get => _table.Database.SpecialLocations.Get(_table.SpecialLocations[_index]);
            set => _table.SpecialLocations[_index] = _table.Database.SpecialLocations.LocalIndex(value);
        }

        [DataMember(Name = "properties", IsRequired = false, EmitDefaultValue = false)]
        internal override IDictionary<string, string> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Run>
        public bool Equals(Run other)
        {
            if (other == null) { return false; }

            if (this.Tool != other.Tool) { return false; }
            if (this.Invocations != other.Invocations) { return false; }
            if (this.Conversion != other.Conversion) { return false; }
            if (this.Language != other.Language) { return false; }
            if (this.VersionControlProvenance != other.VersionControlProvenance) { return false; }
            if (this.OriginalUriBaseIds != other.OriginalUriBaseIds) { return false; }
            if (this.Artifacts != other.Artifacts) { return false; }
            if (this.LogicalLocations != other.LogicalLocations) { return false; }
            if (this.Graphs != other.Graphs) { return false; }
            if (this.Results != other.Results) { return false; }
            if (this.AutomationDetails != other.AutomationDetails) { return false; }
            if (this.RunAggregates != other.RunAggregates) { return false; }
            if (this.BaselineGuid != other.BaselineGuid) { return false; }
            if (this.RedactionTokens != other.RedactionTokens) { return false; }
            if (this.DefaultEncoding != other.DefaultEncoding) { return false; }
            if (this.DefaultSourceLanguage != other.DefaultSourceLanguage) { return false; }
            if (this.NewlineSequences != other.NewlineSequences) { return false; }
            if (this.ColumnKind != other.ColumnKind) { return false; }
            if (this.ExternalPropertyFileReferences != other.ExternalPropertyFileReferences) { return false; }
            if (this.ThreadFlowLocations != other.ThreadFlowLocations) { return false; }
            if (this.Taxonomies != other.Taxonomies) { return false; }
            if (this.Addresses != other.Addresses) { return false; }
            if (this.Translations != other.Translations) { return false; }
            if (this.Policies != other.Policies) { return false; }
            if (this.WebRequests != other.WebRequests) { return false; }
            if (this.WebResponses != other.WebResponses) { return false; }
            if (this.SpecialLocations != other.SpecialLocations) { return false; }
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
                if (Tool != default(Tool))
                {
                    result = (result * 31) + Tool.GetHashCode();
                }

                if (Invocations != default(IList<Invocation>))
                {
                    result = (result * 31) + Invocations.GetHashCode();
                }

                if (Conversion != default(Conversion))
                {
                    result = (result * 31) + Conversion.GetHashCode();
                }

                if (Language != default(string))
                {
                    result = (result * 31) + Language.GetHashCode();
                }

                if (VersionControlProvenance != default(IList<VersionControlDetails>))
                {
                    result = (result * 31) + VersionControlProvenance.GetHashCode();
                }

                if (OriginalUriBaseIds != default(IDictionary<string, ArtifactLocation>))
                {
                    result = (result * 31) + OriginalUriBaseIds.GetHashCode();
                }

                if (Artifacts != default(IList<Artifact>))
                {
                    result = (result * 31) + Artifacts.GetHashCode();
                }

                if (LogicalLocations != default(IList<LogicalLocation>))
                {
                    result = (result * 31) + LogicalLocations.GetHashCode();
                }

                if (Graphs != default(IList<Graph>))
                {
                    result = (result * 31) + Graphs.GetHashCode();
                }

                if (Results != default(IList<Result>))
                {
                    result = (result * 31) + Results.GetHashCode();
                }

                if (AutomationDetails != default(RunAutomationDetails))
                {
                    result = (result * 31) + AutomationDetails.GetHashCode();
                }

                if (RunAggregates != default(IList<RunAutomationDetails>))
                {
                    result = (result * 31) + RunAggregates.GetHashCode();
                }

                if (BaselineGuid != default(string))
                {
                    result = (result * 31) + BaselineGuid.GetHashCode();
                }

                if (RedactionTokens != default(IList<string>))
                {
                    result = (result * 31) + RedactionTokens.GetHashCode();
                }

                if (DefaultEncoding != default(string))
                {
                    result = (result * 31) + DefaultEncoding.GetHashCode();
                }

                if (DefaultSourceLanguage != default(string))
                {
                    result = (result * 31) + DefaultSourceLanguage.GetHashCode();
                }

                if (NewlineSequences != default(IList<string>))
                {
                    result = (result * 31) + NewlineSequences.GetHashCode();
                }

                if (ColumnKind != default(ColumnKind))
                {
                    result = (result * 31) + ColumnKind.GetHashCode();
                }

                if (ExternalPropertyFileReferences != default(ExternalPropertyFileReferences))
                {
                    result = (result * 31) + ExternalPropertyFileReferences.GetHashCode();
                }

                if (ThreadFlowLocations != default(IList<ThreadFlowLocation>))
                {
                    result = (result * 31) + ThreadFlowLocations.GetHashCode();
                }

                if (Taxonomies != default(IList<ToolComponent>))
                {
                    result = (result * 31) + Taxonomies.GetHashCode();
                }

                if (Addresses != default(IList<Address>))
                {
                    result = (result * 31) + Addresses.GetHashCode();
                }

                if (Translations != default(IList<ToolComponent>))
                {
                    result = (result * 31) + Translations.GetHashCode();
                }

                if (Policies != default(IList<ToolComponent>))
                {
                    result = (result * 31) + Policies.GetHashCode();
                }

                if (WebRequests != default(IList<WebRequest>))
                {
                    result = (result * 31) + WebRequests.GetHashCode();
                }

                if (WebResponses != default(IList<WebResponse>))
                {
                    result = (result * 31) + WebResponses.GetHashCode();
                }

                if (SpecialLocations != default(SpecialLocations))
                {
                    result = (result * 31) + SpecialLocations.GetHashCode();
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
            return Equals(obj as Run);
        }

        public static bool operator ==(Run left, Run right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Run left, Run right)
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
            _table = (RunTable)table;
            _index = index;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Run;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Run DeepClone()
        {
            return (Run)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Run(this);
        }
        #endregion

        public static IEqualityComparer<Run> ValueComparer => EqualityComparer<Run>.Default;
        public bool ValueEquals(Run other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
