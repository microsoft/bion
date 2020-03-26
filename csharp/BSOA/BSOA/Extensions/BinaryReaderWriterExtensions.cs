using System;
using System.IO;

namespace BSOA.Extensions
{
    public static class BinaryReaderWriterExtensions
    {
        public static T[] ReadArray<T>(this BinaryReader reader, ref byte[] buffer) where T : unmanaged
        {
            int byteLength = reader.ReadInt32();
            if (byteLength == 0) { return Array.Empty<T>(); }

            int count = byteLength / SizeOf(typeof(T));
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
            // Negative count means write the rest of the array
            if (count < 0) { count = array?.Length - index ?? 0; }

            int elementSize = SizeOf(typeof(T));
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

        public static int SizeOf(Type t)
        {
            if (t == typeof(bool) || t == typeof(byte) || t == typeof(sbyte))
            {
                return 1;
            }
            else if (t == typeof(char) || t == typeof(short) || t == typeof(ushort))
            {
                return 2;
            }
            else if (t == typeof(float) || t == typeof(int) || t == typeof(uint))
            {
                return 4;
            }
            else if (t == typeof(double) || t == typeof(long) || t == typeof(ulong))
            {
                return 8;
            }
            else
            {
                throw new NotSupportedException($"{t.Name} is not supported as a primitive.");
            }
        }
    }
}
