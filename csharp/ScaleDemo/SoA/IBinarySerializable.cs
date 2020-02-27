using System.IO;

namespace ScaleDemo.SoA
{
    public interface IBinarySerializable
    {
        void Read(BinaryReader reader, ref byte[] buffer);
        void Write(BinaryWriter writer, ref byte[] buffer);
    }
}
