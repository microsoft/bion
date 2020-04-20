using System;
using Xunit;

namespace BSOA.Test
{
    public class DateTimeColumnTests
    {
        [Fact]
        public void DateTimeColumn_Basics()
        {
            DateTime defaultValue = DateTime.MinValue.ToUniversalTime();
            DateTime otherValue = DateTime.UtcNow;

            Column.Basics<DateTime>(
                () => new DateTimeColumn(defaultValue),
                defaultValue,
                otherValue,
                (i) => otherValue.AddDays(-i)
            );
        }
    }
}
