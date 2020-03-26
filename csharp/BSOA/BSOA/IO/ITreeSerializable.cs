namespace BSOA.IO
{
    /// <summary>
    ///  Types implement ITreeSerializable to describe how they are written and read
    ///  in terms of a generic hierarchy which can be implemented in different formats.
    /// </summary>
    public interface ITreeSerializable
    {
        void Write(ITreeWriter writer);
        void Read(ITreeReader reader);
    }
}
