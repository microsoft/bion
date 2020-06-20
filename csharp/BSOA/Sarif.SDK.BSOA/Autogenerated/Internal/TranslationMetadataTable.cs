// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'TranslationMetadata'
    /// </summary>
    internal partial class TranslationMetadataTable : Table<TranslationMetadata>
    {
        internal SarifLogDatabase Database;

        internal IColumn<String> Name;
        internal IColumn<String> FullName;
        internal RefColumn ShortDescription;
        internal RefColumn FullDescription;
        internal IColumn<Uri> DownloadUri;
        internal IColumn<Uri> InformationUri;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal TranslationMetadataTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), database.BuildColumn<String>(nameof(TranslationMetadata), nameof(Name), default));
            FullName = AddColumn(nameof(FullName), database.BuildColumn<String>(nameof(TranslationMetadata), nameof(FullName), default));
            ShortDescription = AddColumn(nameof(ShortDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullDescription = AddColumn(nameof(FullDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            DownloadUri = AddColumn(nameof(DownloadUri), database.BuildColumn<Uri>(nameof(TranslationMetadata), nameof(DownloadUri), default));
            InformationUri = AddColumn(nameof(InformationUri), database.BuildColumn<Uri>(nameof(TranslationMetadata), nameof(InformationUri), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(TranslationMetadata), nameof(Properties), default));
        }

        public override TranslationMetadata Get(int index)
        {
            return (index == -1 ? null : new TranslationMetadata(this, index));
        }
    }
}
