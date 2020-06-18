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
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal SarifLogTable(SarifLogDatabase database) : base()
        {
            Database = database;

            SchemaUri = AddColumn(nameof(SchemaUri), ColumnFactory.Build<Uri>(default(Uri)));
            Version = AddColumn(nameof(Version), ColumnFactory.Build<int>((int)default(SarifVersion)));
            Runs = AddColumn(nameof(Runs), new RefListColumn(nameof(SarifLogDatabase.Run)));
            InlineExternalProperties = AddColumn(nameof(InlineExternalProperties), new RefListColumn(nameof(SarifLogDatabase.ExternalProperties)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, SerializedPropertyInfo>>(default(IDictionary<string, SerializedPropertyInfo>)));
        }

        public override SarifLog Get(int index)
        {
            return (index == -1 ? null : new SarifLog(this, index));
        }
    }
}
