// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Column;

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

            // Support both DateTime.MinValue and DateTime.MinValue.ToUniversalTime() as successfully roundtripped defaults
            defaultValue = DateTime.MinValue;
            Column.Basics<DateTime>(
                () => new DateTimeColumn(defaultValue),
                defaultValue,
                otherValue,
                (i) => otherValue.AddDays(-i)
            );
        }
    }
}
