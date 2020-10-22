// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Model;

namespace BSOA.Extensions
{
    public static class ArrayExtensions
    {
        private const int MinimumSize = 32;

        public static void ResizeTo<T>(ref T[] array, int neededSize, T defaultValue = default(T), int minSize = MinimumSize)
        {
            int currentLength = (array?.Length ?? 0);

            // Allocate new array (at least 50% growth)
            int newLength = Math.Max(minSize, Math.Max(neededSize, (currentLength + currentLength / 2)));
            T[] newArray = new T[newLength];

            // Copy existing values (if any)
            if (currentLength > 0)
            {
                array.CopyTo(newArray, 0);
            }

            // Fill new space with desired default
            if (!defaultValue.Equals(default(T)))
            {
                for (int i = currentLength; i < newLength; ++i)
                {
                    newArray[i] = defaultValue;
                }
            }

            array = newArray;
        }

        /// <summary>
        ///  Binary Search for a particular key, where there is an array of keys
        ///  but we're searching a range of an indices array pointing to those keys.
        /// </summary>
        /// <remarks>
        ///  See GenericArraySortHelper.BinarySearch at https://referencesource.microsoft.com; implementation closely matched.
        ///  Added because .NET has Sort() with an indirection array, but not BinarySearch().
        /// </remarks>
        /// <typeparam name="T">Type of values being searched</typeparam>
        /// <param name="indices">Array containing indices to values to search</param>
        /// <param name="index">Index in indices array of first position to search</param>
        /// <param name="length">Number of elements in indices array to check</param>
        /// <param name="values">Array containing values pointed to by indices</param>
        /// <param name="value">Value to find</param>
        /// <returns>Index where values[indices[index]] == value, or bitwise complement of first index larger than value.</returns>
        public static int IndirectBinarySearch<T>(int[] indices, int index, int length, IReadOnlyList<T> values, T value, IComparer<T> comparer)
        {
            int lo = index;
            int hi = index + length - 1;
            while (lo <= hi)
            {
                int mid = (lo + ((hi - lo) >> 1));
                int valueIndex = indices[mid];
                T midValue = values[valueIndex];

                int cmp = comparer.Compare(midValue, value);
                if (cmp == 0)
                {
                    return mid;
                }

                if (cmp < 0)
                {
                    // value > midValue
                    lo = mid + 1;
                }
                else
                {
                    // value < midValue
                    hi = mid - 1;
                }
            }

            // Value not found
            return ~lo;
        }
    }
}
