using System.IO;

namespace BSOA.IO
{
    public interface IBinarySerializable
    {
        void Read(BinaryReader reader, ref byte[] buffer);
        void Write(BinaryWriter writer, ref byte[] buffer);
    }
}
