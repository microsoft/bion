// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Artifact'
    /// </summary>
    internal partial class ArtifactTable : Table<Artifact>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Description;
        internal RefColumn Location;
        internal IColumn<int> ParentIndex;
        internal IColumn<int> Offset;
        internal IColumn<int> Length;
        internal IColumn<int> Roles;
        internal IColumn<String> MimeType;
        internal RefColumn Contents;
        internal IColumn<String> Encoding;
        internal IColumn<String> SourceLanguage;
        internal IColumn<IDictionary<String, String>> Hashes;
        internal IColumn<DateTime> LastModifiedTimeUtc;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ArtifactTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            ParentIndex = AddColumn(nameof(ParentIndex), database.BuildColumn<int>(nameof(Artifact), nameof(ParentIndex), -1));
            Offset = AddColumn(nameof(Offset), database.BuildColumn<int>(nameof(Artifact), nameof(Offset), default));
            Length = AddColumn(nameof(Length), database.BuildColumn<int>(nameof(Artifact), nameof(Length), -1));
            Roles = AddColumn(nameof(Roles), database.BuildColumn<int>(nameof(Artifact), nameof(Roles), (int)default(ArtifactRoles)));
            MimeType = AddColumn(nameof(MimeType), database.BuildColumn<String>(nameof(Artifact), nameof(MimeType), default));
            Contents = AddColumn(nameof(Contents), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            Encoding = AddColumn(nameof(Encoding), database.BuildColumn<String>(nameof(Artifact), nameof(Encoding), default));
            SourceLanguage = AddColumn(nameof(SourceLanguage), database.BuildColumn<String>(nameof(Artifact), nameof(SourceLanguage), default));
            Hashes = AddColumn(nameof(Hashes), database.BuildColumn<IDictionary<String, String>>(nameof(Artifact), nameof(Hashes), default));
            LastModifiedTimeUtc = AddColumn(nameof(LastModifiedTimeUtc), database.BuildColumn<DateTime>(nameof(Artifact), nameof(LastModifiedTimeUtc), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Artifact), nameof(Properties), default));
        }

        public override Artifact Get(int index)
        {
            return (index == -1 ? null : new Artifact(this, index));
        }
    }
}
