namespace BSOA.Column
{
    /// <summary>
    ///  RefListColumn provides a reference from an item in one table to a set
    ///  of items in another table. It stores the integer indices of the references.
    /// </summary>
    public class NumberListColumn<T> : WrappingColumn<NumberList<T>, ArraySlice<T>> where T : unmanaged
    {
        public NumberListColumn() : base(new ArraySliceColumn<T>())
        { }

        public override NumberList<T> this[int index] 
        {
            get => new NumberList<T>(Inner, index);
            set => Inner[index] = value.Slice;
        }
    }
}