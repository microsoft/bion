using BSOA.IO;
using System.Collections.Generic;

namespace ScaleDemo.SoA
{
    public interface ITable<T> : ITreeSerializable, IReadOnlyList<T>
    { }
}
