// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

using BSOA.Model;

using Microsoft.CodeAnalysis.Sarif;
using Microsoft.CodeAnalysis.Sarif.Readers;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  GENERATED: BSOA Entity for 'Result'
    /// </summary>
    [GeneratedCode("BSOA.Generator", "0.5.0")]
    public partial class Result : PropertyBagHolder, ISarifNode, IRow
    {
        private ResultTable _table;
        private int _index;

        public Result() : this(SarifLogDatabase.Current.Result)
        { }

        public Result(SarifLog root) : this(root.Database.Result)
        { }

        internal Result(ResultTable table) : this(table, table.Count)
        {
            table.Add();
            Init();
        }

        internal Result(ResultTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public Result(
            string ruleId,
            int ruleIndex,
            ReportingDescriptorReference rule,
            ResultKind kind,
            FailureLevel level,
            Message message,
            ArtifactLocation analysisTarget,
            IList<Location> locations,
            string guid,
            string correlationGuid,
            int occurrenceCount,
            IDictionary<string, string> partialFingerprints,
            IDictionary<string, string> fingerprints,
            IList<Stack> stacks,
            IList<CodeFlow> codeFlows,
            IList<Graph> graphs,
            IList<GraphTraversal> graphTraversals,
            IList<Location> relatedLocations,
            IList<Suppression> suppressions,
            BaselineState baselineState,
            double rank,
            IList<Attachment> attachments,
            Uri hostedViewerUri,
            IList<Uri> workItemUris,
            ResultProvenance provenance,
            IList<Fix> fixes,
            IList<ReportingDescriptorReference> taxa,
            WebRequest webRequest,
            WebResponse webResponse,
            IDictionary<string, SerializedPropertyInfo> properties
        ) 
            : this(SarifLogDatabase.Current.Result)
        {
            RuleId = ruleId;
            RuleIndex = ruleIndex;
            Rule = rule;
            Kind = kind;
            Level = level;
            Message = message;
            AnalysisTarget = analysisTarget;
            Locations = locations;
            Guid = guid;
            CorrelationGuid = correlationGuid;
            OccurrenceCount = occurrenceCount;
            PartialFingerprints = partialFingerprints;
            Fingerprints = fingerprints;
            Stacks = stacks;
            CodeFlows = codeFlows;
            Graphs = graphs;
            GraphTraversals = graphTraversals;
            RelatedLocations = relatedLocations;
            Suppressions = suppressions;
            BaselineState = baselineState;
            Rank = rank;
            Attachments = attachments;
            HostedViewerUri = hostedViewerUri;
            WorkItemUris = workItemUris;
            Provenance = provenance;
            Fixes = fixes;
            Taxa = taxa;
            WebRequest = webRequest;
            WebResponse = webResponse;
            Properties = properties;
        }

        public Result(Result other) 
            : this(SarifLogDatabase.Current.Result)
        {
            RuleId = other.RuleId;
            RuleIndex = other.RuleIndex;
            Rule = other.Rule;
            Kind = other.Kind;
            Level = other.Level;
            Message = other.Message;
            AnalysisTarget = other.AnalysisTarget;
            Locations = other.Locations;
            Guid = other.Guid;
            CorrelationGuid = other.CorrelationGuid;
            OccurrenceCount = other.OccurrenceCount;
            PartialFingerprints = other.PartialFingerprints;
            Fingerprints = other.Fingerprints;
            Stacks = other.Stacks;
            CodeFlows = other.CodeFlows;
            Graphs = other.Graphs;
            GraphTraversals = other.GraphTraversals;
            RelatedLocations = other.RelatedLocations;
            Suppressions = other.Suppressions;
            BaselineState = other.BaselineState;
            Rank = other.Rank;
            Attachments = other.Attachments;
            HostedViewerUri = other.HostedViewerUri;
            WorkItemUris = other.WorkItemUris;
            Provenance = other.Provenance;
            Fixes = other.Fixes;
            Taxa = other.Taxa;
            WebRequest = other.WebRequest;
            WebResponse = other.WebResponse;
            Properties = other.Properties;
        }

        partial void Init();

        public string RuleId
        {
            get => _table.RuleId[_index];
            set => _table.RuleId[_index] = value;
        }

        public int RuleIndex
        {
            get => _table.RuleIndex[_index];
            set => _table.RuleIndex[_index] = value;
        }

        public ReportingDescriptorReference Rule
        {
            get => _table.Database.ReportingDescriptorReference.Get(_table.Rule[_index]);
            set => _table.Rule[_index] = _table.Database.ReportingDescriptorReference.LocalIndex(value);
        }

        public ResultKind Kind
        {
            get => (ResultKind)_table.Kind[_index];
            set => _table.Kind[_index] = (int)value;
        }

        public FailureLevel Level
        {
            get => (FailureLevel)_table.Level[_index];
            set => _table.Level[_index] = (int)value;
        }

        public Message Message
        {
            get => _table.Database.Message.Get(_table.Message[_index]);
            set => _table.Message[_index] = _table.Database.Message.LocalIndex(value);
        }

        public ArtifactLocation AnalysisTarget
        {
            get => _table.Database.ArtifactLocation.Get(_table.AnalysisTarget[_index]);
            set => _table.AnalysisTarget[_index] = _table.Database.ArtifactLocation.LocalIndex(value);
        }

        public IList<Location> Locations
        {
            get => _table.Database.Location.List(_table.Locations[_index]);
            set => _table.Database.Location.List(_table.Locations[_index]).SetTo(value);
        }

        public string Guid
        {
            get => _table.Guid[_index];
            set => _table.Guid[_index] = value;
        }

        public string CorrelationGuid
        {
            get => _table.CorrelationGuid[_index];
            set => _table.CorrelationGuid[_index] = value;
        }

        public int OccurrenceCount
        {
            get => _table.OccurrenceCount[_index];
            set => _table.OccurrenceCount[_index] = value;
        }

        public IDictionary<string, string> PartialFingerprints
        {
            get => _table.PartialFingerprints[_index];
            set => _table.PartialFingerprints[_index] = value;
        }

        public IDictionary<string, string> Fingerprints
        {
            get => _table.Fingerprints[_index];
            set => _table.Fingerprints[_index] = value;
        }

        public IList<Stack> Stacks
        {
            get => _table.Database.Stack.List(_table.Stacks[_index]);
            set => _table.Database.Stack.List(_table.Stacks[_index]).SetTo(value);
        }

        public IList<CodeFlow> CodeFlows
        {
            get => _table.Database.CodeFlow.List(_table.CodeFlows[_index]);
            set => _table.Database.CodeFlow.List(_table.CodeFlows[_index]).SetTo(value);
        }

        public IList<Graph> Graphs
        {
            get => _table.Database.Graph.List(_table.Graphs[_index]);
            set => _table.Database.Graph.List(_table.Graphs[_index]).SetTo(value);
        }

        public IList<GraphTraversal> GraphTraversals
        {
            get => _table.Database.GraphTraversal.List(_table.GraphTraversals[_index]);
            set => _table.Database.GraphTraversal.List(_table.GraphTraversals[_index]).SetTo(value);
        }

        public IList<Location> RelatedLocations
        {
            get => _table.Database.Location.List(_table.RelatedLocations[_index]);
            set => _table.Database.Location.List(_table.RelatedLocations[_index]).SetTo(value);
        }

        public IList<Suppression> Suppressions
        {
            get => _table.Database.Suppression.List(_table.Suppressions[_index]);
            set => _table.Database.Suppression.List(_table.Suppressions[_index]).SetTo(value);
        }

        public BaselineState BaselineState
        {
            get => (BaselineState)_table.BaselineState[_index];
            set => _table.BaselineState[_index] = (int)value;
        }

        public double Rank
        {
            get => _table.Rank[_index];
            set => _table.Rank[_index] = value;
        }

        public IList<Attachment> Attachments
        {
            get => _table.Database.Attachment.List(_table.Attachments[_index]);
            set => _table.Database.Attachment.List(_table.Attachments[_index]).SetTo(value);
        }

        public Uri HostedViewerUri
        {
            get => _table.HostedViewerUri[_index];
            set => _table.HostedViewerUri[_index] = value;
        }

        public IList<Uri> WorkItemUris
        {
            get => _table.WorkItemUris[_index];
            set => _table.WorkItemUris[_index] = value;
        }

        public ResultProvenance Provenance
        {
            get => _table.Database.ResultProvenance.Get(_table.Provenance[_index]);
            set => _table.Provenance[_index] = _table.Database.ResultProvenance.LocalIndex(value);
        }

        public IList<Fix> Fixes
        {
            get => _table.Database.Fix.List(_table.Fixes[_index]);
            set => _table.Database.Fix.List(_table.Fixes[_index]).SetTo(value);
        }

        public IList<ReportingDescriptorReference> Taxa
        {
            get => _table.Database.ReportingDescriptorReference.List(_table.Taxa[_index]);
            set => _table.Database.ReportingDescriptorReference.List(_table.Taxa[_index]).SetTo(value);
        }

        public WebRequest WebRequest
        {
            get => _table.Database.WebRequest.Get(_table.WebRequest[_index]);
            set => _table.WebRequest[_index] = _table.Database.WebRequest.LocalIndex(value);
        }

        public WebResponse WebResponse
        {
            get => _table.Database.WebResponse.Get(_table.WebResponse[_index]);
            set => _table.WebResponse[_index] = _table.Database.WebResponse.LocalIndex(value);
        }

        internal override IDictionary<string, SerializedPropertyInfo> Properties
        {
            get => _table.Properties[_index];
            set => _table.Properties[_index] = value;
        }

        #region IEquatable<Result>
        public bool Equals(Result other)
        {
            if (other == null) { return false; }

            if (this.RuleId != other.RuleId) { return false; }
            if (this.RuleIndex != other.RuleIndex) { return false; }
            if (this.Rule != other.Rule) { return false; }
            if (this.Kind != other.Kind) { return false; }
            if (this.Level != other.Level) { return false; }
            if (this.Message != other.Message) { return false; }
            if (this.AnalysisTarget != other.AnalysisTarget) { return false; }
            if (this.Locations != other.Locations) { return false; }
            if (this.Guid != other.Guid) { return false; }
            if (this.CorrelationGuid != other.CorrelationGuid) { return false; }
            if (this.OccurrenceCount != other.OccurrenceCount) { return false; }
            if (this.PartialFingerprints != other.PartialFingerprints) { return false; }
            if (this.Fingerprints != other.Fingerprints) { return false; }
            if (this.Stacks != other.Stacks) { return false; }
            if (this.CodeFlows != other.CodeFlows) { return false; }
            if (this.Graphs != other.Graphs) { return false; }
            if (this.GraphTraversals != other.GraphTraversals) { return false; }
            if (this.RelatedLocations != other.RelatedLocations) { return false; }
            if (this.Suppressions != other.Suppressions) { return false; }
            if (this.BaselineState != other.BaselineState) { return false; }
            if (this.Rank != other.Rank) { return false; }
            if (this.Attachments != other.Attachments) { return false; }
            if (this.HostedViewerUri != other.HostedViewerUri) { return false; }
            if (this.WorkItemUris != other.WorkItemUris) { return false; }
            if (this.Provenance != other.Provenance) { return false; }
            if (this.Fixes != other.Fixes) { return false; }
            if (this.Taxa != other.Taxa) { return false; }
            if (this.WebRequest != other.WebRequest) { return false; }
            if (this.WebResponse != other.WebResponse) { return false; }
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
                if (RuleId != default(string))
                {
                    result = (result * 31) + RuleId.GetHashCode();
                }

                if (RuleIndex != default(int))
                {
                    result = (result * 31) + RuleIndex.GetHashCode();
                }

                if (Rule != default(ReportingDescriptorReference))
                {
                    result = (result * 31) + Rule.GetHashCode();
                }

                if (Kind != default(ResultKind))
                {
                    result = (result * 31) + Kind.GetHashCode();
                }

                if (Level != default(FailureLevel))
                {
                    result = (result * 31) + Level.GetHashCode();
                }

                if (Message != default(Message))
                {
                    result = (result * 31) + Message.GetHashCode();
                }

                if (AnalysisTarget != default(ArtifactLocation))
                {
                    result = (result * 31) + AnalysisTarget.GetHashCode();
                }

                if (Locations != default(IList<Location>))
                {
                    result = (result * 31) + Locations.GetHashCode();
                }

                if (Guid != default(string))
                {
                    result = (result * 31) + Guid.GetHashCode();
                }

                if (CorrelationGuid != default(string))
                {
                    result = (result * 31) + CorrelationGuid.GetHashCode();
                }

                if (OccurrenceCount != default(int))
                {
                    result = (result * 31) + OccurrenceCount.GetHashCode();
                }

                if (PartialFingerprints != default(IDictionary<string, string>))
                {
                    result = (result * 31) + PartialFingerprints.GetHashCode();
                }

                if (Fingerprints != default(IDictionary<string, string>))
                {
                    result = (result * 31) + Fingerprints.GetHashCode();
                }

                if (Stacks != default(IList<Stack>))
                {
                    result = (result * 31) + Stacks.GetHashCode();
                }

                if (CodeFlows != default(IList<CodeFlow>))
                {
                    result = (result * 31) + CodeFlows.GetHashCode();
                }

                if (Graphs != default(IList<Graph>))
                {
                    result = (result * 31) + Graphs.GetHashCode();
                }

                if (GraphTraversals != default(IList<GraphTraversal>))
                {
                    result = (result * 31) + GraphTraversals.GetHashCode();
                }

                if (RelatedLocations != default(IList<Location>))
                {
                    result = (result * 31) + RelatedLocations.GetHashCode();
                }

                if (Suppressions != default(IList<Suppression>))
                {
                    result = (result * 31) + Suppressions.GetHashCode();
                }

                if (BaselineState != default(BaselineState))
                {
                    result = (result * 31) + BaselineState.GetHashCode();
                }

                if (Rank != default(double))
                {
                    result = (result * 31) + Rank.GetHashCode();
                }

                if (Attachments != default(IList<Attachment>))
                {
                    result = (result * 31) + Attachments.GetHashCode();
                }

                if (HostedViewerUri != default(Uri))
                {
                    result = (result * 31) + HostedViewerUri.GetHashCode();
                }

                if (WorkItemUris != default(IList<Uri>))
                {
                    result = (result * 31) + WorkItemUris.GetHashCode();
                }

                if (Provenance != default(ResultProvenance))
                {
                    result = (result * 31) + Provenance.GetHashCode();
                }

                if (Fixes != default(IList<Fix>))
                {
                    result = (result * 31) + Fixes.GetHashCode();
                }

                if (Taxa != default(IList<ReportingDescriptorReference>))
                {
                    result = (result * 31) + Taxa.GetHashCode();
                }

                if (WebRequest != default(WebRequest))
                {
                    result = (result * 31) + WebRequest.GetHashCode();
                }

                if (WebResponse != default(WebResponse))
                {
                    result = (result * 31) + WebResponse.GetHashCode();
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
            return Equals(obj as Result);
        }

        public static bool operator ==(Result left, Result right)
        {
            if (object.ReferenceEquals(left, null))
            {
                return object.ReferenceEquals(right, null);
            }

            return left.Equals(right);
        }

        public static bool operator !=(Result left, Result right)
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

        void IRow.Next()
        {
            _index++;
        }
        #endregion

        #region ISarifNode
        public SarifNodeKind SarifNodeKind => SarifNodeKind.Result;

        ISarifNode ISarifNode.DeepClone()
        {
            return DeepCloneCore();
        }

        /// <summary>
        /// Creates a deep copy of this instance.
        /// </summary>
        public Result DeepClone()
        {
            return (Result)DeepCloneCore();
        }

        private ISarifNode DeepCloneCore()
        {
            return new Result(this);
        }
        #endregion

        public static IEqualityComparer<Result> ValueComparer => EqualityComparer<Result>.Default;
        public bool ValueEquals(Result other) => Equals(other);
        public int ValueGetHashCode() => GetHashCode();
    }
}
