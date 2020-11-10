// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace BSOA.Extensions
{
    public static class EnumerableExtensions
    {
        public static void CopyTo<T>(this IEnumerable<T> source, int sourceCount, T[] array, int arrayIndex)
        {
            if (array == null) { throw new ArgumentNullException(nameof(array)); }
            if (arrayIndex < 0) { throw new ArgumentOutOfRangeException(nameof(arrayIndex)); }
            if (arrayIndex + sourceCount > array.Length) { throw new ArgumentException(nameof(arrayIndex)); }

            int next = arrayIndex;
            foreach (T item in source)
            {
                array[next++] = item;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
            {
                foreach (T item in source)
                {
                    action(item);
                }
            }
        }
    }
}
