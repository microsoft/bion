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

        internal IColumn<string> RuleId;
        internal IColumn<int> RuleIndex;
        internal RefColumn Rule;
        internal IColumn<int> Kind;
        internal IColumn<int> Level;
        internal RefColumn Message;
        internal RefColumn AnalysisTarget;
        internal RefListColumn Locations;
        internal IColumn<string> Guid;
        internal IColumn<string> CorrelationGuid;
        internal IColumn<int> OccurrenceCount;
        internal IColumn<IDictionary<string, string>> PartialFingerprints;
        internal IColumn<IDictionary<string, string>> Fingerprints;
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
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ResultTable(SarifLogDatabase database) : base()
        {
            Database = database;

            RuleId = AddColumn(nameof(RuleId), ColumnFactory.Build<string>());
            RuleIndex = AddColumn(nameof(RuleIndex), ColumnFactory.Build<int>(-1));
            Rule = AddColumn(nameof(Rule), new RefColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            Kind = AddColumn(nameof(Kind), ColumnFactory.Build<int>((int)ResultKind.Fail));
            Level = AddColumn(nameof(Level), ColumnFactory.Build<int>((int)FailureLevel.Warning));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            AnalysisTarget = AddColumn(nameof(AnalysisTarget), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.Location)));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>());
            CorrelationGuid = AddColumn(nameof(CorrelationGuid), ColumnFactory.Build<string>());
            OccurrenceCount = AddColumn(nameof(OccurrenceCount), ColumnFactory.Build<int>());
            PartialFingerprints = AddColumn(nameof(PartialFingerprints), ColumnFactory.Build<IDictionary<string, string>>());
            Fingerprints = AddColumn(nameof(Fingerprints), ColumnFactory.Build<IDictionary<string, string>>());
            Stacks = AddColumn(nameof(Stacks), new RefListColumn(nameof(SarifLogDatabase.Stack)));
            CodeFlows = AddColumn(nameof(CodeFlows), new RefListColumn(nameof(SarifLogDatabase.CodeFlow)));
            Graphs = AddColumn(nameof(Graphs), new RefListColumn(nameof(SarifLogDatabase.Graph)));
            GraphTraversals = AddColumn(nameof(GraphTraversals), new RefListColumn(nameof(SarifLogDatabase.GraphTraversal)));
            RelatedLocations = AddColumn(nameof(RelatedLocations), new RefListColumn(nameof(SarifLogDatabase.Location)));
            Suppressions = AddColumn(nameof(Suppressions), new RefListColumn(nameof(SarifLogDatabase.Suppression)));
            BaselineState = AddColumn(nameof(BaselineState), ColumnFactory.Build<int>((int)default(BaselineState)));
            Rank = AddColumn(nameof(Rank), ColumnFactory.Build<double>(-1));
            Attachments = AddColumn(nameof(Attachments), new RefListColumn(nameof(SarifLogDatabase.Attachment)));
            HostedViewerUri = AddColumn(nameof(HostedViewerUri), ColumnFactory.Build<Uri>());
            WorkItemUris = AddColumn(nameof(WorkItemUris), ColumnFactory.Build<IList<Uri>>());
            Provenance = AddColumn(nameof(Provenance), new RefColumn(nameof(SarifLogDatabase.ResultProvenance)));
            Fixes = AddColumn(nameof(Fixes), new RefListColumn(nameof(SarifLogDatabase.Fix)));
            Taxa = AddColumn(nameof(Taxa), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            WebRequest = AddColumn(nameof(WebRequest), new RefColumn(nameof(SarifLogDatabase.WebRequest)));
            WebResponse = AddColumn(nameof(WebResponse), new RefColumn(nameof(SarifLogDatabase.WebResponse)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override Result Get(int index)
        {
            return (index == -1 ? null : new Result(this, index));
        }
    }
}
