using System;
using System.Collections.Generic;
using Xunit;

namespace BSOA.Test
{
    public static class IList
    {
        public static void Basics<T>(IList<T> row, Func<int, T> valueProvider)
        {
            List<T> expected = new List<T>();
            row.Clear();

            // Set up 5 values each
            for (int i = 0; i < 5; ++i)
            {
                T value = valueProvider(i);
                row.Add(value);
                expected.Add(value);
            }

            // Verify inner lists
            ReadOnlyList.VerifySame(expected, row);

            // Find a value not in this particular set
            T notInExpected = default(T);
            for (int i = 5; i < 20; ++i)
            {
                notInExpected = valueProvider(i);
                if (!expected.Contains(notInExpected)) { break; }
            }

            // Lists should not report ReadOnly
            Assert.False(row.IsReadOnly);

            // Verify count correct
            Assert.Equal(expected.Count, row.Count);

            for (int i = 0; i < expected.Count; ++i)
            {
                // Check Contains
                Assert.True(row.Contains(expected[i]) == true);

                // Test IndexOf; verify indices are mapped back to [0, count-1]
                Assert.Equal(i, row.IndexOf(expected[i]));
            }

            Assert.False(row.Contains(notInExpected));
            Assert.Equal(-1, row.IndexOf(notInExpected));

            // Test CopyTo
            T[] other = new T[row.Count + 1];
            row.CopyTo(other, 1);

            for (int i = 0; i < expected.Count; ++i)
            {
                Assert.Equal(expected[i], other[i + 1]);
            }

            // CopyTo bounds
            Assert.Throws<ArgumentNullException>(() => row.CopyTo(null, 0));
            Assert.Throws<ArgumentException>(() => row.CopyTo(other, 2));
            Assert.Throws<ArgumentOutOfRangeException>(() => row.CopyTo(other, -1));

            // Change an item inline; verify changed, array not moved, not expandable
            T firstValue = expected[0];
            row[0] = notInExpected;
            expected[0] = notInExpected;
            ReadOnlyList.VerifySame(expected, row);

            row[0] = firstValue;
            expected[0] = firstValue;

            // Append an item; verify appended, count changed
            T ten = valueProvider(10);
            row.Add(ten);
            expected.Add(ten);
            ReadOnlyList.VerifySame(expected, row);

            // Test Remove, RemoveAt
            Assert.False(row.Remove(notInExpected));
            Assert.True(row.Remove(ten));
            expected.Remove(ten);
            ReadOnlyList.VerifySame(expected, row);

            // Test RemoveAt bounds checks
            Assert.Throws<IndexOutOfRangeException>(() => row.RemoveAt(-1));
            Assert.Throws<IndexOutOfRangeException>(() => row.RemoveAt(row.Count));

            // Test Insert, RemoveAt
            row.Insert(0, notInExpected);
            expected.Insert(0, notInExpected);
            ReadOnlyList.VerifySame(expected, row);

            row.RemoveAt(0);
            expected.RemoveAt(0);
            ReadOnlyList.VerifySame(expected, row);

            row.Insert(2, ten);
            expected.Insert(2, ten);
            ReadOnlyList.VerifySame(expected, row);

            // Test Insert bounds checks
            Assert.Throws<IndexOutOfRangeException>(() => row.Insert(-1, notInExpected));
            Assert.Throws<IndexOutOfRangeException>(() => row.Insert(row.Count, notInExpected));

            // Clear; verify empty, read-only static instance
            row.Clear();
            expected.Clear();
            Assert.Empty(row);

            // Test Add until resize required; verify old elements copied to larger array properly
            for (int i = 0; i < 50; ++i)
            {
                T value = valueProvider(i);
                row.Add(value);
                expected.Add(value);
            }

            ReadOnlyList.VerifySame(expected, row);
        }
    }
}
