namespace BSOA.IO
{
    /// <summary>
    ///  Types implement ITreeSerializable to support agnostic serialization.
    ///  ITreeReader and ITreeWriter support a clean subset of Newtonsoft's JsonReader and JsonWriter.
    ///  
    ///  Serialization follows the same rules as JSON - one root element, 
    ///  objects have propertyName-value pairs, arrays have values only.
    ///  
    ///  Readers and Writers add support for BlockArrays, which should be written and read without
    ///  per element conditions or calls. This allows great I/O performance for large data.
    /// </summary>
    public interface ITreeSerializable
    {
        void Write(ITreeWriter writer);
        void Read(ITreeReader reader);
    }
}
