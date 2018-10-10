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
            Vector256<sbyte> startOrEndCutoffV = SetAllTo(0xFB);
            startOrEndCutoffV = Avx2.Subtract(startOrEndCutoffV, toSignedV);

            Vector256<sbyte> startCutoffV = SetAllTo(0xFD);
            startCutoffV = Avx2.Subtract(startCutoffV, toSignedV);

            fixed (byte* contentPtr = &content[0])
            {
                int fullBlockLength = content.Length & ~31;

                int i;
                for (i = 0; i < fullBlockLength; i += 32)
                {
                    // Load a vector of content and convert to signed
                    Vector256<sbyte> contentV = Unsafe.ReadUnaligned<Vector256<sbyte>>(&contentPtr[i]);
                    Vector256<sbyte> contentSV = Avx2.Subtract(contentV, toSignedV);

                    // Find start containers only, convert to bit vector
                    Vector256<sbyte> startV = Avx2.CompareGreaterThan(contentSV, startCutoffV);
                    uint startBits = unchecked((uint)Avx2.MoveMask(startV));

                    // Find all start or end containers, convert to bit vector
                    Vector256<sbyte> startOrEndV = Avx2.CompareGreaterThan(contentSV, startOrEndCutoffV);
                    uint startOrEndBits = unchecked((uint)Avx2.MoveMask(startOrEndV));
                    uint endBits = (startOrEndBits & ~startBits);

                    // Count start and ends found
                    int startCount = Popcnt.PopCount(startBits);
                    int endCount = Popcnt.PopCount(endBits);

                    if(depth - endCount <= 0)
                    {
                        // If there are enough end containers here to reach the root, we have to check the order
                        int index = SkipCs(content.Slice(i, 32), ref depth);
                        if (index < 32) return i + index;
                    }
                    else
                    {
                        // Otherwise, it's safe to continue looking for the end
                        depth = depth - endCount + startCount;
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
    }
}
