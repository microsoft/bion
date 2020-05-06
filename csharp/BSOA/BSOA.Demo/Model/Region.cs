using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  SoA Item for 'Region' type.
    /// </summary>
    public readonly struct Region
    {
        internal readonly RegionTable _table;
        internal readonly int _index;

        internal Region(RegionTable table, int index)
        {
            _table = table;
            _index = index;
        }

        public Region(RegionTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Region(SarifLogBsoa database) : this(database.Region)
        { }

        public bool IsNull => (_table == null || _index < 0);

        public int StartLine
        {
            get => _table.StartLine[_index];
            set => _table.StartLine[_index] = value;
        }

        public int StartColumn
        {
            get => _table.StartColumn[_index];
            set => _table.StartColumn[_index] = value;
        }

        public int EndLine
        {
            get => _table.EndLine[_index];
            set => _table.EndLine[_index] = value;
        }

        public int EndColumn
        {
            get => _table.EndColumn[_index];
            set => _table.EndColumn[_index] = value;
        }

        public int ByteOffset
        {
            get => _table.ByteOffset[_index];
            set => _table.ByteOffset[_index] = value;
        }

        public int ByteLength
        {
            get => _table.ByteLength[_index];
            set => _table.ByteLength[_index] = value;
        }

        public int CharOffset
        {
            get => _table.CharOffset[_index];
            set => _table.CharOffset[_index] = value;
        }

        public int CharLength
        {
            get => _table.CharLength[_index];
            set => _table.CharLength[_index] = value;
        }

        public ArtifactContent Snippet
        {
            get => _table.Database.ArtifactContent[_table.Snippet[_index]];
            set => _table.Snippet[_index] = value._index;
        }

        public Message Message
        {
            get => _table.Database.Message[_table.Message[_index]];
            set => _table.Message[_index] = value._index;
        }

        public string SourceLanguage
        {
            get => _table.SourceLanguage[_index];
            set => _table.SourceLanguage[_index] = value;
        }

    }

    /// <summary>
    ///  SoA Table for 'Region' type.
    /// </summary>
    public class RegionTable : Table<Region>
    {
        internal SarifLogBsoa Database;

        internal NumberColumn<int> StartLine;
        internal NumberColumn<int> StartColumn;
        internal NumberColumn<int> EndLine;
        internal NumberColumn<int> EndColumn;

        internal NumberColumn<int> ByteOffset;
        internal NumberColumn<int> ByteLength;
        internal NumberColumn<int> CharOffset;
        internal NumberColumn<int> CharLength;

        internal RefColumn Snippet;
        internal RefColumn Message;

        internal StringColumn SourceLanguage;

        // Properties

        public RegionTable(SarifLogBsoa database) : base()
        {
            this.Database = database;

            this.StartLine = AddColumn(nameof(StartLine), new NumberColumn<int>(0));
            this.StartColumn = AddColumn(nameof(StartColumn), new NumberColumn<int>(0));
            this.EndLine = AddColumn(nameof(EndLine), new NumberColumn<int>(0));
            this.EndColumn = AddColumn(nameof(EndColumn), new NumberColumn<int>(0));

            this.ByteOffset = AddColumn(nameof(ByteOffset), new NumberColumn<int>(-1));
            this.ByteLength = AddColumn(nameof(ByteLength), new NumberColumn<int>(0));
            this.CharOffset = AddColumn(nameof(CharOffset), new NumberColumn<int>(-1));
            this.CharLength = AddColumn(nameof(CharLength), new NumberColumn<int>(0));

            this.Snippet = AddColumn(nameof(Snippet), new RefColumn(nameof(SarifLogBsoa.ArtifactContent)));
            this.Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogBsoa.Message)));

            this.SourceLanguage = AddColumn(nameof(SourceLanguage), new StringColumn());
        }

        public override Region this[int index] => new Region(this, index);
    }
}
