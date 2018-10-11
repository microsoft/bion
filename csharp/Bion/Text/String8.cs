using Bion.Core;
using System;
using System.Text;

namespace Bion.Text
{
    public struct String8 : IEquatable<String8>
    {
        public ReadOnlyMemory<byte> Value { get; set; }

        public String8(ReadOnlyMemory<byte> value)
        {
            Value = value;
        }

        public String8(string value)
        {
            Value = Encoding.UTF8.GetBytes(value);
        }

        public static String8 Copy(String8 value)
        {
            byte[] copy = new byte[value.Value.Length];
            value.Value.CopyTo(copy);
            return new String8(copy);
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
