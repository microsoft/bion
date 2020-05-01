using BSOA.Converter;
using System;

namespace BSOA.Column
{
    /// <summary>
    ///  DateTimeColumn implements IColumn for DateTime on top of a NumberColumn&lt;long&gt;
    /// </summary>
    public class DateTimeColumn : ConvertingColumn<DateTime, long>
    {
        public DateTimeColumn(DateTime defaultValue)
            : this(DateTimeConverter.Instance.Convert(defaultValue))
        { }

        private DateTimeColumn(long defaultValue) 
            : base(new NumberColumn<long>(defaultValue), DateTimeConverter.Instance, DateTimeConverter.Instance)
        { }
    }
}
