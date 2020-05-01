using BSOA.Converter;
using System;

namespace BSOA.Column
{
    /// <summary>
    ///  UriColumn implements IColumn for Uri on top of a StringColumn
    /// </summary>
    public class UriColumn : ConvertingColumn<Uri, string>
    {
        public UriColumn() : base(new StringColumn(), UriConverter.Instance, UriConverter.Instance)
        { }
    }
}
