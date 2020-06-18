// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ToolComponent'
    /// </summary>
    internal partial class ToolComponentTable : Table<ToolComponent>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Guid;
        internal IColumn<string> Name;
        internal IColumn<string> Organization;
        internal IColumn<string> Product;
        internal IColumn<string> ProductSuite;
        internal RefColumn ShortDescription;
        internal RefColumn FullDescription;
        internal IColumn<string> FullName;
        internal IColumn<string> Version;
        internal IColumn<string> SemanticVersion;
        internal IColumn<string> DottedQuadFileVersion;
        internal IColumn<string> ReleaseDateUtc;
        internal IColumn<Uri> DownloadUri;
        internal IColumn<Uri> InformationUri;
        internal IColumn<IDictionary<string, MultiformatMessageString>> GlobalMessageStrings;
        internal RefListColumn Notifications;
        internal RefListColumn Rules;
        internal RefListColumn Taxa;
        internal RefListColumn Locations;
        internal IColumn<string> Language;
        internal IColumn<int> Contents;
        internal IColumn<bool> IsComprehensive;
        internal IColumn<string> LocalizedDataSemanticVersion;
        internal IColumn<string> MinimumRequiredLocalizedDataSemanticVersion;
        internal RefColumn AssociatedComponent;
        internal RefColumn TranslationMetadata;
        internal RefListColumn SupportedTaxonomies;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ToolComponentTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>(default(string)));
            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>(default(string)));
            Organization = AddColumn(nameof(Organization), ColumnFactory.Build<string>(default(string)));
            Product = AddColumn(nameof(Product), ColumnFactory.Build<string>(default(string)));
            ProductSuite = AddColumn(nameof(ProductSuite), ColumnFactory.Build<string>(default(string)));
            ShortDescription = AddColumn(nameof(ShortDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullDescription = AddColumn(nameof(FullDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullName = AddColumn(nameof(FullName), ColumnFactory.Build<string>(default(string)));
            Version = AddColumn(nameof(Version), ColumnFactory.Build<string>(default(string)));
            SemanticVersion = AddColumn(nameof(SemanticVersion), ColumnFactory.Build<string>(default(string)));
            DottedQuadFileVersion = AddColumn(nameof(DottedQuadFileVersion), ColumnFactory.Build<string>(default(string)));
            ReleaseDateUtc = AddColumn(nameof(ReleaseDateUtc), ColumnFactory.Build<string>(default(string)));
            DownloadUri = AddColumn(nameof(DownloadUri), ColumnFactory.Build<Uri>(default(Uri)));
            InformationUri = AddColumn(nameof(InformationUri), ColumnFactory.Build<Uri>(default(Uri)));
            GlobalMessageStrings = AddColumn(nameof(GlobalMessageStrings), ColumnFactory.Build<IDictionary<string, MultiformatMessageString>>(default(IDictionary<string, MultiformatMessageString>)));
            Notifications = AddColumn(nameof(Notifications), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptor)));
            Rules = AddColumn(nameof(Rules), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptor)));
            Taxa = AddColumn(nameof(Taxa), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptor)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Language = AddColumn(nameof(Language), ColumnFactory.Build<string>("en-US"));
            Contents = AddColumn(nameof(Contents), ColumnFactory.Build<int>((int)default(ToolComponentContents)));
            IsComprehensive = AddColumn(nameof(IsComprehensive), ColumnFactory.Build<bool>(false));
            LocalizedDataSemanticVersion = AddColumn(nameof(LocalizedDataSemanticVersion), ColumnFactory.Build<string>(default(string)));
            MinimumRequiredLocalizedDataSemanticVersion = AddColumn(nameof(MinimumRequiredLocalizedDataSemanticVersion), ColumnFactory.Build<string>(default(string)));
            AssociatedComponent = AddColumn(nameof(AssociatedComponent), new RefColumn(nameof(SarifLogDatabase.ToolComponentReference)));
            TranslationMetadata = AddColumn(nameof(TranslationMetadata), new RefColumn(nameof(SarifLogDatabase.TranslationMetadata)));
            SupportedTaxonomies = AddColumn(nameof(SupportedTaxonomies), new RefListColumn(nameof(SarifLogDatabase.ToolComponentReference)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, SerializedPropertyInfo>>(default(IDictionary<string, SerializedPropertyInfo>)));
        }

        public override ToolComponent Get(int index)
        {
            return (index == -1 ? null : new ToolComponent(this, index));
        }
    }
}
