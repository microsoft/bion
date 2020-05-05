using BSOA.Column;
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
            Assert.True(column.Empty);
            Assert.Equal(0, column.Count);
            Assert.Equal(defaultValue, column[0]);
            Assert.Equal(defaultValue, column[10]);
            Assert.Equal(0, column.Count);

            // Empty roundtrip works
            ReadOnlyList.VerifySame(expected, TreeSerializer.RoundTrip(column, builder, TreeFormat.Binary));
            ReadOnlyList.VerifySame(expected, TreeSerializer.RoundTrip(column, builder, TreeFormat.Json));

            // Empty trim works
            column.Trim();

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

            // Append a default value; verify the count tracks it correctly
            column[1025] = defaultValue;
            Assert.Equal(1026, column.Count);
            Assert.Equal(defaultValue, column[1025]);

            // Verify serialization round trip via all current serialization mechanisms
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

            // Swap two non-default values, verify swapped, swap back
            column.Swap(10, 20);
            Assert.Equal(valueProvider(10), column[20]);
            Assert.Equal(valueProvider(20), column[10]);
            column.Swap(10, 20);
            Assert.Equal(valueProvider(10), column[10]);
            Assert.Equal(valueProvider(20), column[20]);

            // Swap a default with a non-default value, verify swapped, swap back
            column.Swap(30, 60);
            Assert.Equal(valueProvider(30), column[60]);
            Assert.Equal(defaultValue, column[30]);
            column.Swap(30, 60);
            Assert.Equal(valueProvider(30), column[30]);
            Assert.Equal(defaultValue, column[60]);

            // Verify RemoveFromEnd for only default values works
            column.RemoveFromEnd(column.Count - 100);
            Assert.Equal(100, column.Count);
            Assert.Equal(defaultValue, column[100]);

            // Verify RemoveFromEnd down to non-default values works
            column.RemoveFromEnd(100 - 10);
            Assert.Equal(10, column.Count);

            for (int i = 0; i < 100; ++i)
            {
                T value = (i < 10 ? valueProvider(i) : defaultValue);
                Assert.Equal(value, column[i]);
            }

            // Verify Trim doesn't throw
            column.Trim();

            // Verify clear resets count and that previously set values are back to default if accessed
            Assert.False(column.Empty);
            column.Clear();
            Assert.True(column.Empty);
            Assert.Equal(0, column.Count);
            Assert.Equal(defaultValue, column[0]);
            Assert.Equal(defaultValue, column[1]);

            // Add one default value (inner array may still not be allocated), then try RemoveFromEnd
            column[0] = defaultValue;
            column.RemoveFromEnd(1);
            Assert.Equal(0, column.Count);

            // Verify indexer range check (< 0 only; columns auto-size for bigger values)
            Assert.Throws<IndexOutOfRangeException>(() => column[-1]);
        }
    }
}
