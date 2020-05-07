namespace BSOA.Column
{
    /// <summary>
    ///  RefListColumn provides a reference from an item in one table to a set
    ///  of items in another table. It stores the integer indices of the references.
    /// </summary>
    public class RefListColumn : WrappingColumn<NumberList<int>, ArraySlice<int>>
    {
        public string ReferencedTableName { get; }

        public RefListColumn(string referencedTableName) : base(new NumberListColumn<int>())
        {
            ReferencedTableName = referencedTableName;
        }

        public override NumberList<int> this[int index] 
        {
            get => new NumberList<int>(Inner, index);
            set => Inner[index] = value.Slice;
        }
    }
}