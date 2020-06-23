using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace BSOA.Json.Converters
{
    public static class JsonToIDictionary<TKey, TValue>
    {
        public static IDictionary<TKey, TValue> Read<TRoot>(JsonReader reader, TRoot root, IDictionary<TKey, TValue> dictionary, Func<JsonReader, TRoot, TValue> readValue)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }

            reader.Expect(JsonToken.StartObject);
            reader.Read();

            IDictionary<TKey, TValue> result = (dictionary ?? new Dictionary<TKey, TValue>());

            while (reader.TokenType != JsonToken.EndObject)
            {
                TKey key = (TKey)(object)JsonToString.Read(reader, root);
                reader.Read();

                TValue value = readValue(reader, root);
                reader.Read();

                result[key] = value;
            }

            reader.Expect(JsonToken.EndObject);
            return result;
        }
        
        public static void Write(JsonWriter writer, string propertyName, IDictionary<string, TValue> dictionary, Action<JsonWriter, TValue> writeValue)
        {
            if (dictionary?.Count > 0)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, dictionary, writeValue);
            }
        }

        public static void Write(JsonWriter writer, IDictionary<string, TValue> dictionary, Action<JsonWriter, TValue> writeValue)
        {
            if (dictionary == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartObject();

            foreach (KeyValuePair<string, TValue> pair in dictionary)
            {
                writer.WritePropertyName(pair.Key);
                writeValue(writer, pair.Value);
            }

            writer.WriteEndObject();
        }
    }
}
