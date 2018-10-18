using System;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;

namespace Bion.Vector
{
    public static unsafe class ByteVector
    {
        public static int IndexOf(byte value, byte[] content, int index, int endIndex)
        {
            if (endIndex - index > 128 && Avx2.IsSupported)
            {
                return IndexOfAvx(value, content, index, endIndex);
            }
            else
            {
                return IndexOfCs(value, content, index, endIndex);
            }
        }

        private static int IndexOfCs(byte value, byte[] content, int index, int endIndex)
        {
            int i;
            for (i = index; i < endIndex; ++i)
            {
                if (content[i] == value) { return i; }
            }

            return -1;
        }

        private static int IndexOfAvx(byte value, byte[] content, int index, int endIndex)
        {
            Vector256<sbyte> toFindV = SetAllTo(value);

            fixed (byte* contentPtr = &content[0])
            {
                int fullBlockLength = endIndex - 32;

                int i;
                for (i = index; i < fullBlockLength; i += 32)
                {
                    // Load a vector of content
                    Vector256<sbyte> contentV = Unsafe.ReadUnaligned<Vector256<sbyte>>(&contentPtr[i]);

                    // Make a mask of matches
                    Vector256<sbyte> matchV = Avx2.CompareEqual(contentV, toFindV);

                    // Convert to a bit vector
                    uint matchBits = unchecked((uint)Avx2.MoveMask(matchV));
                    if(matchBits != 0)
                    {
                        // Not Yet Supported [.NET Core 2.1]
                        //return i + (int)Lzcnt.LeadingZeroCount(matchBits);

                        return IndexOfCs(value, content, i, i + 32);
                    }
                }

                return IndexOfCs(value, content, i, endIndex);
            }
        }

        public static int Skip(byte[] content, int index, int endIndex, ref int depth)
        {
            if (endIndex - index > 128 && Avx2.IsSupported)
            {
                return SkipAvx(content, index, endIndex, ref depth);
            }
            else
            {
                return SkipCs(content, index, endIndex, ref depth);
            }
        }

        private static int SkipCs(byte[] content, int index, int endIndex, ref int depth)
        {
            int i;
            for (i = index; i < endIndex; ++i)
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

        private static int SkipAvx(byte[] content, int index, int endIndex, ref int depth)
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
                int fullBlockLength = endIndex - 32;

                int i;
                for (i = index; i < fullBlockLength; i += 32)
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
                        int inner = SkipCs(content, index, index + 32, ref depth);
                        if (index < index + 32) return inner;
                    }
                    else
                    {
                        // Otherwise, it's safe to continue looking for the end
                        depth = depth - endCount + startCount;
                    }
                }

                return SkipCs(content, i, endIndex, ref depth);
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
