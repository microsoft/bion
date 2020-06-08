using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ArtifactLocation'
    /// </summary>
    internal partial class ArtifactLocationTable : Table<ArtifactLocation>
    {
        internal SarifLogDatabase Database;

        internal IColumn<Uri> Uri;
        internal IColumn<string> UriBaseId;
        internal IColumn<int> Index;
        internal RefColumn Description;

        internal ArtifactLocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Uri = AddColumn(nameof(Uri), ColumnFactory.Build<Uri>(null));
            UriBaseId = AddColumn(nameof(UriBaseId), ColumnFactory.Build<string>(null));
            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
        }

        public override ArtifactLocation Get(int index)
        {
            return (index == -1 ? null : new ArtifactLocation(this, index));
        }
    }
}
