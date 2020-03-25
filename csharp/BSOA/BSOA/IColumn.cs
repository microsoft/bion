using BSOA.IO;
using System.Collections.Generic;

namespace BSOA
{
    public interface IColumn<T> : IReadOnlyList<T>, IBinarySerializable
    {
        new T this[int index] { get; set; }

        // int Count { get; }
        // T this[int index] { get; }
        // IEnumerator<T> GetEnumerator();

        // void Read(BinaryReader reader, ref byte[] buffer);
        // void Write(BinaryWriter writer, ref byte[] buffer);
    }
}
