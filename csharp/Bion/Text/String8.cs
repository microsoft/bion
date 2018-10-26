using Bion.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Bion.Text
{
    public struct String8 : IEquatable<String8>, IComparable<String8>
    {
        public byte[] Array;
        public int Index;
        public int Length;

        public static readonly String8 Empty = new String8(null, 0, 0);

        private String8(byte[] array) : this(array, 0, array.Length)
        { }

        private String8(byte[] array, int index, int length)
        {
            Array = array;
            Index = index;
            Length = length;
        }

        public static String8 Copy(string value, ref byte[] convertBuffer)
        {
            // Start with maximum possible needed byte length
            int length = value.Length * 3;

            // If buffer isn't big enough, get real length
            if (convertBuffer == null || convertBuffer.Length < length)
            {
                length = Encoding.UTF8.GetByteCount(value);
            }

            // If buffer isn't big enough, expand
            if (convertBuffer == null || convertBuffer.Length < length)
            {
                convertBuffer = new byte[length];
            }

            // Convert and return String8
            length = Encoding.UTF8.GetBytes(value, convertBuffer);
            return new String8(convertBuffer, 0, length);
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

            for (int i = 0; i < this.Length; ++i)
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

        /// <summary>
        ///  Compare this String8 to another one. Returns which String8 sorts earlier.
        /// </summary>
        /// <param name="other">String8 to compare to</param>
        /// <returns>Negative if this String8 sorts earlier, zero if equal, positive if this String8 sorts later</returns>
        public int CompareTo(String8 other)
        {
            // If String8s point to the same thing, return the same
            if (other.Index == Index && other.Array == Array && other.Length == Length) { return 0; }

            // If one or the other is empty, the non-empty one is greater
            if (this.Length == 0)
            {
                return (other.Length == 0 ? 0 : -1);
            }
            else if (other.Length == 0)
            {
                return 1;
            }

            // Next, compare up to the length both strings are
            int cmp = 0;
            int commonLength = Math.Min(this.Length, other.Length);
            for (int i = 0; i < commonLength && cmp == 0; ++i)
            {
                cmp = this.Array[Index + i].CompareTo(other.Array[other.Index + i]);
            }

            if (cmp != 0) { return cmp; }

            // If all bytes are equal, the longer one is later
            return Length.CompareTo(other.Length);
        }
    }
}
