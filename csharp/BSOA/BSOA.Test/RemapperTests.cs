using System;
using System.Linq;
using Xunit;

namespace BSOA.Test
{
    public class RemapperTests
    {
        [Fact]
        public void Remapper_Basics()
        {
            IRemapper<int> remapper = RemapperFactory.Build<int>();
            ArraySlice<int> sample = new ArraySlice<int>(Enumerable.Range(2, 50).ToArray());
            
            // Verify RemoveValues finds 0 and 1 unused in array with 2-52
            BitVector unusedItems = new BitVector(true, 52);
            remapper.RemoveValues(sample, unusedItems);
            Assert.Equal("0, 1", string.Join(", ", unusedItems));

            // Verify RemapAbove changes 50 and 51 to 100, 101 when instructed
            remapper.RemapAbove(sample, 50, new int[] { 100, 101 });
            Assert.Equal(49, sample[47]);
            Assert.Equal(100, sample[48]);
            Assert.Equal(101, sample[49]);

            // Verify factory throws for unsupported types
            Assert.Throws<NotImplementedException>(() => RemapperFactory.Build<ulong>());
        }
    }
}
