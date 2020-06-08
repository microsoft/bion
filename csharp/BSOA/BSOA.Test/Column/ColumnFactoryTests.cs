using BSOA.Column;
using BSOA.Model;
using System;
using System.Collections.Generic;
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
            Assert.Throws<NotImplementedException>(() => ColumnFactory.Build<IList<int>>());

            IColumn<ColumnList<string>> column = (IColumn<ColumnList<string>>)ColumnFactory.Build(typeof(ColumnList<string>), null);
            Assert.NotNull(column);

            Assert.NotNull(ColumnFactory.Build<ColumnList<string>>());
            Assert.NotNull(ColumnFactory.Build<ColumnList<string>>(ColumnList<string>.Empty));
        }

        private void AssertBuild<T>(object defaultValue)
        {
            IColumn column = ColumnFactory.Build(typeof(T), defaultValue);
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
