// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;
using System.Text;

using BSOA.IO;

namespace BSOA.Extensions
{
    public static class BinaryReaderWriterExtensions
    {
        public static T[] ReadBlockArray<T>(this BinaryReader reader, ref byte[] buffer) where T : unmanaged
        {
            return reader.ReadBlockArray<T>((byte)(reader.ReadByte() >> 4), ref buffer);
        }

        public static T[] ReadBlockArray<T>(this BinaryReader reader, byte hint, ref byte[] buffer) where T : unmanaged
        {
            int byteLength = (int)reader.ReadLong(hint);
            if (byteLength == 0) { return Array.Empty<T>(); }

            int count = byteLength / SizeOf(typeof(T));
            T[] array = new T[count];

            if (buffer == null || buffer.Length < byteLength) { buffer = new byte[byteLength]; }

            reader.Read(buffer, 0, byteLength);
            Buffer.BlockCopy(buffer, 0, array, 0, byteLength);

            return array;
        }

        public static void SkipBlockArray(this BinaryReader reader, byte hint)
        {
            int byteLength = (int)reader.ReadLong(hint);
            reader.BaseStream.Seek(byteLength, SeekOrigin.Current);
        }

        public static void WriteBlockArray<T>(this BinaryWriter writer, T[] array, ref byte[] buffer) where T : unmanaged
        {
            WriteBlockArray<T>(writer, array, 0, array?.Length ?? 0, ref buffer);
        }

        public static void WriteBlockArray<T>(this BinaryWriter writer, T[] array, int index, int count, ref byte[] buffer) where T : unmanaged
        {
            // Negative count means write the rest of the array
            if (array == null) { count = 0; }
            if (count < 0) { count = array?.Length - index ?? 0; }

            int elementSize = SizeOf(typeof(T));
            int byteOffset = index * elementSize;
            int byteLength = count * elementSize;

            writer.WriteLong(TreeToken.BlockArray, byteLength);

            if (byteLength > 0)
            {
                if (buffer == null || buffer.Length < byteLength) { buffer = new byte[byteLength]; }
                Buffer.BlockCopy(array, byteOffset, buffer, 0, byteLength);
                writer.Write(buffer, 0, byteLength);
            }
        }

        public static string ReadString(this BinaryReader reader, byte hint, ref byte[] buffer)
        {
            int byteLength = (int)reader.ReadLong(hint);

            if (byteLength == 0)
            {
                return string.Empty;
            }
            else
            {
                if (buffer == null || buffer.Length < byteLength) { buffer = new byte[byteLength]; }
                reader.Read(buffer, 0, byteLength);

                return Encoding.UTF8.GetString(buffer, 0, byteLength);
            }
        }

        public static void WriteString(this BinaryWriter writer, TreeToken token, string value, ref byte[] buffer)
        {
            if (string.IsNullOrEmpty(value))
            {
                writer.WriteMarker(token, 0);
            }
            else
            {
                // Ensure buffer long enough
                int maxByteLength = 3 * value.Length;
                if (buffer == null || buffer.Length < maxByteLength) { buffer = new byte[maxByteLength]; }

                // Convert to UTF-8
                int actualByteLength = Encoding.UTF8.GetBytes(value, 0, value.Length, buffer, 0);

                // Write marker and length, then bytes
                writer.WriteLong(token, actualByteLength);
                writer.Write(buffer, 0, actualByteLength);
            }
        }

        public static long ReadLong(this BinaryReader reader, byte hint)
        {
            switch (hint)
            {
                case 15:
                    return reader.ReadInt64();
                case 14:
                    return reader.ReadUInt32();
                case 13:
                    return reader.ReadUInt16();
                case 12:
                    return reader.ReadByte();
                default:
                    return hint;
            }
        }

        public static void WriteLong(this BinaryWriter writer, TreeToken token, long value)
        {
            if (value < 0 || value > uint.MaxValue)
            {
                // Hint 15: 8 byte value
                writer.WriteMarker(token, 15);
                writer.Write(value);
            }
            else if (value > ushort.MaxValue)
            {
                // Hint 14: 4 byte value
                writer.WriteMarker(token, 14);
                writer.Write((uint)value);
            }
            else if (value > byte.MaxValue)
            {
                // Hint 13: 2 byte value
                writer.WriteMarker(token, 13);
                writer.Write((ushort)value);
            }
            else if (value > 11)
            {
                // Hint 12: 1 byte value
                writer.WriteMarker(token, 12);
                writer.Write((byte)value);
            }
            else
            {
                // Hint 0-11 is value
                writer.WriteMarker(token, (int)value);
            }
        }

        public static void WriteMarker(this BinaryWriter writer, TreeToken token, int hint)
        {
            if (hint < 0 || hint > 15) { throw new ArgumentException(nameof(hint)); }
            writer.Write((byte)((byte)(token) + (byte)(hint << 4)));
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
