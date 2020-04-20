namespace BSOA.Demo.Model
{
    /// <summary>
    ///  Region4 is a struct *and* uses the SoA (struct of Arrays) pattern,
    ///  allowing reading by column in bulk and avoiding any storage for unused columns.
    /// </summary>
    public struct Region4
    {
        private RegionTable Table { get; set; }
        private int Index { get; set; }

        public Region4(RegionTable table)
        {
            table.Add();
            this.Table = table;
            this.Index = (table.Count - 1);
        }

        public Region4(RegionTable table, int index)
        {
            this.Table = table;
            this.Index = index;
        }

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
    }
}
