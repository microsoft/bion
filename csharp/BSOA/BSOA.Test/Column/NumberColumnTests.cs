using BSOA.Column;
using System;
using System.Linq;
using Xunit;

namespace BSOA.Test
{
    public class NumberColumnTests
    {
        [Fact]
        public void NumberColumn_Basics()
        {
            NumberColumnTest<sbyte>(-10, 100, (i) => (sbyte)(i % sbyte.MaxValue));
            NumberColumnTest<byte>(250, 100, (i) => (byte)(i % byte.MaxValue));
         
            NumberColumnTest<short>(-20, 100, (i) => (short)(i % short.MaxValue));
            NumberColumnTest<ushort>(ushort.MaxValue, 100, (i) => (ushort)(i % ushort.MaxValue));

            NumberColumnTest<int>(int.MinValue, 100, (i) => i);
            NumberColumnTest<uint>(uint.MaxValue, 100, (i) => (uint)i);

            NumberColumnTest<long>(int.MinValue, 100, (i) => i);
            NumberColumnTest<ulong>(0, 100, (i) => (ulong)i);

            NumberColumnTest<float>(-5.5f, 124.5f, (i) => 0.5f * i);
            NumberColumnTest<double>(-5.5f, 124.5f, (i) => 0.5f * i);

            NumberColumn<int> column = new NumberColumn<int>(0);
            int sum = 0;
            for(int i = 0; i < 100; ++i)
            {
                column[i] = 2 * i;
                sum += 2 * i;
            }

            int actual = 0;
            column.ForEach((slice) => actual += slice.Sum());
            Assert.Equal(sum, actual);
        }

        private void NumberColumnTest<T>(T defaultValue, T otherValue, Func<int, T> valueProvider) where T : unmanaged, IEquatable<T>, IComparable<T>
        {
            Column.Basics<T>(() => new NumberColumn<T>(defaultValue), defaultValue, otherValue, valueProvider);
        }
    }
}
