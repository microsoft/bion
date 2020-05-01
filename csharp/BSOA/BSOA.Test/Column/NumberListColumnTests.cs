using BSOA.Column;
using System;
using Xunit;

namespace BSOA.Test
{
    public class NumberListColumnTests
    {
        [Fact]
        public void NumberListColumn_Basics()
        {
            Func<int, ArraySlice<byte>> builder = (index) => new ArraySlice<byte>(new byte[] { (byte)index, (byte)(index + 1) });
            Column.Basics<ArraySlice<byte>>(() => new NumberListColumn<byte>(), ArraySlice<byte>.Empty, new ArraySlice<byte>(new byte[] { 50, 60, 70 }), builder);
        }
    }
}
