// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using BSOA.Column;
using BSOA.Model;
using BSOA.Test.Components;

using Xunit;

namespace BSOA.Test
{
    public class Column
    {
        public static void Basics<T>(Func<IColumn<T>> builder, T defaultValue, T otherValue, Func<int, T> valueProvider)
        {
            IColumn<T> column = builder();
            List<T> expected = new List<T>();

            // ICollection basics
            Assert.False(column.IsReadOnly);
            Assert.False(column.IsSynchronized);
            Assert.Null(column.SyncRoot);

            Assert.Equal(typeof(T), column.Type);

            // Empty behavior
            Assert.Equal(0, column.Count);
            Assert.Equal(defaultValue, column[0]);
            Assert.Equal(defaultValue, column[10]);
            Assert.Equal(0, column.Count);

            // Empty roundtrip works
            CollectionReadVerifier.VerifySame(expected, TreeSerializer.RoundTrip(column, builder, TreeFormat.Binary));
            CollectionReadVerifier.VerifySame(expected, TreeSerializer.RoundTrip(column, builder, TreeFormat.Json));

            // Empty trim works
            column.Trim();

            // Append values
            for (int i = 0; i < 50; ++i)
            {
                T value = valueProvider(i);
                column.Add(value);
                expected.Add(value);
            }

            // Verify count, values, indexer, enumerators
            Assert.Equal(expected.Count, column.Count);
            CollectionReadVerifier.VerifySame<T>(expected, column);

            // Verify item type supports equatability (needed for IndexOf, Contains to work)
            if (otherValue != null)
            {
                Assert.True(otherValue.Equals(otherValue));
                Assert.Equal(otherValue.GetHashCode(), otherValue.GetHashCode());
                Assert.False(otherValue.Equals(defaultValue));
                Assert.NotEqual(otherValue.GetHashCode(), defaultValue?.GetHashCode() ?? 0);
                Assert.False(otherValue.Equals(1.45d));
            }

            // Contains / IndexOf
            T notInList = valueProvider(50);
            if (!expected.Contains(notInList))
            {
                Assert.DoesNotContain(notInList, column);
                Assert.Equal(-1, column.IndexOf(notInList));
            }

            Assert.Contains(valueProvider(1), column);
            Assert.Equal(1, column.IndexOf(valueProvider(1)));

            // CopyTo
            T[] other = new T[column.Count + 1];
            column.CopyTo(other, 1);
            Assert.Equal(column[0], other[1]);

            // CopyTo preconditions
            if (!Debugger.IsAttached)
            {
                Assert.Throws<ArgumentNullException>(() => column.CopyTo(null, 0));
                Assert.Throws<ArgumentException>(() => column.CopyTo(other, 2));
                Assert.Throws<ArgumentOutOfRangeException>(() => column.CopyTo(other, -1));
                Assert.Throws<ArgumentException>(() => column.CopyTo((Array)(new decimal[column.Count]), 0));
            }

            // CopyTo (untyped)
            other = new T[column.Count];
            column.CopyTo((Array)other, 0);
            CollectionReadVerifier.VerifySame(other, column, quick: true);

            // Verify multi-threaded reads return correct values (no read-only threading problems)
            Parallel.For(0, 8, (instance) =>
            {
                Random r = new Random(instance);
                for (int i = 0; i < column.Count * 20; ++i)
                {
                    int rowIndex = r.Next(column.Count);
                    Assert.Equal(expected[rowIndex], column[rowIndex]);
                }
            });

            // Change existing value
            column[1] = otherValue;
            Assert.Equal(otherValue, column[1]);

            // Set value back to default, back to non-default
            column[1] = defaultValue;
            Assert.Equal(defaultValue, column[1]);
            column[1] = valueProvider(1);
            Assert.Equal(valueProvider(1), column[1]);

            // Set to self
            column[1] = column[1];
            Assert.Equal(valueProvider(1), column[1]);

            // Add() without instance
            T item = column.Add();
            Assert.Equal(defaultValue, item);

            // Copy values via CopyItem
            column.CopyItem(1, column, 2);
            Assert.Equal(column[1], column[2]);
            column[1] = valueProvider(1);

            // CopyItem type checking
            if (!Debugger.IsAttached)
            {
                Assert.Throws<ArgumentException>(() => column.CopyItem(1, new NumberColumn<Decimal>(0.0m), 0));
            }

            // Append so resize is required
            column[100] = valueProvider(100);

            // Verify old values were kept, middle defaulted, last one set
            for (int i = 0; i < column.Count; ++i)
            {
                T value = (i < 50 || i == 100 ? valueProvider(i) : defaultValue);
                Assert.Equal(value, column[i]);
            }

            // Verify serialization round trip via all current serialization mechanisms
            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, builder, TreeFormat.Binary), quick: true);
            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, builder, TreeFormat.Json), quick: true);

            // Verify column is properly skippable (required to allow flexible file format schema)
            TreeSerializer.VerifySkip(column, TreeFormat.Binary);
            TreeSerializer.VerifySkip(column, TreeFormat.Json);

            // Verify original values are still there post-serialization (ensure column not corrupted by serialization)
            for (int i = 0; i < column.Count; ++i)
            {
                T value = (i < 50 || i == 100 ? valueProvider(i) : defaultValue);
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
            column.RemoveFromEnd(column.Count - 60);
            Assert.Equal(60, column.Count);
            Assert.Equal(defaultValue, column[60]);

            // Verify RemoveFromEnd down to non-default values works
            column.RemoveFromEnd(60 - 10);
            Assert.Equal(10, column.Count);

            for (int i = 0; i < 60; ++i)
            {
                T value = (i < 10 ? valueProvider(i) : defaultValue);
                Assert.Equal(value, column[i]);
            }

            // Append a default value big enough another resize would be required
            //  Verify the count is tracked correctly, previous items are initialized to default
            column[201] = defaultValue;
            Assert.Equal(202, column.Count);
            Assert.Equal(defaultValue, column[200]);

            // Verify serialization handles 'many defaults at end' properly
            CollectionReadVerifier.VerifySame(column, TreeSerializer.RoundTrip(column, builder, TreeFormat.Binary), quick: true);

            // Verify Trim doesn't throw
            column.Trim();

            // Verify clear resets count and that previously set values are back to default if accessed
            column.Clear();
            Assert.Equal(0, column.Count);
            Assert.Equal(defaultValue, column[0]);
            Assert.Equal(defaultValue, column[1]);

            // Add one default value (inner array may still not be allocated), then try RemoveFromEnd
            column[0] = defaultValue;
            column.RemoveFromEnd(1);
            Assert.Equal(0, column.Count);

            if (!Debugger.IsAttached)
            {
                // Verify indexer range check (< 0 only; columns auto-size for bigger values)
                Assert.Throws<IndexOutOfRangeException>(() => column[-1]);

                // Verify indexer range check (< 0 only; columns auto-size for bigger values)
                Assert.Throws<IndexOutOfRangeException>(() => column[-1] = defaultValue);

                // Verify Remove throws (not expected to be implemented)
                Assert.Throws<NotSupportedException>(() => column.Remove(defaultValue));
            }
        }
    }
}
