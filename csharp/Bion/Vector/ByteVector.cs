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
            if(content.Length > 128 && Avx2.IsSupported)
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

            for(int i = 0; i < content.Length; ++i)
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

        private static Vector256<sbyte> SetAllTo(byte value)
        {
            // Make 32 copies of value
            sbyte* _loader = stackalloc sbyte[32];
            for(int i = 0; i < 32; ++i)
            {
                _loader[i] = unchecked((sbyte)value);
            }

            // Load into a Vector256 and return
            return Unsafe.Read<Vector256<sbyte>>(_loader);
        }
    }
}
