
namespace ScaleDemo.SoA
{
    public interface IColumn<T> : IBinarySerializable
    {
        T this[int index] { get; set; }
    }
}
