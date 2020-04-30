using System;

namespace BSOA.Column
{
    public interface IConverter<T, U>
    {
        U Convert(T value);
    }

    public class ByteConverter : IConverter<byte, long>
    {
        public long Convert(byte value)
        {
            return value;
        }
    }
}
