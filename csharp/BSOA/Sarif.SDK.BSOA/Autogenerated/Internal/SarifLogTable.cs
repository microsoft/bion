// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'SarifLog'
    /// </summary>
    internal partial class SarifLogTable : Table<SarifLog>
    {
        internal SarifLogDatabase Database;

        internal IColumn<Uri> SchemaUri;
        internal IColumn<int> Version;
        internal RefListColumn Runs;
        internal RefListColumn InlineExternalProperties;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal SarifLogTable(SarifLogDatabase database) : base()
        {
            Database = database;

            SchemaUri = AddColumn(nameof(SchemaUri), database.BuildColumn<Uri>(nameof(SarifLog), nameof(SchemaUri), default));
            Version = AddColumn(nameof(Version), database.BuildColumn<int>(nameof(SarifLog), nameof(Version), (int)default(SarifVersion)));
            Runs = AddColumn(nameof(Runs), new RefListColumn(nameof(SarifLogDatabase.Run)));
            InlineExternalProperties = AddColumn(nameof(InlineExternalProperties), new RefListColumn(nameof(SarifLogDatabase.ExternalProperties)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(SarifLog), nameof(Properties), default));
        }

        public override SarifLog Get(int index)
        {
            return (index == -1 ? null : new SarifLog(this, index));
        }
    }
}
