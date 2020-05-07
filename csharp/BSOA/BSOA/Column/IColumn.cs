using BSOA.IO;
using System.Collections.Generic;

namespace BSOA.Column
{
    public interface IColumn : ITreeSerializable
    {
        // Return if unused (Count == 0)
        bool Empty { get; }

        // Remove excess capacity and prepare to serialize
        void Trim();

        // Remove last rows from column
        void RemoveFromEnd(int count);

        // Swap two values efficiently within column
        void Swap(int index1, int index2);
    }

    public interface IColumn<T> : IReadOnlyList<T>, IColumn
    {
        new T this[int index] { get; set; }

        // IReadOnlyList
        // -------------
        // int Count { get; }
        // T this[int index] { get; }
        // IEnumerator<T> GetEnumerator();

        // ITreeSerializable
        // -----------------
        // void Write(ITreeWriter writer);
        // void Read(ITreeReader reader);
    }
}
