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

            Name = AddColumn(nameof(Name), ColumnFactory.Build<String>(default));
            FullName = AddColumn(nameof(FullName), ColumnFactory.Build<String>(default));
            ShortDescription = AddColumn(nameof(ShortDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            FullDescription = AddColumn(nameof(FullDescription), new RefColumn(nameof(SarifLogDatabase.MultiformatMessageString)));
            DownloadUri = AddColumn(nameof(DownloadUri), ColumnFactory.Build<Uri>(default));
            InformationUri = AddColumn(nameof(InformationUri), ColumnFactory.Build<Uri>(default));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override TranslationMetadata Get(int index)
        {
            return (index == -1 ? null : new TranslationMetadata(this, index));
        }
    }
}
