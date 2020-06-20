// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'VersionControlDetails'
    /// </summary>
    internal partial class VersionControlDetailsTable : Table<VersionControlDetails>
    {
        internal SarifLogDatabase Database;

        internal IColumn<Uri> RepositoryUri;
        internal IColumn<String> RevisionId;
        internal IColumn<String> Branch;
        internal IColumn<String> RevisionTag;
        internal IColumn<DateTime> AsOfTimeUtc;
        internal RefColumn MappedTo;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal VersionControlDetailsTable(SarifLogDatabase database) : base()
        {
            Database = database;

            RepositoryUri = AddColumn(nameof(RepositoryUri), database.BuildColumn<Uri>(nameof(VersionControlDetails), nameof(RepositoryUri), default));
            RevisionId = AddColumn(nameof(RevisionId), database.BuildColumn<String>(nameof(VersionControlDetails), nameof(RevisionId), default));
            Branch = AddColumn(nameof(Branch), database.BuildColumn<String>(nameof(VersionControlDetails), nameof(Branch), default));
            RevisionTag = AddColumn(nameof(RevisionTag), database.BuildColumn<String>(nameof(VersionControlDetails), nameof(RevisionTag), default));
            AsOfTimeUtc = AddColumn(nameof(AsOfTimeUtc), database.BuildColumn<DateTime>(nameof(VersionControlDetails), nameof(AsOfTimeUtc), default));
            MappedTo = AddColumn(nameof(MappedTo), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(VersionControlDetails), nameof(Properties), default));
        }

        public override VersionControlDetails Get(int index)
        {
            return (index == -1 ? null : new VersionControlDetails(this, index));
        }
    }
}
