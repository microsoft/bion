using System;
using System.Collections.Generic;

namespace Newtonsoft.Json
{
    public static class JsonReaderExtensions
    {
        public static void Expect(this JsonReader reader, JsonToken token)
        {
            if (reader.TokenType != token)
            {
                JsonTextReader jtr = reader as JsonTextReader;
                string position = (jtr == null ? "" : $"({jtr.LineNumber:n0}, {jtr.LinePosition:n0})");
                throw new JsonReaderException($"Expected {token}, found {reader.TokenType} at {reader.Path}. {position}");
            }
        }

        public static void ReadObject<TItem, TRoot>(this JsonReader reader, TRoot root, TItem item, Dictionary<string, Action<JsonReader, TRoot, TItem>> setters)
        {
            if (reader.TokenType == JsonToken.Null) { return; }

            reader.Expect(JsonToken.StartObject);
            reader.Read();

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

            reader.Expect(JsonToken.EndObject);
        }

        public static void ReadList<TItem, TRoot>(this JsonReader reader, TRoot root, IList<TItem> list, Func<JsonReader, TRoot, TItem> readItem)
        {
            if (reader.TokenType == JsonToken.Null) { return; }

            reader.Expect(JsonToken.StartArray);
            reader.Read();

            while (reader.TokenType != JsonToken.EndArray)
            {
                list.Add(readItem(reader, root));
                reader.Read();
            }

            reader.Expect(JsonToken.EndArray);
        }

        public static void ReadDictionary<TKey, TValue, TRoot>(this JsonReader reader, TRoot root, IDictionary<TKey, TValue> dictionary, Func<JsonReader, TRoot, TKey> readKey, Func<JsonReader, TRoot, TValue> readValue)
        {
            if (reader.TokenType == JsonToken.Null) { return; }
            
            reader.Expect(JsonToken.StartObject);
            reader.Read();

            while (reader.TokenType != JsonToken.EndObject)
            {
                TKey key = readKey(reader, root);
                reader.Read();

                TValue value = readValue(reader, root);
                reader.Read();
            }

            reader.Expect(JsonToken.EndObject);
        }

        public static int ReadInt<TRoot>(this JsonReader reader, TRoot root)
        {
            return (int)(long)reader.Value;
        }

        public static string ReadString<TRoot>(this JsonReader reader, TRoot root)
        {
            return (string)reader.Value;
        }
    }
}
