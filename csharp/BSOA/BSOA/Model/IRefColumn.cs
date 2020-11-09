using BSOA.GC;

namespace BSOA.Model
{
    /// <summary>
    ///  Columns which contain cross-table references must implement IRefColumn
    ///  to support Garbage Collection.
    /// </summary>
    public interface IRefColumn : INumberColumn<int>
    {
        // Used to identify table these values refer to, to walk all reachable rows
        string ReferencedTableName { get; }

        // Recursively add rows referenced by the given row
        long Traverse(int index, IGraphTraverser referencedTableCollector);

        // Used to remap indices after unreachable rows are removed
        // void ForEach(Action<ArraySlice<int>>) [from INumberColumn<int>]
    }
}
