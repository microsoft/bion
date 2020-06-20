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

        internal IColumn<String> Guid;
        internal IColumn<String> Name;
        internal IColumn<String> Organization;
        internal IColumn<String> Product;
        internal IColumn<String> ProductSuite;
        internal RefColumn ShortDescription;
        internal RefColumn FullDescription;
        internal IColumn<String> FullName;
        internal IColumn<String> Version;
        internal IColumn<String> SemanticVersion;
        internal IColumn<String> DottedQuadFileVersion;
        internal IColumn<String> ReleaseDateUtc;
        internal IColumn<Uri> DownloadUri;
        internal IColumn<Uri> InformationUri;
        internal IColumn<IDictionary<String, MultiformatMessageString>> GlobalMessageStrings;
        internal RefListColumn Notifications;
        internal RefListColumn Rules;
        internal RefListColumn Taxa;
        internal RefListColumn Locations;
        internal IColumn<String> Language;
        internal IColumn<int> Contents;
        internal IColumn<bool> IsComprehensive;
        internal IColumn<String> LocalizedDataSemanticVersion;
        internal IColumn<String> MinimumRequiredLocalizedDataSemanticVersion;
        internal RefColumn AssociatedComponent;
        internal RefColumn TranslationMetadata;
        internal RefListColumn SupportedTaxonomies;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ToolComponentTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Guid = AddColumn(nameof(Guid), database.BuildColumn<String>(nameof(ToolComponent), nameof(Guid), default));
            Name = AddColumn(nameof(Name), database.BuildColumn<String>(nameof(ToolComponent), nameof(Name), default));
            Organization = AddColumn(nameof(Organization), database.BuildColumn<String>(nameof(ToolComponent), nameof(Organization), default));
            Product = AddColumn(nameof(Product), database.BuildColumn<String>(nameof(ToolComponent), nameof(Product), default));
            ProductSuite = AddColumn(nameof(ProductSuite), database.BuildColumn<String>(nameof(ToolComponent), nameof(ProductSuite), default));
            ShortDescription = AddColumn(nameof(ShortDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullDescription = AddColumn(nameof(FullDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullName = AddColumn(nameof(FullName), database.BuildColumn<String>(nameof(ToolComponent), nameof(FullName), default));
            Version = AddColumn(nameof(Version), database.BuildColumn<String>(nameof(ToolComponent), nameof(Version), default));
            SemanticVersion = AddColumn(nameof(SemanticVersion), database.BuildColumn<String>(nameof(ToolComponent), nameof(SemanticVersion), default));
            DottedQuadFileVersion = AddColumn(nameof(DottedQuadFileVersion), database.BuildColumn<String>(nameof(ToolComponent), nameof(DottedQuadFileVersion), default));
            ReleaseDateUtc = AddColumn(nameof(ReleaseDateUtc), database.BuildColumn<String>(nameof(ToolComponent), nameof(ReleaseDateUtc), default));
            DownloadUri = AddColumn(nameof(DownloadUri), database.BuildColumn<Uri>(nameof(ToolComponent), nameof(DownloadUri), default));
            InformationUri = AddColumn(nameof(InformationUri), database.BuildColumn<Uri>(nameof(ToolComponent), nameof(InformationUri), default));
            GlobalMessageStrings = AddColumn(nameof(GlobalMessageStrings), database.BuildColumn<IDictionary<String, MultiformatMessageString>>(nameof(ToolComponent), nameof(GlobalMessageStrings), default));
            Notifications = AddColumn(nameof(Notifications), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptor)));
            Rules = AddColumn(nameof(Rules), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptor)));
            Taxa = AddColumn(nameof(Taxa), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptor)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Language = AddColumn(nameof(Language), database.BuildColumn<String>(nameof(ToolComponent), nameof(Language), "en-US"));
            Contents = AddColumn(nameof(Contents), database.BuildColumn<int>(nameof(ToolComponent), nameof(Contents), (int)default(ToolComponentContents)));
            IsComprehensive = AddColumn(nameof(IsComprehensive), database.BuildColumn<bool>(nameof(ToolComponent), nameof(IsComprehensive), false));
            LocalizedDataSemanticVersion = AddColumn(nameof(LocalizedDataSemanticVersion), database.BuildColumn<String>(nameof(ToolComponent), nameof(LocalizedDataSemanticVersion), default));
            MinimumRequiredLocalizedDataSemanticVersion = AddColumn(nameof(MinimumRequiredLocalizedDataSemanticVersion), database.BuildColumn<String>(nameof(ToolComponent), nameof(MinimumRequiredLocalizedDataSemanticVersion), default));
            AssociatedComponent = AddColumn(nameof(AssociatedComponent), new RefColumn(nameof(SarifLogDatabase.ToolComponentReference)));
            TranslationMetadata = AddColumn(nameof(TranslationMetadata), new RefColumn(nameof(SarifLogDatabase.TranslationMetadata)));
            SupportedTaxonomies = AddColumn(nameof(SupportedTaxonomies), new RefListColumn(nameof(SarifLogDatabase.ToolComponentReference)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ToolComponent), nameof(Properties), default));
        }

        public override ToolComponent Get(int index)
        {
            return (index == -1 ? null : new ToolComponent(this, index));
        }
    }
}
