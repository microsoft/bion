using BSOA.Extensions;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace BSOA.Test.Extensions
{
    public class BinaryReaderWriterExtensionsTests
    {
        [Fact]
        public static void ReadAndWriteArray_Null()
        {
            byte[] buffer = null;
            int[] roundTripped = null;
                
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                {
                    writer.WriteArray<int>(null, ref buffer);
                }

                long position = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);
                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
                {
                    roundTripped = reader.ReadArray<int>(ref buffer);
                }

                // Ensure all bytes (length only) read back
                Assert.Equal(position, stream.Position);

                // Ensure array is empty (not null)
                Assert.NotNull(roundTripped);
                Assert.Empty(roundTripped);
            }
        }

        [Fact]
        public static void ReadAndWriteArray_BufferUse()
        {
            int[] sample = new int[] { 1, 2, 3 };
            byte[] buffer = null;

            using (MemoryStream stream = new MemoryStream())
            {
                // Write, ensure buffer used (started as null)
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                {
                    writer.WriteArray<int>(sample, ref buffer);
                }

                Assert.True(buffer?.Length >= 12);


                // Write, ensure buffer used (too small)
                buffer = new byte[11];
                stream.Seek(0, SeekOrigin.Begin);
                using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
                {
                    writer.WriteArray<int>(sample, ref buffer);
                }

                Assert.True(buffer?.Length >= 12);


                // Read, ensure buffer used (null)
                buffer = null;
                stream.Seek(0, SeekOrigin.Begin);
                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
                {
                    int[] roundTripped = reader.ReadArray<int>(ref buffer);
                }

                Assert.True(buffer?.Length >= 12);


                // Read, ensure buffer used (too small)
                buffer = new byte[11];
                stream.Seek(0, SeekOrigin.Begin);
                using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
                {
                    int[] roundTripped = reader.ReadArray<int>(ref buffer);
                }

                Assert.True(buffer?.Length >= 12);
            }
        }

        [Fact]
        public void SizeOf()
        {
            // Verify char size reported properly (Marshal.SizeOf reports one because it defaults to marshalling to ASCII)
            Assert.Equal(2, BinaryReaderWriterExtensions.SizeOf(typeof(char)));
            Assert.Throws<NotSupportedException>(() => BinaryReaderWriterExtensions.SizeOf(typeof(Guid)));
        }
    }
}
