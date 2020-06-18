// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ExternalProperties'
    /// </summary>
    internal partial class ExternalPropertiesTable : Table<ExternalProperties>
    {
        internal SarifLogDatabase Database;

        internal IColumn<Uri> Schema;
        internal IColumn<int> Version;
        internal IColumn<string> Guid;
        internal IColumn<string> RunGuid;
        internal RefColumn Conversion;
        internal RefListColumn Graphs;
        internal RefColumn ExternalizedProperties;
        internal RefListColumn Artifacts;
        internal RefListColumn Invocations;
        internal RefListColumn LogicalLocations;
        internal RefListColumn ThreadFlowLocations;
        internal RefListColumn Results;
        internal RefListColumn Taxonomies;
        internal RefColumn Driver;
        internal RefListColumn Extensions;
        internal RefListColumn Policies;
        internal RefListColumn Translations;
        internal RefListColumn Addresses;
        internal RefListColumn WebRequests;
        internal RefListColumn WebResponses;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ExternalPropertiesTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Schema = AddColumn(nameof(Schema), ColumnFactory.Build<Uri>(default));
            Version = AddColumn(nameof(Version), ColumnFactory.Build<int>((int)default(SarifVersion)));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>(default));
            RunGuid = AddColumn(nameof(RunGuid), ColumnFactory.Build<string>(default));
            Conversion = AddColumn(nameof(Conversion), new RefColumn(nameof(SarifLogDatabase.Conversion)));
            Graphs = AddColumn(nameof(Graphs), new RefListColumn(nameof(SarifLogDatabase.Graph)));
            ExternalizedProperties = AddColumn(nameof(ExternalizedProperties), new RefColumn(nameof(SarifLogDatabase.PropertyBag)));
            Artifacts = AddColumn(nameof(Artifacts), new RefListColumn(nameof(SarifLogDatabase.Artifact)));
            Invocations = AddColumn(nameof(Invocations), new RefListColumn(nameof(SarifLogDatabase.Invocation)));
            LogicalLocations = AddColumn(nameof(LogicalLocations), new RefListColumn(nameof(SarifLogDatabase.LogicalLocation)));
            ThreadFlowLocations = AddColumn(nameof(ThreadFlowLocations), new RefListColumn(nameof(SarifLogDatabase.ThreadFlowLocation)));
            Results = AddColumn(nameof(Results), new RefListColumn(nameof(SarifLogDatabase.Result)));
            Taxonomies = AddColumn(nameof(Taxonomies), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Driver = AddColumn(nameof(Driver), new RefColumn(nameof(SarifLogDatabase.ToolComponent)));
            Extensions = AddColumn(nameof(Extensions), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Policies = AddColumn(nameof(Policies), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Translations = AddColumn(nameof(Translations), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Addresses = AddColumn(nameof(Addresses), new RefListColumn(nameof(SarifLogDatabase.Address)));
            WebRequests = AddColumn(nameof(WebRequests), new RefListColumn(nameof(SarifLogDatabase.WebRequest)));
            WebResponses = AddColumn(nameof(WebResponses), new RefListColumn(nameof(SarifLogDatabase.WebResponse)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override ExternalProperties Get(int index)
        {
            return (index == -1 ? null : new ExternalProperties(this, index));
        }
    }
}
