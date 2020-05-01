using System;

namespace BSOA.Converter
{
    /// <summary>
    ///  DateTimeConverter provides conversion between DateTime and long.
    /// </summary>
    public class DateTimeConverter : IConverter<long, DateTime>, IConverter<DateTime, long>
    {
        public static DateTimeConverter Instance = new DateTimeConverter();

        private DateTimeConverter()
        { }

        public DateTime Convert(long value)
        {
            return new DateTime(value, DateTimeKind.Utc);
        }

        public long Convert(DateTime value)
        {
            return value.ToUniversalTime().Ticks;
        }
    }
}
