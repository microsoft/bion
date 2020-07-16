// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace BSOA
{
    /// <summary>
    ///  Container for standard names written to BSOA files.
    /// </summary>
    public static class Names
    {
        // IColumn standard and wrapped columns
        public const string Count = "c";
        public const string Values = "v";

        // Columns containing direct arrays
        public const string Array = "a";

        // Columns containing int indices to other rows
        public const string Indices = "i";

        // Table
        public const string Columns = "o";

        // NullableColumn
        public const string IsNull = "n";

        // BitVector
        public const string Capacity = "y";

        // NumberListColumn
        public const string Chapters = "C";
        public const string PageStart = "P";
        public const string ValueEnd = "V";
        public const string SmallValues = "S";
        public const string LargeValues = "L";

        // DictionaryColumn
        public const string Keys = "k";
        public const string Pairs = "p";
    }

    //// Verbose form
    //public static class Names
    //{
    //    // IColumn standard and wrapped columns
    //    public const string Count = "Count";
    //    public const string Values = "Values";

    //    // Columns containing direct arrays
    //    public const string Array = "Array";

    //    // Columns containing int indices to other rows
    //    public const string Indices = "Indices";

    //    // Table
    //    public const string Columns = "Columns";

    //    // NullableColumn
    //    public const string IsNull = "IsNull";

    //    // BitVector
    //    public const string Capacity = "Capacity";

    //    // NumberListColumn
    //    public const string Chapters = "Chapters";
    //    public const string PageStart = "PageStart";
    //    public const string ValueEnd = "ValueEnd";
    //    public const string SmallValues = "SmallValues";
    //    public const string LargeValues = "LargeValues";

    //    // DictionaryColumn
    //    public const string Keys = "Keys";
    //    public const string Pairs = "Pairs";
    //}
}
