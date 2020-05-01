namespace BSOA.Converter
{
    /// <summary>
    ///  IConverters provide conversion from T to U.
    /// </summary>
    /// <typeparam name="T">Type converter can convert from</typeparam>
    /// <typeparam name="U">Type converter can convert to</typeparam>
    public interface IConverter<T, U>
    {
        U Convert(T value);
    }
}
