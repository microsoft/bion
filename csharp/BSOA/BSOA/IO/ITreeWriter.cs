namespace BSOA.IO
{
    /// <summary>
    ///  ITreeWriter is a generic interface for hierarchical serialization.
    ///  Types in a hierarchy can describe how they serialize name/value pairs,
    ///  and different serialization formats can implement ITreeWriter to add format support.
    /// </summary>
    public interface ITreeWriter
    {
        void WriteStartObject();
        void WriteEndObject();

        void Write(string name, string value);
        void Write(string name, bool value);
        void Write(string name, short value);
        void Write(string name, int value);
        void Write(string name, long value);

        void WriteArray<T>(string name, T[] array, int index, int count) where T : unmanaged;
    }
}
