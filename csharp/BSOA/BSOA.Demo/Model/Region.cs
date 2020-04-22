using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  SoA Item for 'Region' type.
    /// </summary>
    public struct Region
    {
        internal RegionTable Table { get; }
        internal int Index { get; }

        internal Region(RegionTable table, int index)
        {
            this.Table = table;
            this.Index = index;
        }

        public Region(RegionTable table) : this(table, table.Count)
        {
            table.Add();
        }

        public Region(SarifLogBsoa database) : this(database.Region)
        { }

        public int StartLine
        {
            get => Table.StartLine[Index];
            set => Table.StartLine[Index] = value;
        }

        public int StartColumn
        {
            get => Table.StartColumn[Index];
            set => Table.StartColumn[Index] = value;
        }

        public int EndLine
        {
            get => Table.EndLine[Index];
            set => Table.EndLine[Index] = value;
        }

        public int EndColumn
        {
            get => Table.EndColumn[Index];
            set => Table.EndColumn[Index] = value;
        }

        public int ByteOffset
        {
            get => Table.ByteOffset[Index];
            set => Table.ByteOffset[Index] = value;
        }

        public int ByteLength
        {
            get => Table.ByteLength[Index];
            set => Table.ByteLength[Index] = value;
        }

        public int CharOffset
        {
            get => Table.CharOffset[Index];
            set => Table.CharOffset[Index] = value;
        }

        public int CharLength
        {
            get => Table.CharLength[Index];
            set => Table.CharLength[Index] = value;
        }

        public ArtifactContent Snippet
        {
            get => Table.Database.ArtifactContent[Table.Snippet[Index]];
            set => Table.Snippet[Index] = value.Index;
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

        // Message Message
        // ArtifactContent Snippet
        // string SourceLanguage
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
        }

        public override Region this[int index] => new Region(this, index);
    }
}
