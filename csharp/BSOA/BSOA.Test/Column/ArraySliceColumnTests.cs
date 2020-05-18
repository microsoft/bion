using BSOA.Column;
using System;
using System.Linq;
using Xunit;

namespace BSOA.Test
{
    public class ArraySliceColumnTests
    {
        [Fact]
        public void ArraySliceColumn_Basics()
        {
            Func<int, ArraySlice<byte>> builder = (index) => new ArraySlice<byte>(new byte[] { (byte)index, (byte)(index + 1) });
            Column.Basics<ArraySlice<byte>>(() => new ArraySliceColumn<byte>(), ArraySlice<byte>.Empty, new ArraySlice<byte>(new byte[] { 50, 60, 70 }), builder);

            ArraySliceColumn<int> column = new ArraySliceColumn<int>();

            int sum = 0;
            for(int i = 0; i < 10; ++i)
            {
                int[] values = Enumerable.Range(10 * i, 10).ToArray();
                column[i] = new ArraySlice<int>(values);
                sum += values.Sum();
            }

            int[] big = Enumerable.Range(0, 2048).ToArray();
            column[10] = new ArraySlice<int>(big);
            sum += big.Sum();

            int actual = 0;
            column.ForEach((slice) => actual += slice.Sum());
            Assert.Equal(sum, actual);
        }
    }
}
