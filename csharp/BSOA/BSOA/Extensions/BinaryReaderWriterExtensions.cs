using System;
using System.IO;
using System.Runtime.InteropServices;

namespace BSOA.Extensions
{
    public static class BinaryReaderWriterExtensions
    {
        public static T[] ReadArray<T>(this BinaryReader reader, ref byte[] buffer) where T : unmanaged
        {
            int byteLength = reader.ReadInt32();
            if (byteLength == 0) { return Array.Empty<T>(); }

            int count = byteLength / Marshal.SizeOf(typeof(T));
            T[] array = new T[count];

            if (buffer == null || buffer.Length < byteLength) { buffer = new byte[byteLength]; }

            reader.Read(buffer, 0, byteLength);
            Buffer.BlockCopy(buffer, 0, array, 0, byteLength);

            return array;
        }

        public static void WriteArray<T>(this BinaryWriter writer, T[] array, ref byte[] buffer) where T : unmanaged
        {
            WriteArray<T>(writer, array, 0, array?.Length ?? 0, ref buffer);
        }

        public static void WriteArray<T>(this BinaryWriter writer, T[] array, int index, int count, ref byte[] buffer) where T : unmanaged
        {
            int elementSize = Marshal.SizeOf(typeof(T));
            int byteOffset = index * elementSize;
            int byteLength = count * elementSize;
            writer.Write(byteLength);

            if (byteLength > 0)
            {
                if (buffer == null || buffer.Length < byteLength) { buffer = new byte[byteLength]; }
                Buffer.BlockCopy(array, byteOffset, buffer, 0, byteLength);
                writer.Write(buffer, 0, byteLength);
            }
        }
    }
}
