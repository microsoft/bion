using Bion.Core;
using System;
using System.Text;

namespace Bion.Text
{
    public struct String8 : IEquatable<String8>
    {
        public ReadOnlyMemory<byte> Value { get; set; }
        public int Length => Value.Length;

        private String8(ReadOnlyMemory<byte> value)
        {
            Value = value;
        }

        public String8(string value)
        {
            Value = Encoding.UTF8.GetBytes(value);
        }

        public static String8 Reference(ReadOnlyMemory<byte> value)
        {
            return new String8(value);
        }

        public static String8 Copy(String8 value)
        {
            return String8.Copy(value.Value);
        }

        public static String8 Copy(ReadOnlyMemory<byte> value)
        {
            byte[] copy = new byte[value.Length];
            value.CopyTo(copy);
            return new String8(copy);
        }

        public void CopyTo(Memory<byte> memory)
        {
            Value.CopyTo(memory);
        }

        public void CopyTo(Span<byte> span)
        {
            Value.Span.CopyTo(span);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is String8)) { return false; }
            return Equals((String8)obj);
        }

        public bool Equals(String8 other)
        {
            if (Value.Length != other.Value.Length) { return false; }

            ReadOnlySpan<byte> left = Value.Span;
            ReadOnlySpan<byte> right = other.Value.Span;
            for(int i = 0; i < left.Length; ++i)
            {
                if (left[i] != right[i]) { return false; }
            }

            return true;
        }

        public override int GetHashCode()
        {
            return unchecked((int)Hashing.Murmur2(Value.Span, 0));
        }

        public override string ToString()
        {
            return Encoding.UTF8.GetString(Value.Span);
        }
    }
}
