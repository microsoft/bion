using BSOA.IO;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace BSOA.Test
{
    public static class BinarySerializable
    {
        public static T RoundTrip<T>(T value) where T : IBinarySerializable, new()
        {
            return RoundTrip(value, () => new T());
        }

        public static T RoundTrip<T>(T value, Func<T> builder) where T : IBinarySerializable
        {
            byte[] buffer = null;
            T roundTripped = builder();

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                {
                    value.Write(writer, ref buffer);
                }

                long lengthWritten = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
                {
                    roundTripped.Read(reader, ref buffer);
                }

                // Ensure the same number of bytes written were read back
                Assert.Equal(lengthWritten, stream.Position);
            }

            return roundTripped;
        }
    }
}
