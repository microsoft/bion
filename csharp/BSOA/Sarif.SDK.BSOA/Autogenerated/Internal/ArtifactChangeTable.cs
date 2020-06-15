// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ArtifactChange'
    /// </summary>
    internal partial class ArtifactChangeTable : Table<ArtifactChange>
    {
        internal SarifLogDatabase Database;

        internal RefColumn ArtifactLocation;
        internal RefListColumn Replacements;
        internal IColumn<IDictionary<string, string>> Properties;

        internal ArtifactChangeTable(SarifLogDatabase database) : base()
        {
            Database = database;

            ArtifactLocation = AddColumn(nameof(ArtifactLocation), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Replacements = AddColumn(nameof(Replacements), new RefListColumn(nameof(SarifLogDatabase.Replacement)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override ArtifactChange Get(int index)
        {
            return (index == -1 ? null : new ArtifactChange(this, index));
        }
    }
}
