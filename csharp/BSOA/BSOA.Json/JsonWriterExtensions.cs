using System.Collections;

namespace Newtonsoft.Json
{
    public static class JsonWriterExtensions
    {
        public static void Write<TItem>(this JsonWriter writer, string propertyName, TItem item, TItem defaultValue)
        {
            if (!object.Equals(item, defaultValue) && !IsEmpty(item))
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        private static bool IsEmpty<T>(T item)
        {
            ICollection collection = item as ICollection;
            return (collection?.Count == 0);
        }
    }
}
