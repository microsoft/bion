namespace BSOA.Converter
{
    /// <summary>
    ///  IConverters provide conversion between T and U.
    /// </summary>
    public interface IConverter<T, U>
    {
        U Convert(T value);
        T Convert(U value);
    }
}
