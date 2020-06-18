// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Fix'
    /// </summary>
    internal partial class FixTable : Table<Fix>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Description;
        internal RefListColumn ArtifactChanges;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal FixTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            ArtifactChanges = AddColumn(nameof(ArtifactChanges), new RefListColumn(nameof(SarifLogDatabase.ArtifactChange)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, SerializedPropertyInfo>>(default(IDictionary<string, SerializedPropertyInfo>)));
        }

        public override Fix Get(int index)
        {
            return (index == -1 ? null : new Fix(this, index));
        }
    }
}
