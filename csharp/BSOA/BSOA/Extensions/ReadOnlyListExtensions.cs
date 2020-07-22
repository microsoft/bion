// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace BSOA.Extensions
{
    public static class ReadOnlyListExtensions
    {
        /// <summary>
        ///  ForEachReverse calls an action on each list item in reverse order.
        ///  It is used to clean up BSOA instances, which can be removed more easily
        ///  if they are the last index in a table.
        /// </summary>
        /// <typeparam name="T">Type of List items</typeparam>
        /// <param name="list">List to act on</param>
        /// <param name="action">Action to take for each list item</param>
        public static void ForEachReverse<T>(this IList<T> list, Action<T> action)
        {
            if (list != null)
            {
                for (int i = list.Count - 1; i >= 0; --i)
                {
                    action(list[i]);
                }
            }
        }

        public static bool AreEqual<T>(this IReadOnlyList<T> left, IReadOnlyList<T> right)
        {
            if (left == null || right == null) { return left == null && right == null; }

            if (left.Count != right.Count) { return false; }
            for (int i = 0; i < left.Count; ++i)
            {
                if (!object.Equals(left[i], right[i])) { return false; }
            }

            return true;
        }

        public static int GetHashCode<T>(this IReadOnlyList<T> me)
        {
            int hashCode = 17;

            for (int i = 0; i < me.Count; ++i)
            {
                hashCode = unchecked(hashCode * 31) + me[i]?.GetHashCode() ?? 0;
            }

            return hashCode;
        }
    }
}
