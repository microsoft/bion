using BSOA.Collections;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BSOA.Test.Collections
{
    public class BitVectorTests
    {
        [Fact]
        public void BitVectorTests_Basics()
        {
            BitVector vector = new BitVector(false, 260);
            HashSet<int> expected = new HashSet<int>();

            // Empty vector
            vector.Trim();
            VerifySame(expected, vector);
            Assert.Null(vector.Array);
            Assert.Equal(260, vector.Capacity);

            // Add every third item
            for (int i = 0; i < vector.Capacity; i += 3)
            {
                // True the first time
                Assert.Equal(expected.Add(i), vector.Add(i));

                // False when already present
                Assert.Equal(expected.Add(i), vector.Add(i));
            }

            VerifySame(expected, vector);

            // Remove every sixth item
            for (int i = 0; i < vector.Capacity; i += 6)
            {
                // True the first time
                Assert.Equal(expected.Remove(i), vector.Remove(i));

                // False when already present
                Assert.Equal(expected.Remove(i), vector.Remove(i));
            }

            VerifySame(expected, vector);

            // Contains
            for (int i = 0; i < vector.Capacity; ++i)
            {
                Assert.Equal(expected.Contains(i), vector.Contains(i));
            }

            // Verify untyped enumerator, Reset()
            List<int> expectedList = new List<int>(expected);
            IEnumerator untyped = ((IEnumerable)vector).GetEnumerator();
            int index = 0;
            while (untyped.MoveNext())
            {
                Assert.Equal(expectedList[index], untyped.Current);
                index++;
            }

            untyped.Reset();
            index = 0;
            while (untyped.MoveNext())
            {
                Assert.Equal(expectedList[index], untyped.Current);
                index++;
            }

            // Automatic growth w/default (need 126 ints = 4,001 / 32 rounded up)
            vector.Add(4000);
            expected.Add(4000);
            Assert.Equal(4001, vector.Capacity);
            Assert.Equal(((4001 + 31) / 32), vector.Array?.Length ?? 0);
            VerifySame(expected, vector);

            // Clear
            vector.Clear();
            Assert.Empty(vector);
            Assert.Equal(0, vector.Count);
            Assert.Equal(0, vector.Capacity);

            // UnionWith
            vector.UnionWith(expected);
            VerifySame(expected, vector);

            // ExceptWith
            vector.ExceptWith(expected);
            Assert.Empty(vector);
            Assert.Equal(0, vector.Count);

            // SetAll
            vector.Clear();
            vector[100] = true;

            vector.SetAll(true);
            Assert.Equal(vector.Capacity, vector.Count);
            Assert.False(vector[vector.Capacity]);

            vector.SetAll(false);
            Assert.Equal(0, vector.Count);
            Assert.False(vector[vector.Capacity]);
        }

        [Fact]
        public void BitVector_DefaultTrue()
        {
            // Default = true vector
            BitVector vector = new BitVector(true, 32);
            HashSet<int> expected = new HashSet<int>(Enumerable.Range(0, 32));

            Assert.Equal(32, vector.Count);
            VerifySame(expected, vector);

            // Clear every 4th value
            for(int i = 0; i < vector.Capacity; i += 4)
            {
                Assert.Equal(expected.Remove(i), vector.Remove(i));
            }

            VerifySame(expected, vector);

            // Automatic growth with default
            vector.Remove(100);
            Assert.Equal(101, vector.Capacity);
            Assert.False(vector[100]);

            expected.UnionWith(Enumerable.Range(32, 100 - 32));
            VerifySame(expected, vector);

            // SetAll
            vector.SetAll(true);
            Assert.Equal(vector.Capacity, vector.Count);
            Assert.True(vector[vector.Capacity]);

            vector.SetAll(false);
            Assert.Equal(0, vector.Count);
            Assert.True(vector[vector.Capacity]);
        }

        private void VerifySame(HashSet<int> expected, BitVector actual)
        {
            Assert.Equal(expected.Count, actual.Count);

            HashSet<int> scratch = new HashSet<int>(expected);
            scratch.SymmetricExceptWith(actual);
            Assert.Empty(scratch);
        }
    }
}
