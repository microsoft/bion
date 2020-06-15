// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'PhysicalLocation'
    /// </summary>
    internal partial class PhysicalLocationTable : Table<PhysicalLocation>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Address;
        internal RefColumn ArtifactLocation;
        internal RefColumn Region;
        internal RefColumn ContextRegion;
        internal IColumn<IDictionary<string, string>> Properties;

        internal PhysicalLocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Address = AddColumn(nameof(Address), new RefColumn(nameof(SarifLogDatabase.Address)));
            ArtifactLocation = AddColumn(nameof(ArtifactLocation), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Region = AddColumn(nameof(Region), new RefColumn(nameof(SarifLogDatabase.Region)));
            ContextRegion = AddColumn(nameof(ContextRegion), new RefColumn(nameof(SarifLogDatabase.Region)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override PhysicalLocation Get(int index)
        {
            return (index == -1 ? null : new PhysicalLocation(this, index));
        }
    }
}
