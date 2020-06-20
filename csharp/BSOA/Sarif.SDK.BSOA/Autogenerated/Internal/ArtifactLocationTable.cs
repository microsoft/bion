// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ArtifactLocation'
    /// </summary>
    internal partial class ArtifactLocationTable : Table<ArtifactLocation>
    {
        internal SarifLogDatabase Database;

        internal IColumn<Uri> Uri;
        internal IColumn<String> UriBaseId;
        internal IColumn<int> Index;
        internal RefColumn Description;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ArtifactLocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Uri = AddColumn(nameof(Uri), database.BuildColumn<Uri>(nameof(ArtifactLocation), nameof(Uri), default));
            UriBaseId = AddColumn(nameof(UriBaseId), database.BuildColumn<String>(nameof(ArtifactLocation), nameof(UriBaseId), default));
            Index = AddColumn(nameof(Index), database.BuildColumn<int>(nameof(ArtifactLocation), nameof(Index), -1));
            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ArtifactLocation), nameof(Properties), default));
        }

        public override ArtifactLocation Get(int index)
        {
            return (index == -1 ? null : new ArtifactLocation(this, index));
        }
    }
}
