using BSOA.Demo.Model;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace RegionDemo.Model_Ext
{
    public static class Converters
    {
        public static void ReadObject<TItem, TRoot>(JsonReader reader, TRoot root, TItem item, Dictionary<string, Action<JsonReader, TRoot, TItem>> setters)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                reader.Read();
            }

            while (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = (string)reader.Value;
                reader.Read();

                if (!setters.TryGetValue(propertyName, out var setter))
                {
                    throw new NotImplementedException($"Unknown property ArtifactContent.{propertyName}. Stopping.");
                }

                setter(reader, root, item);

                reader.Read();
            }
        }

        public static void ReadList<T>(JsonReader reader, TinyLog root, IList<T> list, Func<JsonReader, TinyLog, T> readItem)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                reader.Read();
            }

            while (reader.TokenType != JsonToken.EndArray)
            {
                list.Add(readItem(reader, root));
                reader.Read();
            }
        }
    }
}
