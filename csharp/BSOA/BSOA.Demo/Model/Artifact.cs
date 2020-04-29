using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    public struct Artifact
    {
        internal ArtifactTable _table;
        internal int _index;

        public Artifact(ArtifactTable table, int index)
        {
            _table = table;
            _index = index;
        }

        public Artifact(ArtifactTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Artifact(SarifLogBsoa database) : this(database.Artifact)
        { }

        public bool IsNull => (_table == null || _index < 0);

        public Message Description
        {
            get => _table.Database.Message[_table.Description[_index]];
            set => _table.Description[_index] = value._index;
        }

        public ArtifactLocation Location
        {
            get => _table.Database.ArtifactLocation[_table.Location[_index]];
            set => _table.Location[_index] = value._index;
        }

        public int ParentIndex
        {
            get => _table.ParentIndex[_index];
            set => _table.ParentIndex[_index] = value;
        }

        public int Offset
        {
            get => _table.Offset[_index];
            set => _table.Offset[_index] = value;
        }

        public int Length
        {
            get => _table.Length[_index];
            set => _table.Length[_index] = value;
        }

        public string MimeType
        {
            get => _table.MimeType[_index];
            set => _table.MimeType[_index] = value;
        }

        public ArtifactContent Contents
        {
            get => _table.Database.ArtifactContent[_table.Contents[_index]];
            set => _table.Contents[_index] = value._index;
        }

        public string Encoding
        {
            get => _table.Encoding[_index];
            set => _table.Encoding[_index] = value;
        }

        public string SourceLanguage
        {
            get => _table.SourceLanguage[_index];
            set => _table.SourceLanguage[_index] = value;
        }

        public DateTime LastModifiedTimeUtc
        {
            get => _table.LastModifiedTimeUtc[_index];
            set => _table.LastModifiedTimeUtc[_index] = value;
        }
    }

    public class ArtifactTable : Table<Artifact>
    {
        internal SarifLogBsoa Database;
        
        internal RefColumn Description;
        internal RefColumn Location;
        internal NumberColumn<int> ParentIndex;
        internal NumberColumn<int> Offset;
        internal NumberColumn<int> Length;
        internal StringColumn MimeType;
        internal RefColumn Contents;
        internal StringColumn Encoding;
        internal StringColumn SourceLanguage;
        internal DateTimeColumn LastModifiedTimeUtc;

        // Missing: Roles, Hashes, Properties

        public ArtifactTable(SarifLogBsoa database) : base()
        {
            this.Database = database;

            this.Description = AddColumn(nameof(Description), new RefColumn(nameof(database.Message)));
            this.Location = AddColumn(nameof(Location), new RefColumn(nameof(database.ArtifactLocation)));
            this.ParentIndex = AddColumn(nameof(ParentIndex), new NumberColumn<int>(-1));
            this.Offset = AddColumn(nameof(Offset), new NumberColumn<int>(0));
            this.Length = AddColumn(nameof(Length), new NumberColumn<int>(-1));
            this.MimeType = AddColumn(nameof(MimeType), new StringColumn());
            this.Contents = AddColumn(nameof(Contents), new RefColumn(nameof(database.ArtifactContent)));
            this.Encoding = AddColumn(nameof(Encoding), new StringColumn());
            this.SourceLanguage = AddColumn(nameof(SourceLanguage), new StringColumn());
            this.LastModifiedTimeUtc = AddColumn(nameof(LastModifiedTimeUtc), new DateTimeColumn(DateTime.MinValue));
        }

        public override Artifact this[int index] => new Artifact(this, index);
    }

}
