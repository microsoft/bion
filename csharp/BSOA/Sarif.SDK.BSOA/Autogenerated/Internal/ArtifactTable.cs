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
        internal IColumn<string> MimeType;
        internal RefColumn Contents;
        internal IColumn<string> Encoding;
        internal IColumn<string> SourceLanguage;
        internal IColumn<IDictionary<string, string>> Hashes;
        internal IColumn<DateTime> LastModifiedTimeUtc;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ArtifactTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            ParentIndex = AddColumn(nameof(ParentIndex), ColumnFactory.Build<int>(-1));
            Offset = AddColumn(nameof(Offset), ColumnFactory.Build<int>(default));
            Length = AddColumn(nameof(Length), ColumnFactory.Build<int>(-1));
            Roles = AddColumn(nameof(Roles), ColumnFactory.Build<int>((int)default(ArtifactRoles)));
            MimeType = AddColumn(nameof(MimeType), ColumnFactory.Build<string>(default));
            Contents = AddColumn(nameof(Contents), new RefColumn(nameof(SarifLogDatabase.ArtifactContent)));
            Encoding = AddColumn(nameof(Encoding), ColumnFactory.Build<string>(default));
            SourceLanguage = AddColumn(nameof(SourceLanguage), ColumnFactory.Build<string>(default));
            Hashes = AddColumn(nameof(Hashes), ColumnFactory.Build<IDictionary<string, string>>(default));
            LastModifiedTimeUtc = AddColumn(nameof(LastModifiedTimeUtc), ColumnFactory.Build<DateTime>(default));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override Artifact Get(int index)
        {
            return (index == -1 ? null : new Artifact(this, index));
        }
    }
}
