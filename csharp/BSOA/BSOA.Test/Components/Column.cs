using System;
using Xunit;

namespace BSOA.Test
{
    public class Column
    {
        public static void Basics<T>(Func<IColumn<T>> builder, T defaultValue, T otherValue, Func<int, T> valueProvider)
        {
            IColumn<T> column = builder();

            // Empty behavior
            Assert.Equal(0, column.Count);
            Assert.Equal(defaultValue, column[0]);
            Assert.Equal(defaultValue, column[10]);
            Assert.Equal(0, column.Count);

            // Append values
            for (int i = 0; i < 10; ++i)
            {
                column[i] = valueProvider(i);
            }

            // Check for valid count and set values
            Assert.Equal(10, column.Count);
            for (int i = 0; i < 10; ++i)
            {
                Assert.Equal(valueProvider(i), column[i]);
            }

            // Change existing value
            column[1] = otherValue;
            Assert.Equal(otherValue, column[1]);

            // Set value back to default, back to non-default
            column[1] = defaultValue;
            Assert.Equal(defaultValue, column[1]);
            column[1] = valueProvider(1);
            Assert.Equal(valueProvider(1), column[1]);

            // Append so resize is required
            column[1024] = valueProvider(1024);

            // Verify previously set values were kept (array copy on resize)
            for (int i = 0; i < 10; ++i)
            {
                Assert.Equal(valueProvider(i), column[i]);
            }

            // Verify new unset values were defaulted
            for (int i = 10; i < 1024; ++i)
            {
                Assert.Equal(defaultValue, column[i]);
            }

            // Verify last set value was set
            Assert.Equal(valueProvider(1024), column[1024]);

            // Roundtrip via MemoryStream
            IColumn<T> roundTripped = BinarySerializable.RoundTrip<IColumn<T>>(column, builder);

            // Verify Count and Values match
            Assert.Equal(column.Count, roundTripped.Count);
            for (int i = 0; i < column.Count; ++i)
            {
                Assert.Equal(column[i], roundTripped[i]);
            }

            // Verify indexer range check (< 0 only; columns auto-size for bigger values)
            Assert.Throws<IndexOutOfRangeException>(() => column[-1]);
        }
    }
}
