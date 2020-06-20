// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ExternalPropertyFileReference'
    /// </summary>
    internal partial class ExternalPropertyFileReferenceTable : Table<ExternalPropertyFileReference>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Location;
        internal IColumn<String> Guid;
        internal IColumn<int> ItemCount;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ExternalPropertyFileReferenceTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Guid = AddColumn(nameof(Guid), database.BuildColumn<String>(nameof(ExternalPropertyFileReference), nameof(Guid), default));
            ItemCount = AddColumn(nameof(ItemCount), database.BuildColumn<int>(nameof(ExternalPropertyFileReference), nameof(ItemCount), -1));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ExternalPropertyFileReference), nameof(Properties), default));
        }

        public override ExternalPropertyFileReference Get(int index)
        {
            return (index == -1 ? null : new ExternalPropertyFileReference(this, index));
        }
    }
}
