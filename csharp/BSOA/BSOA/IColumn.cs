using BSOA.IO;

namespace BSOA
{
    public interface IColumn<T> : IBinarySerializable
    {
        int Count { get; }
        T this[int index] { get; set; }
    }
}
