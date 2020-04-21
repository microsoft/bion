using BSOA.Test.Components;
using System;
using System.Collections.Generic;
using Xunit;

namespace BSOA.Test
{
    public class Column
    {
        public static void Basics<T>(Func<IColumn<T>> builder, T defaultValue, T otherValue, Func<int, T> valueProvider)
        {
            IColumn<T> column = builder();
            List<T> expected = new List<T>();

            // Empty behavior
            Assert.Equal(0, column.Count);
            Assert.Equal(defaultValue, column[0]);
            Assert.Equal(defaultValue, column[10]);
            Assert.Equal(0, column.Count);

            // Append values
            for (int i = 0; i < 50; ++i)
            {
                T value = valueProvider(i);
                column[i] = value;
                expected.Add(value);
            }

            // Verify count, values, indexer, enumerators
            Assert.Equal(expected.Count, column.Count);
            ReadOnlyList.VerifySame<T>(expected, column);
            
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

            // Verify old values were kept, middle defaulted, last one set
            for (int i = 0; i < column.Count; ++i)
            {
                T value = (i < 50 || i == 1024 ? valueProvider(i) : defaultValue);
                Assert.Equal(value, column[i]);
            }

            // Verify serialization round trip via all current serialization mechanisms
            ReadOnlyList.VerifySame(column, BinarySerializable.RoundTrip(column, builder));
            ReadOnlyList.VerifySame(column, TreeSerializer.RoundTrip(column, builder, TreeFormat.Binary));
            ReadOnlyList.VerifySame(column, TreeSerializer.RoundTrip(column, builder, TreeFormat.Json));
            
            // Verify column is properly skippable (required to allow flexible file format schema)
            TreeSerializer.VerifySkip(column, TreeFormat.Binary);
            TreeSerializer.VerifySkip(column, TreeFormat.Json);

            // Verify original values are still there post-serialization (ensure column not corrupted by serialization)
            for (int i = 0; i < column.Count; ++i)
            {
                T value = (i < 50 || i == 1024 ? valueProvider(i) : defaultValue);
                Assert.Equal(value, column[i]);
            }

            // Verify clear resets count and that previously set values are back to default if accessed
            column.Clear();
            Assert.Equal(0, column.Count);
            Assert.Equal(defaultValue, column[0]);
            Assert.Equal(defaultValue, column[1]);

            // Verify indexer range check (< 0 only; columns auto-size for bigger values)
            Assert.Throws<IndexOutOfRangeException>(() => column[-1]);
        }
    }
}
