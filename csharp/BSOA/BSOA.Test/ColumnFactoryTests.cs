// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

using Xunit;

namespace BSOA.Test
{
    public class ColumnFactoryTests
    {
        [Fact]
        public void ColumnFactory_Build()
        {
            AssertBuild<string>(null);
            AssertBuild<Uri>(null);
            AssertBuild<DateTime>(DateTime.UtcNow);
            AssertBuild<bool>(true);

            AssertBuild<byte>((byte)1);
            AssertBuild<sbyte>((sbyte)1);
            AssertBuild<ushort>((ushort)1);
            AssertBuild<short>((short)1);
            AssertBuild<uint>((uint)1);
            AssertBuild<int>((int)1);
            AssertBuild<ulong>((ulong)1);
            AssertBuild<long>((long)1);
            AssertBuild<float>((float)1);
            AssertBuild<double>((double)1);
            AssertBuild<char>((char)1);

            Assert.Throws<NotImplementedException>(() => ColumnFactory.Build(typeof(DayOfWeek), DayOfWeek.Sunday));

            IColumn<IList<string>> listColumn = (IColumn<IList<string>>)ColumnFactory.Build(typeof(IList<string>), null);
            Assert.NotNull(listColumn);

            IColumn<IDictionary<string, string>> dictionaryColumn = (IColumn<IDictionary<string, string>>)(ColumnFactory.Build(typeof(IDictionary<string, string>), null));
            Assert.NotNull(dictionaryColumn);
        }

        private void AssertBuild<T>(object defaultValue)
        {
            IColumn column = ColumnFactory.BuildTyped<T>((T)defaultValue);
            Assert.NotNull(column);
            Assert.True(column is IColumn<T>);

            if (defaultValue != null)
            {
                column = ColumnFactory.Build(typeof(T), null);
                Assert.NotNull(column);
                Assert.True(column is IColumn<T>);
            }
        }
    }
}
