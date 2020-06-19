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
        internal IColumn<string> Language;
        internal RefListColumn VersionControlProvenance;
        internal IColumn<IDictionary<string, ArtifactLocation>> OriginalUriBaseIds;
        internal RefListColumn Artifacts;
        internal RefListColumn LogicalLocations;
        internal RefListColumn Graphs;
        internal RefListColumn Results;
        internal RefColumn AutomationDetails;
        internal RefListColumn RunAggregates;
        internal IColumn<string> BaselineGuid;
        internal IColumn<IList<string>> RedactionTokens;
        internal IColumn<string> DefaultEncoding;
        internal IColumn<string> DefaultSourceLanguage;
        internal IColumn<IList<string>> NewlineSequences;
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
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal RunTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Tool = AddColumn(nameof(Tool), new RefColumn(nameof(SarifLogDatabase.Tool)));
            Invocations = AddColumn(nameof(Invocations), new RefListColumn(nameof(SarifLogDatabase.Invocation)));
            Conversion = AddColumn(nameof(Conversion), new RefColumn(nameof(SarifLogDatabase.Conversion)));
            Language = AddColumn(nameof(Language), ColumnFactory.Build<string>("en-US"));
            VersionControlProvenance = AddColumn(nameof(VersionControlProvenance), new RefListColumn(nameof(SarifLogDatabase.VersionControlDetails)));
            OriginalUriBaseIds = AddColumn(nameof(OriginalUriBaseIds), new DictionaryColumn<string, ArtifactLocation>(new DistinctColumn<string>(new StringColumn()), new ArtifactLocationColumn(this.Database)));
            Artifacts = AddColumn(nameof(Artifacts), new RefListColumn(nameof(SarifLogDatabase.Artifact)));
            LogicalLocations = AddColumn(nameof(LogicalLocations), new RefListColumn(nameof(SarifLogDatabase.LogicalLocation)));
            Graphs = AddColumn(nameof(Graphs), new RefListColumn(nameof(SarifLogDatabase.Graph)));
            Results = AddColumn(nameof(Results), new RefListColumn(nameof(SarifLogDatabase.Result)));
            AutomationDetails = AddColumn(nameof(AutomationDetails), new RefColumn(nameof(SarifLogDatabase.RunAutomationDetails)));
            RunAggregates = AddColumn(nameof(RunAggregates), new RefListColumn(nameof(SarifLogDatabase.RunAutomationDetails)));
            BaselineGuid = AddColumn(nameof(BaselineGuid), ColumnFactory.Build<string>(default));
            RedactionTokens = AddColumn(nameof(RedactionTokens), ColumnFactory.Build<IList<string>>(default));
            DefaultEncoding = AddColumn(nameof(DefaultEncoding), ColumnFactory.Build<string>(default));
            DefaultSourceLanguage = AddColumn(nameof(DefaultSourceLanguage), ColumnFactory.Build<string>(default));
            NewlineSequences = AddColumn(nameof(NewlineSequences), ColumnFactory.Build<IList<string>>(default));
            ColumnKind = AddColumn(nameof(ColumnKind), ColumnFactory.Build<int>((int)default(ColumnKind)));
            ExternalPropertyFileReferences = AddColumn(nameof(ExternalPropertyFileReferences), new RefColumn(nameof(SarifLogDatabase.ExternalPropertyFileReferences)));
            ThreadFlowLocations = AddColumn(nameof(ThreadFlowLocations), new RefListColumn(nameof(SarifLogDatabase.ThreadFlowLocation)));
            Taxonomies = AddColumn(nameof(Taxonomies), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Addresses = AddColumn(nameof(Addresses), new RefListColumn(nameof(SarifLogDatabase.Address)));
            Translations = AddColumn(nameof(Translations), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Policies = AddColumn(nameof(Policies), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            WebRequests = AddColumn(nameof(WebRequests), new RefListColumn(nameof(SarifLogDatabase.WebRequest)));
            WebResponses = AddColumn(nameof(WebResponses), new RefListColumn(nameof(SarifLogDatabase.WebResponse)));
            SpecialLocations = AddColumn(nameof(SpecialLocations), new RefColumn(nameof(SarifLogDatabase.SpecialLocations)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Run Get(int index)
        {
            return (index == -1 ? null : new Run(this, index));
        }
    }
}
