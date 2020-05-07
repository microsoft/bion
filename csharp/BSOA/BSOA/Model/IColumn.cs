using BSOA.IO;

namespace BSOA.Model
{
    public interface IColumn<T> : ILimitedList<T>, IColumn
    {
        // Clear all items from column
        new void Clear();
    }

    public interface IColumn : ILimitedList, ITreeSerializable
    {
        // Clear all items from column
        new void Clear();

        // Remove excess capacity and prepare to serialize
        void Trim();
    }
}
