// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Bion.Core
{
    public static unsafe class Hashing
    {
        private const ulong m = 0xc6a4a7935bd1e995;
        private const int r = 47;

        // Murmur2 64-bit for Span<T>; see https://github.com/aappleby/smhasher. MIT License.
        public static unsafe ulong Murmur2(byte[] array, int index, int length, ulong seed)
        {
            ulong h = seed ^ unchecked(((ulong)length * m));

            int blockLength = length / 8;
            if (blockLength > 0)
            {
                fixed (byte* spanPtr = &array[index])
                {
                    ulong* blockPtr = (ulong*)spanPtr;
                    for (int i = 0; i < blockLength; ++i)
                    {
                        ulong k = blockPtr[i];

                        k *= m;
                        k ^= k >> r;
                        k *= m;

                        h ^= k;
                        h *= m;
                    }
                }
            }

            if ((length & 0x7) != 0)
            {
                for (int i = 8 * blockLength, shift = 0; i < length; ++i, shift += 8)
                {
                    h ^= (ulong)array[index + i] << shift;
                }

                h *= m;
            }

            h ^= h >> r;
            h *= m;
            h ^= h >> r;

            return h;
        }

        public static ulong Murmur2(ulong value, ulong seed)
        {
            ulong h = seed ^ unchecked((ulong)8 * m);
            h ^= value;
            h ^= h >> r;
            h *= m;
            h ^= h >> r;

            return h;
        }
    }
}
