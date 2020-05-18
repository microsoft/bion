namespace BSOA
{
    /// <summary>
    ///  Container for standard names written to BSOA files.
    /// </summary>
    public static class Names
    {
        // IColumn standard and wrapped columns
        public const string Count = nameof(Count);
        public const string Values = nameof(Values);

        // Table
        public const string Columns = nameof(Columns);

        // NullableColumn
        public const string IsNull = nameof(IsNull);

        // BitVector
        public const string Capacity = nameof(Capacity);

        // Columns containing direct arrays
        public const string Array = nameof(Array);

        // Columns containing int indices to other rows
        public const string Indices = nameof(Indices);

        // NumberListColumn
        public const string Chapters = nameof(Chapters);
        public const string PageStart = nameof(PageStart);
        public const string ValueEnd = nameof(ValueEnd);
        public const string SmallValues = nameof(SmallValues);
        public const string LargeValues = nameof(LargeValues);
    }
}
