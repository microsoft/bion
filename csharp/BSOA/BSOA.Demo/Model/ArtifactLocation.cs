using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    public readonly struct ArtifactLocation
    {
        internal readonly ArtifactLocationTable _table;
        internal readonly int _index;

        public ArtifactLocation(ArtifactLocationTable table, int index)
        {
            this._table = table;
            this._index = index;
        }

        public ArtifactLocation(ArtifactLocationTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public ArtifactLocation(SarifLogBsoa database) : this(database.ArtifactLocation)
        { }

        public bool IsNull => (_table == null || _index < 0);

        public Uri Uri
        {
            get => _table.Uri[_index];
            set => _table.Uri[_index] = value;
        }

        public string UriBaseId
        {
            get => _table.UriBaseId[_index];
            set => _table.UriBaseId[_index] = value;
        }

        public int Index
        {
            get => _table.Index[_index];
            set => _table.Index[_index] = value;
        }

        public Message Description
        {
            get => _table.Database.Message[_table.Description[_index]];
            set => _table.Description[_index] = value._index;
        }
    }

    public class ArtifactLocationTable : Table<ArtifactLocation>
    {
        internal SarifLogBsoa Database;

        internal UriColumn Uri;
        internal StringColumn UriBaseId;
        internal NumberColumn<int> Index;
        internal RefColumn Description;

        // Properties

        public ArtifactLocationTable(SarifLogBsoa database) : base()
        {
            this.Database = database;

            this.UriBaseId = AddColumn(nameof(UriBaseId), new StringColumn());
            this.Uri = AddColumn(nameof(Uri), new UriColumn());
            this.Index = AddColumn(nameof(Index), new NumberColumn<int>(-1));
            this.Description = AddColumn(nameof(Description), new RefColumn(nameof(database.Message)));
        }

        public override ArtifactLocation this[int index] => new ArtifactLocation(this, index);
    }

}
