// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Result'
    /// </summary>
    internal partial class ResultTable : Table<Result>
    {
        internal SarifLogDatabase Database;

        internal IColumn<String> RuleId;
        internal IColumn<int> RuleIndex;
        internal RefColumn Rule;
        internal IColumn<int> Kind;
        internal IColumn<int> Level;
        internal RefColumn Message;
        internal RefColumn AnalysisTarget;
        internal RefListColumn Locations;
        internal IColumn<String> Guid;
        internal IColumn<String> CorrelationGuid;
        internal IColumn<int> OccurrenceCount;
        internal IColumn<IDictionary<String, String>> PartialFingerprints;
        internal IColumn<IDictionary<String, String>> Fingerprints;
        internal RefListColumn Stacks;
        internal RefListColumn CodeFlows;
        internal RefListColumn Graphs;
        internal RefListColumn GraphTraversals;
        internal RefListColumn RelatedLocations;
        internal RefListColumn Suppressions;
        internal IColumn<int> BaselineState;
        internal IColumn<double> Rank;
        internal RefListColumn Attachments;
        internal IColumn<Uri> HostedViewerUri;
        internal IColumn<IList<Uri>> WorkItemUris;
        internal RefColumn Provenance;
        internal RefListColumn Fixes;
        internal RefListColumn Taxa;
        internal RefColumn WebRequest;
        internal RefColumn WebResponse;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ResultTable(SarifLogDatabase database) : base()
        {
            Database = database;

            RuleId = AddColumn(nameof(RuleId), database.BuildColumn<String>(nameof(Result), nameof(RuleId), default));
            RuleIndex = AddColumn(nameof(RuleIndex), database.BuildColumn<int>(nameof(Result), nameof(RuleIndex), -1));
            Rule = AddColumn(nameof(Rule), new RefColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            Kind = AddColumn(nameof(Kind), database.BuildColumn<int>(nameof(Result), nameof(Kind), (int)ResultKind.Fail));
            Level = AddColumn(nameof(Level), database.BuildColumn<int>(nameof(Result), nameof(Level), (int)FailureLevel.Warning));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            AnalysisTarget = AddColumn(nameof(AnalysisTarget), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.Location)));
            Guid = AddColumn(nameof(Guid), database.BuildColumn<String>(nameof(Result), nameof(Guid), default));
            CorrelationGuid = AddColumn(nameof(CorrelationGuid), database.BuildColumn<String>(nameof(Result), nameof(CorrelationGuid), default));
            OccurrenceCount = AddColumn(nameof(OccurrenceCount), database.BuildColumn<int>(nameof(Result), nameof(OccurrenceCount), default));
            PartialFingerprints = AddColumn(nameof(PartialFingerprints), database.BuildColumn<IDictionary<String, String>>(nameof(Result), nameof(PartialFingerprints), default));
            Fingerprints = AddColumn(nameof(Fingerprints), database.BuildColumn<IDictionary<String, String>>(nameof(Result), nameof(Fingerprints), default));
            Stacks = AddColumn(nameof(Stacks), new RefListColumn(nameof(SarifLogDatabase.Stack)));
            CodeFlows = AddColumn(nameof(CodeFlows), new RefListColumn(nameof(SarifLogDatabase.CodeFlow)));
            Graphs = AddColumn(nameof(Graphs), new RefListColumn(nameof(SarifLogDatabase.Graph)));
            GraphTraversals = AddColumn(nameof(GraphTraversals), new RefListColumn(nameof(SarifLogDatabase.GraphTraversal)));
            RelatedLocations = AddColumn(nameof(RelatedLocations), new RefListColumn(nameof(SarifLogDatabase.Location)));
            Suppressions = AddColumn(nameof(Suppressions), new RefListColumn(nameof(SarifLogDatabase.Suppression)));
            BaselineState = AddColumn(nameof(BaselineState), database.BuildColumn<int>(nameof(Result), nameof(BaselineState), (int)default(BaselineState)));
            Rank = AddColumn(nameof(Rank), database.BuildColumn<double>(nameof(Result), nameof(Rank), -1));
            Attachments = AddColumn(nameof(Attachments), new RefListColumn(nameof(SarifLogDatabase.Attachment)));
            HostedViewerUri = AddColumn(nameof(HostedViewerUri), database.BuildColumn<Uri>(nameof(Result), nameof(HostedViewerUri), default));
            WorkItemUris = AddColumn(nameof(WorkItemUris), database.BuildColumn<IList<Uri>>(nameof(Result), nameof(WorkItemUris), default));
            Provenance = AddColumn(nameof(Provenance), new RefColumn(nameof(SarifLogDatabase.ResultProvenance)));
            Fixes = AddColumn(nameof(Fixes), new RefListColumn(nameof(SarifLogDatabase.Fix)));
            Taxa = AddColumn(nameof(Taxa), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            WebRequest = AddColumn(nameof(WebRequest), new RefColumn(nameof(SarifLogDatabase.WebRequest)));
            WebResponse = AddColumn(nameof(WebResponse), new RefColumn(nameof(SarifLogDatabase.WebResponse)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Result), nameof(Properties), default));
        }

        public override Result Get(int index)
        {
            return (index == -1 ? null : new Result(this, index));
        }
    }
}
