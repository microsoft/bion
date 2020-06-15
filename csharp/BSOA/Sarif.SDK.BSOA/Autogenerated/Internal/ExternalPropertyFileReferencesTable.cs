// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ExternalPropertyFileReferences'
    /// </summary>
    internal partial class ExternalPropertyFileReferencesTable : Table<ExternalPropertyFileReferences>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Conversion;
        internal RefListColumn Graphs;
        internal RefColumn ExternalizedProperties;
        internal RefListColumn Artifacts;
        internal RefListColumn Invocations;
        internal RefListColumn LogicalLocations;
        internal RefListColumn ThreadFlowLocations;
        internal RefListColumn Results;
        internal RefListColumn Taxonomies;
        internal RefListColumn Addresses;
        internal RefColumn Driver;
        internal RefListColumn Extensions;
        internal RefListColumn Policies;
        internal RefListColumn Translations;
        internal RefListColumn WebRequests;
        internal RefListColumn WebResponses;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ExternalPropertyFileReferencesTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Conversion = AddColumn(nameof(Conversion), new RefColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Graphs = AddColumn(nameof(Graphs), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            ExternalizedProperties = AddColumn(nameof(ExternalizedProperties), new RefColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Artifacts = AddColumn(nameof(Artifacts), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Invocations = AddColumn(nameof(Invocations), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            LogicalLocations = AddColumn(nameof(LogicalLocations), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            ThreadFlowLocations = AddColumn(nameof(ThreadFlowLocations), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Results = AddColumn(nameof(Results), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Taxonomies = AddColumn(nameof(Taxonomies), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Addresses = AddColumn(nameof(Addresses), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Driver = AddColumn(nameof(Driver), new RefColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Extensions = AddColumn(nameof(Extensions), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Policies = AddColumn(nameof(Policies), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Translations = AddColumn(nameof(Translations), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            WebRequests = AddColumn(nameof(WebRequests), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            WebResponses = AddColumn(nameof(WebResponses), new RefListColumn(nameof(SarifLogDatabase.ExternalPropertyFileReference)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override ExternalPropertyFileReferences Get(int index)
        {
            return (index == -1 ? null : new ExternalPropertyFileReferences(this, index));
        }
    }
}
