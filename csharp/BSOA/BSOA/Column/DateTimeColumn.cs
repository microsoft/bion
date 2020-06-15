// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace BSOA.Column
{
    /// <summary>
    ///  DateTimeColumn implements IColumn for DateTime on top of a NumberColumn&lt;long&gt;
    /// </summary>
    public class DateTimeColumn : WrappingColumn<DateTime, long>
    {
        public DateTimeColumn(DateTime defaultValue) : base(new NumberColumn<long>(Convert(defaultValue)))
        { }

        public override DateTime this[int index] 
        {
            get => Convert(Inner[index]);
            set => Inner[index] = Convert(value);
        }

        private static DateTime Convert(long value)
        {
            return new DateTime(value, DateTimeKind.Utc);
        }

        private static long Convert(DateTime value)
        {
            return value.ToUniversalTime().Ticks;
        }
    }
}
