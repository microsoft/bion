using BSOA.IO;
using System.Collections.Generic;

namespace BSOA
{
    public interface IColumn : IBinarySerializable, ITreeSerializable
    {
        // Remove all items from column
        void Clear();
    }

    public interface IColumn<T> : IReadOnlyList<T>, IColumn
    {
        new T this[int index] { get; set; }

        // IReadOnlyList
        // -------------
        // int Count { get; }
        // T this[int index] { get; }
        // IEnumerator<T> GetEnumerator();

        // IBinarySerializable
        // -------------------
        // void Read(BinaryReader reader, ref byte[] buffer);
        // void Write(BinaryWriter writer, ref byte[] buffer);

        // ITreeSerializable
        // -----------------
        // void Write(ITreeWriter writer);
        // void Read(ITreeReader reader);
    }
}
