namespace BSOA.Model
{
    /// <summary>
    ///  Types which represent Rows from a Table must implement IRow.
    /// </summary>
    public interface IRow
    {
        // Provide the Table and Index to allow the Table to identify
        // the source of the row and copy it from other Table instances.
        ITable Table { get; }
        int Index { get; }

        // Reset this instance to refer to another row.
        // This provides a way to set up many instances without allocation for each.
        void Reset(ITable table, int index);
    }
}
