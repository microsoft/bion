using BSOA.Model;

namespace BSOA.Demo.Model
{
    public class RegionTable : Table<Region4>
    {
        internal NumberColumn<int> StartLine;
        internal NumberColumn<int> StartColumn;
        internal NumberColumn<int> EndLine;
        internal NumberColumn<int> EndColumn;

        internal NumberColumn<int> ByteOffset;
        internal NumberColumn<int> ByteLength;
        internal NumberColumn<int> CharOffset;
        internal NumberColumn<int> CharLength;

        public RegionTable() : base()
        {
            this.StartLine = AddColumn(nameof(StartLine), new NumberColumn<int>(0));
            this.StartColumn = AddColumn(nameof(StartColumn), new NumberColumn<int>(0));
            this.EndLine = AddColumn(nameof(EndLine), new NumberColumn<int>(0));
            this.EndColumn = AddColumn(nameof(EndColumn), new NumberColumn<int>(0));

            this.ByteOffset = AddColumn(nameof(ByteOffset), new NumberColumn<int>(-1));
            this.ByteLength = AddColumn(nameof(ByteLength), new NumberColumn<int>(0));
            this.CharOffset = AddColumn(nameof(CharOffset), new NumberColumn<int>(-1));
            this.CharLength = AddColumn(nameof(CharLength), new NumberColumn<int>(0));
        }

        public override Region4 this[int index] => new Region4(this, index);

        public override Region4 Add()
        {
            this.Count++;
            return this[this.Count - 1];
        }
    }
}
