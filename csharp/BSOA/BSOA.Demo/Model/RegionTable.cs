using BSOA.Column;
using BSOA.Model;
using System;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Region' entity.
    /// </summary>
    public partial class RegionTable : Table<Region>
    {
        internal SarifLogBsoa Database;

        internal IColumn<int> StartLine;
        internal IColumn<int> StartColumn;
        internal IColumn<int> EndLine;
        internal IColumn<int> EndColumn;
        internal IColumn<int> ByteOffset;
        internal IColumn<int> ByteLength;
        internal IColumn<int> CharOffset;
        internal IColumn<int> CharLength;
        internal RefColumn Snippet;
        internal RefColumn Message;
        internal IColumn<string> SourceLanguage;

        public RegionTable(SarifLogBsoa database) : base()
        {
            Database = database;

            StartLine = AddColumn(nameof(StartLine), ColumnFactory.Build<int>(0));
            StartColumn = AddColumn(nameof(StartColumn), ColumnFactory.Build<int>(0));
            EndLine = AddColumn(nameof(EndLine), ColumnFactory.Build<int>(0));
            EndColumn = AddColumn(nameof(EndColumn), ColumnFactory.Build<int>(0));
            ByteOffset = AddColumn(nameof(ByteOffset), ColumnFactory.Build<int>(-1));
            ByteLength = AddColumn(nameof(ByteLength), ColumnFactory.Build<int>(0));
            CharOffset = AddColumn(nameof(CharOffset), ColumnFactory.Build<int>(-1));
            CharLength = AddColumn(nameof(CharLength), ColumnFactory.Build<int>(0));
            Snippet = AddColumn(nameof(Snippet), new RefColumn(nameof(SarifLogBsoa.ArtifactContent)));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogBsoa.Message)));
            SourceLanguage = AddColumn(nameof(SourceLanguage), ColumnFactory.Build<string>(null));
        }

        public override Region Get(int index)
        {
            return (index == -1 ? null : new Region(this, index));
        }
    }
}
