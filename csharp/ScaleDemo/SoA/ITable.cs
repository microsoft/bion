using System.Collections.Generic;

namespace ScaleDemo.SoA
{
    public interface ITable<T> : IBinarySerializable, IReadOnlyList<T>
    { }
}
