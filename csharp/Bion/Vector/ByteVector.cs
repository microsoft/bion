using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Bion.Vector
{
    public static unsafe class ByteVector
    {
        public static int CountGreaterThan(Span<byte> content, byte cutoff)
        {
            if (content.Length > 128 && Avx2.IsSupported)
            {
                return CountGreaterThanAvx(content, cutoff);
            }
            else
            {
                return CountGreaterThanCs(content, cutoff);
            }
        }

        private static int CountGreaterThanCs(Span<byte> content, byte cutoff)
        {
            int count = 0;

            for (int i = 0; i < content.Length; ++i)
            {
                if (content[i] > cutoff) count++;
            }

            return count;
        }

        private static int CountGreaterThanAvx(Span<byte> content, byte cutoff)
        {
            int count = 0;

            // Load a vector to convert unsigned to signed value order (only signed compare supported)
            Vector256<sbyte> toSignedV = SetAllTo(128);

            // Load a vector of the compare cutoff and convert to signed
            Vector256<sbyte> cutoffV = SetAllTo(cutoff);
            cutoffV = Avx2.Subtract(cutoffV, toSignedV);

            fixed (byte* contentPtr = &content[0])
            {
                int blockLength = content.Length / 32;
                for (int i = 0; i < blockLength; ++i)
                {
                    // Load a vector of content and convert to signed
                    Vector256<sbyte> contentV = Unsafe.ReadUnaligned<Vector256<sbyte>>(&contentPtr[i * 32]);
                    contentV = Avx2.Subtract(contentV, toSignedV);

                    // Find bytes greater than the cutoff
                    Vector256<sbyte> result = Avx2.CompareGreaterThan(contentV, cutoffV);

                    // Convert the byte mask of matches to a bit mask
                    uint bits = unchecked((uint)Avx2.MoveMask(result));

                    // Count the bits for the number of matches
                    count += Popcnt.PopCount(bits);
                }

                count += CountGreaterThanCs(content.Slice(blockLength * 32), cutoff);
            }

            return count;
        }

        public static int Skip(Span<byte> content, ref int depth)
        {
            if (content.Length > 128 && Avx2.IsSupported)
            {
                return SkipAvx(content, ref depth);
            }
            else
            {
                return SkipCs(content, ref depth);
            }
        }

        private static int SkipCs(Span<byte> content, ref int depth)
        {
            int i;
            for (i = 0; i < content.Length; ++i)
            {
                byte value = content[i];
                if (value > 0xFB)
                {
                    // Depth +1 for 0xFF, 0xFE, -1 for 0xFD, 0xFC.
                    // Second to last bit is one for open, zero for close.
                    depth += (value & 0x2) - 1;
                    if (depth == 0) break;
                }
            }

            return i;
        }

        private static int SkipAvx(Span<byte> content, ref int depth)
        {
            // Load a vector to convert unsigned to signed value order (only signed compare supported)
            Vector256<sbyte> toSignedV = SetAllTo(128);

            // Look for StartObject, StartArray, EndObject, EndArray
            Vector256<sbyte> cutoffV = SetAllTo(0xFB);
            cutoffV = Avx2.Subtract(cutoffV, toSignedV);

            fixed (byte* contentPtr = &content[0])
            {
                int fullBlockLength = content.Length - 31;

                int i;
                for (i = 0; i < fullBlockLength; i += 32)
                {
                    // Load a vector of content and convert to signed
                    Vector256<sbyte> contentV = Unsafe.ReadUnaligned<Vector256<sbyte>>(&contentPtr[i]);
                    Vector256<sbyte> contentSV = Avx2.Subtract(contentV, toSignedV);

                    // Find bytes greater than the cutoff
                    Vector256<sbyte> result = Avx2.CompareGreaterThan(contentSV, cutoffV);

                    // Convert the byte mask of matches to a bit mask
                    uint bits = unchecked((uint)Avx2.MoveMask(result));

                    // Find the index of the first container
                    int index = TrailingZeroCount(bits);

                    // If container found, ...
                    if (index < 32)
                    {
                        // Calculate new depth
                        byte container = content[i + index];
                        depth += (container & 0x2) - 1;

                        // If zero, return where
                        if (depth == 0) return (i + index);

                        // Otherwise, set to continue scanning right after match (index + 1)
                        i += (index + 1) - 32;
                    }
                }

                return i + SkipCs(content.Slice(i), ref depth);
            }
        }

        private static Vector256<sbyte> SetAllTo(byte value)
        {
            // Make 32 copies of value
            sbyte* _loader = stackalloc sbyte[32];
            for (int i = 0; i < 32; ++i)
            {
                _loader[i] = unchecked((sbyte)value);
            }

            // Load into a Vector256 and return
            return Unsafe.Read<Vector256<sbyte>>(_loader);
        }

        private const uint DeBruijnSequence = 0x077CB531U;
        private static readonly int[] DeBruijnTrailingZeroCount =
        {
            0, 1, 28, 2, 29, 14, 24, 3, 30, 22, 20, 15, 25, 17, 4, 8,
            31, 27, 13, 23, 21, 19, 16, 7, 26, 12, 18, 6, 11, 5, 10, 9
        };

        private static int TrailingZeroCount(uint bits)
        {
            // Should be this, but it's not in the JIT as of .NET Core 2.1
            //return unchecked((int)Bmi1.TrailingZeroCount(bits));

            // http://graphics.stanford.edu/~seander/bithacks.html#ZerosOnRightMultLookup
            if (bits == 0) return 32;
            return DeBruijnTrailingZeroCount[(unchecked((uint)((int)bits & -(int)bits)) * DeBruijnSequence) >> 27];
        }
    }
}
