using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Artifact' entity.
    /// </summary>
    public partial class ArtifactTable : Table<Artifact>
    {
        internal SarifLogBsoa Database;

        internal RefColumn Description;
        internal RefColumn Location;
        internal IColumn<int> ParentIndex;
        internal IColumn<int> Offset;
        internal IColumn<int> Length;
        internal IColumn<string> MimeType;
        internal RefColumn Contents;
        internal IColumn<string> Encoding;
        internal IColumn<string> SourceLanguage;
        internal IColumn<DateTime> LastModifiedTimeUtc;

        public ArtifactTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogBsoa.Message)));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogBsoa.ArtifactLocation)));
            ParentIndex = AddColumn(nameof(ParentIndex), ColumnFactory.Build<int>(-1));
            Offset = AddColumn(nameof(Offset), ColumnFactory.Build<int>(0));
            Length = AddColumn(nameof(Length), ColumnFactory.Build<int>(-1));
            MimeType = AddColumn(nameof(MimeType), ColumnFactory.Build<string>(null));
            Contents = AddColumn(nameof(Contents), new RefColumn(nameof(SarifLogBsoa.ArtifactContent)));
            Encoding = AddColumn(nameof(Encoding), ColumnFactory.Build<string>(null));
            SourceLanguage = AddColumn(nameof(SourceLanguage), ColumnFactory.Build<string>(null));
            LastModifiedTimeUtc = AddColumn(nameof(LastModifiedTimeUtc), ColumnFactory.Build<DateTime>(DateTime.MinValue));
        }

        public override Artifact Get(int index)
        {
            return (index == -1 ? null : new Artifact(this, index));
        }
    }
}
