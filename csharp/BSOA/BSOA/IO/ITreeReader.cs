using System;
using System.Collections.Generic;

namespace BSOA.IO
{
    /// <summary>
    ///  ITreeReader is a generic interface for hierarchical deserialization.
    ///  Types in a hierarchy are read as single values, arrays, and sub-objects.
    /// </summary>
    /// <remarks>
    ///  Future:
    ///    - Partial Array reading (read huge array in parts?)
    ///    - Position, Seek, Skip support for partial loading? (Could this be handled in the implementation?)
    /// </remarks>
    public interface ITreeReader
    {
        void ReadObject<T>(ref T instance, Dictionary<string, Action<ITreeReader, T>> propertyReaders);

        bool ReadBoolean();
        short ReadInt16();
        int ReadInt32();
        long ReadInt64();
        string ReadString();

        T[] ReadArray<T>() where T : unmanaged;
    }
}
