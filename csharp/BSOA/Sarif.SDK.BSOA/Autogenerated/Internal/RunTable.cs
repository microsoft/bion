// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Run'
    /// </summary>
    internal partial class RunTable : Table<Run>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Tool;
        internal RefListColumn Invocations;
        internal RefColumn Conversion;
        internal IColumn<String> Language;
        internal RefListColumn VersionControlProvenance;
        internal IColumn<IDictionary<String, ArtifactLocation>> OriginalUriBaseIds;
        internal RefListColumn Artifacts;
        internal RefListColumn LogicalLocations;
        internal RefListColumn Graphs;
        internal RefListColumn Results;
        internal RefColumn AutomationDetails;
        internal RefListColumn RunAggregates;
        internal IColumn<String> BaselineGuid;
        internal IColumn<IList<String>> RedactionTokens;
        internal IColumn<String> DefaultEncoding;
        internal IColumn<String> DefaultSourceLanguage;
        internal IColumn<IList<String>> NewlineSequences;
        internal IColumn<int> ColumnKind;
        internal RefColumn ExternalPropertyFileReferences;
        internal RefListColumn ThreadFlowLocations;
        internal RefListColumn Taxonomies;
        internal RefListColumn Addresses;
        internal RefListColumn Translations;
        internal RefListColumn Policies;
        internal RefListColumn WebRequests;
        internal RefListColumn WebResponses;
        internal RefColumn SpecialLocations;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal RunTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Tool = AddColumn(nameof(Tool), new RefColumn(nameof(SarifLogDatabase.Tool)));
            Invocations = AddColumn(nameof(Invocations), new RefListColumn(nameof(SarifLogDatabase.Invocation)));
            Conversion = AddColumn(nameof(Conversion), new RefColumn(nameof(SarifLogDatabase.Conversion)));
            Language = AddColumn(nameof(Language), database.BuildColumn<String>(nameof(Run), nameof(Language), "en-US"));
            VersionControlProvenance = AddColumn(nameof(VersionControlProvenance), new RefListColumn(nameof(SarifLogDatabase.VersionControlDetails)));
            OriginalUriBaseIds = AddColumn(nameof(OriginalUriBaseIds), database.BuildColumn<IDictionary<String, ArtifactLocation>>(nameof(Run), nameof(OriginalUriBaseIds), default));
            Artifacts = AddColumn(nameof(Artifacts), new RefListColumn(nameof(SarifLogDatabase.Artifact)));
            LogicalLocations = AddColumn(nameof(LogicalLocations), new RefListColumn(nameof(SarifLogDatabase.LogicalLocation)));
            Graphs = AddColumn(nameof(Graphs), new RefListColumn(nameof(SarifLogDatabase.Graph)));
            Results = AddColumn(nameof(Results), new RefListColumn(nameof(SarifLogDatabase.Result)));
            AutomationDetails = AddColumn(nameof(AutomationDetails), new RefColumn(nameof(SarifLogDatabase.RunAutomationDetails)));
            RunAggregates = AddColumn(nameof(RunAggregates), new RefListColumn(nameof(SarifLogDatabase.RunAutomationDetails)));
            BaselineGuid = AddColumn(nameof(BaselineGuid), database.BuildColumn<String>(nameof(Run), nameof(BaselineGuid), default));
            RedactionTokens = AddColumn(nameof(RedactionTokens), database.BuildColumn<IList<String>>(nameof(Run), nameof(RedactionTokens), default));
            DefaultEncoding = AddColumn(nameof(DefaultEncoding), database.BuildColumn<String>(nameof(Run), nameof(DefaultEncoding), default));
            DefaultSourceLanguage = AddColumn(nameof(DefaultSourceLanguage), database.BuildColumn<String>(nameof(Run), nameof(DefaultSourceLanguage), default));
            NewlineSequences = AddColumn(nameof(NewlineSequences), database.BuildColumn<IList<String>>(nameof(Run), nameof(NewlineSequences), default));
            ColumnKind = AddColumn(nameof(ColumnKind), database.BuildColumn<int>(nameof(Run), nameof(ColumnKind), (int)default(ColumnKind)));
            ExternalPropertyFileReferences = AddColumn(nameof(ExternalPropertyFileReferences), new RefColumn(nameof(SarifLogDatabase.ExternalPropertyFileReferences)));
            ThreadFlowLocations = AddColumn(nameof(ThreadFlowLocations), new RefListColumn(nameof(SarifLogDatabase.ThreadFlowLocation)));
            Taxonomies = AddColumn(nameof(Taxonomies), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Addresses = AddColumn(nameof(Addresses), new RefListColumn(nameof(SarifLogDatabase.Address)));
            Translations = AddColumn(nameof(Translations), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Policies = AddColumn(nameof(Policies), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            WebRequests = AddColumn(nameof(WebRequests), new RefListColumn(nameof(SarifLogDatabase.WebRequest)));
            WebResponses = AddColumn(nameof(WebResponses), new RefListColumn(nameof(SarifLogDatabase.WebResponse)));
            SpecialLocations = AddColumn(nameof(SpecialLocations), new RefColumn(nameof(SarifLogDatabase.SpecialLocations)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Run), nameof(Properties), default));
        }

        public override Run Get(int index)
        {
            return (index == -1 ? null : new Run(this, index));
        }
    }
}
