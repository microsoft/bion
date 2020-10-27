namespace BSOA.Model
{
    /// <summary>
    ///  Columns which contain cross-table references must implement IRefColumn
    ///  to support Garbage Collection.
    /// </summary>
    public interface IRefColumn : INumberColumn<int>
    {
        // Used to walk database items to find all reachable rows
        string ReferencedTableName { get; }

        // Used to remap indices after unreachable rows are removed
        // void ForEach(Action<ArraySlice<int>>) [from INumberColumn<int>]
    }
}
