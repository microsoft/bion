using Bion.Core;
using System;
using System.Text;

namespace Bion.Text
{
    public struct String8 : IEquatable<String8>
    {
        public byte[] Array;
        public int Index;
        public int Length;

        public static readonly String8 Empty = new String8(null, 0, 0);

        private String8(byte[] array, int index, int length)
        {
            Array = array;
            Index = index;
            Length = length;
        }

        public String8(string value)
        {
            Array = Encoding.UTF8.GetBytes(value);
            Index = 0;
            Length = Array.Length;
        }

        public static String8 Reference(byte[] array, int index, int length)
        {
            return new String8(array, index, length);
        }

        public static String8 Copy(String8 value)
        {
            return String8.Copy(value.Array, value.Index, value.Length);
        }

        public static String8 Copy(byte[] array, int index, int length)
        {
            byte[] copy = new byte[length];
            Buffer.BlockCopy(array, index, copy, 0, length);
            return new String8(copy, 0, length);
        }

        public static String8 Copy(ReadOnlyMemory<byte> memory)
        {
            return String8.Copy(memory.Span);
        }

        public static String8 Copy(ReadOnlySpan<byte> span)
        {
            byte[] copy = new byte[span.Length];
            span.CopyTo(copy);
            return new String8(copy, 0, span.Length);
        }

        public Span<byte> Span => this.Array.AsSpan(this.Index, this.Length);

        public void CopyTo(byte[] array, int index)
        {
            Buffer.BlockCopy(this.Array, this.Index, array, index, this.Length);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is String8)) { return false; }
            return Equals((String8)obj);
        }

        public bool Equals(String8 other)
        {
            if (this.Length != other.Length) { return false; }

            for(int i = 0; i < this.Length; ++i)
            {
                if (this.Array[this.Index + i] != other.Array[other.Index + i]) { return false; }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return unchecked((int)Hashing.Murmur2(Array, Index, Length, 0));
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(Array, Index, Length);
        }
    }
}
