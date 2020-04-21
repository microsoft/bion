using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  Typed SoA table for SARIF Regions.
    /// </summary>
    /// <remarks>
    ///  - Has strongly typed column properties for instant access by Region properties.
    ///  - Controls column construction (types and defaults).
    ///  - Calls AddColumn on columns (so base class can manage serialization)
    ///  - Provides indexer (controls construction of item instances).
    /// </remarks>
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
    }
}
