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
        internal IColumn<string> RevisionId;
        internal IColumn<string> Branch;
        internal IColumn<string> RevisionTag;
        internal IColumn<DateTime> AsOfTimeUtc;
        internal RefColumn MappedTo;
        internal IColumn<IDictionary<string, string>> Properties;

        internal VersionControlDetailsTable(SarifLogDatabase database) : base()
        {
            Database = database;

            RepositoryUri = AddColumn(nameof(RepositoryUri), ColumnFactory.Build<Uri>());
            RevisionId = AddColumn(nameof(RevisionId), ColumnFactory.Build<string>());
            Branch = AddColumn(nameof(Branch), ColumnFactory.Build<string>());
            RevisionTag = AddColumn(nameof(RevisionTag), ColumnFactory.Build<string>());
            AsOfTimeUtc = AddColumn(nameof(AsOfTimeUtc), ColumnFactory.Build<DateTime>());
            MappedTo = AddColumn(nameof(MappedTo), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override VersionControlDetails Get(int index)
        {
            return (index == -1 ? null : new VersionControlDetails(this, index));
        }
    }
}
