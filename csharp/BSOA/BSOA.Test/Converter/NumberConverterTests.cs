using BSOA.Column;
using BSOA.Converter;
using System;
using Xunit;

namespace BSOA.Test
{
    public class NumberConverterTests
    {
        [Fact]
        public void NumberColumn_Upconvert_Basics()
        {
            UpconvertTest(() => new ConvertingColumn<int, byte>(new NumberColumn<byte>(0), ByteConverter.Instance));
            UpconvertTest(() => new ConvertingColumn<int, sbyte>(new NumberColumn<sbyte>(0), SByteConverter.Instance));
            UpconvertTest(() => new ConvertingColumn<int, short>(new NumberColumn<short>(0), ShortConverter.Instance));
            UpconvertTest(() => new ConvertingColumn<int, ushort>(new NumberColumn<ushort>(0), UShortConverter.Instance));
        }

        private void UpconvertTest(Func<IColumn<int>> ctor)
        {
            // Test column with values from 0-127 only, to fit in all smaller types
            Column.Basics<int>(ctor, defaultValue: 0, otherValue: 5, (i) => i % 128);
        }

        [Fact]
        public void NumberConverter_LimitChecks()
        {
            LimitTest<int, byte>(ByteConverter.Instance, byte.MinValue, (int)(byte.MinValue) - 1, byte.MaxValue, (int)(byte.MaxValue) + 1);
            LimitTest<int, sbyte>(SByteConverter.Instance, sbyte.MinValue, (int)(sbyte.MinValue) - 1, sbyte.MaxValue, (int)(sbyte.MaxValue) + 1);
            LimitTest<int, short>(ShortConverter.Instance, short.MinValue, (int)(short.MinValue) - 1, short.MaxValue, (int)(short.MaxValue) + 1);
            LimitTest<int, ushort>(UShortConverter.Instance, ushort.MinValue, (int)(ushort.MinValue) - 1, ushort.MaxValue, (int)(ushort.MaxValue) + 1);
        }

        private void LimitTest<T, U>(IConverter<T, U> converter, T minSafe, T tooSmall, T maxSafe, T tooBig)
        {
            // Verify round trip for barely-in-bounds values
            Assert.Equal(minSafe, converter.Convert(converter.Convert(minSafe)));
            Assert.Equal(maxSafe, converter.Convert(converter.Convert(maxSafe)));

            // Verify throws ArgumentOutOfRange for values just out of range
            Assert.Throws<ArgumentOutOfRangeException>(() => converter.Convert(tooSmall));
            Assert.Throws<ArgumentOutOfRangeException>(() => converter.Convert(tooBig));
        }
    }
}
