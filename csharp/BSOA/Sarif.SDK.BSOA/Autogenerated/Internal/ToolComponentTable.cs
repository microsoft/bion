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

            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<String>(default));
            Name = AddColumn(nameof(Name), ColumnFactory.Build<String>(default));
            Organization = AddColumn(nameof(Organization), ColumnFactory.Build<String>(default));
            Product = AddColumn(nameof(Product), ColumnFactory.Build<String>(default));
            ProductSuite = AddColumn(nameof(ProductSuite), ColumnFactory.Build<String>(default));
            ShortDescription = AddColumn(nameof(ShortDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullDescription = AddColumn(nameof(FullDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullName = AddColumn(nameof(FullName), ColumnFactory.Build<String>(default));
            Version = AddColumn(nameof(Version), ColumnFactory.Build<String>(default));
            SemanticVersion = AddColumn(nameof(SemanticVersion), ColumnFactory.Build<String>(default));
            DottedQuadFileVersion = AddColumn(nameof(DottedQuadFileVersion), ColumnFactory.Build<String>(default));
            ReleaseDateUtc = AddColumn(nameof(ReleaseDateUtc), ColumnFactory.Build<String>(default));
            DownloadUri = AddColumn(nameof(DownloadUri), ColumnFactory.Build<Uri>(default));
            InformationUri = AddColumn(nameof(InformationUri), ColumnFactory.Build<Uri>(default));
            GlobalMessageStrings = AddColumn(nameof(GlobalMessageStrings), new DictionaryColumn<String, MultiformatMessageString>(new DistinctColumn<string>(new StringColumn()), new MultiformatMessageStringColumn(this.Database)));
            Notifications = AddColumn(nameof(Notifications), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptor)));
            Rules = AddColumn(nameof(Rules), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptor)));
            Taxa = AddColumn(nameof(Taxa), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptor)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Language = AddColumn(nameof(Language), ColumnFactory.Build<String>("en-US"));
            Contents = AddColumn(nameof(Contents), ColumnFactory.Build<int>((int)default(ToolComponentContents)));
            IsComprehensive = AddColumn(nameof(IsComprehensive), ColumnFactory.Build<bool>(false));
            LocalizedDataSemanticVersion = AddColumn(nameof(LocalizedDataSemanticVersion), ColumnFactory.Build<String>(default));
            MinimumRequiredLocalizedDataSemanticVersion = AddColumn(nameof(MinimumRequiredLocalizedDataSemanticVersion), ColumnFactory.Build<String>(default));
            AssociatedComponent = AddColumn(nameof(AssociatedComponent), new RefColumn(nameof(SarifLogDatabase.ToolComponentReference)));
            TranslationMetadata = AddColumn(nameof(TranslationMetadata), new RefColumn(nameof(SarifLogDatabase.TranslationMetadata)));
            SupportedTaxonomies = AddColumn(nameof(SupportedTaxonomies), new RefListColumn(nameof(SarifLogDatabase.ToolComponentReference)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override ToolComponent Get(int index)
        {
            return (index == -1 ? null : new ToolComponent(this, index));
        }
    }
}
