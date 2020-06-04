using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'ArtifactLocation' entity.
    /// </summary>
    public partial class ArtifactLocationTable : Table<ArtifactLocation>
    {
        internal SarifLogBsoa Database;

        internal IColumn<Uri> Uri;
        internal IColumn<string> UriBaseId;
        internal IColumn<int> Index;
        internal RefColumn Description;

        public ArtifactLocationTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Uri = AddColumn(nameof(Uri), ColumnFactory.Build<Uri>(null));
            UriBaseId = AddColumn(nameof(UriBaseId), ColumnFactory.Build<string>(null));
            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogBsoa.Message)));
        }

        public override ArtifactLocation Get(int index)
        {
            return (index == -1 ? null : new ArtifactLocation(this, index));
        }
    }
}
