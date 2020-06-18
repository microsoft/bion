// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'SpecialLocations'
    /// </summary>
    internal partial class SpecialLocationsTable : Table<SpecialLocations>
    {
        internal SarifLogDatabase Database;

        internal RefColumn DisplayBase;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal SpecialLocationsTable(SarifLogDatabase database) : base()
        {
            Database = database;

            DisplayBase = AddColumn(nameof(DisplayBase), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, SerializedPropertyInfo>>(default(IDictionary<string, SerializedPropertyInfo>)));
        }

        public override SpecialLocations Get(int index)
        {
            return (index == -1 ? null : new SpecialLocations(this, index));
        }
    }
}
