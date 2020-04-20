using BSOA.IO;
using System.Collections.Generic;

namespace BSOA.Model
{
    public interface ITable<T> : ITreeSerializable, IReadOnlyList<T>
    {
        T Add();
    }
}
