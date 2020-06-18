using Newtonsoft.Json.Converters;

using System;
using System.Collections.Generic;
using System.Globalization;

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

                dictionary.Add(new KeyValuePair<TKey, TValue>(key, value));
            }

            reader.Expect(JsonToken.EndObject);
        }

        private static StringEnumConverter _enumConverter = new StringEnumConverter(camelCaseText: true);
        public static TEnum ReadEnum<TEnum, TRoot>(this JsonReader reader, TRoot root) where TEnum : System.Enum
        {
            return (TEnum)_enumConverter.ReadJson(reader, typeof(TEnum), null, null);
        }

        public static byte ReadByte<TRoot>(this JsonReader reader, TRoot root)
        {
            return (byte)(long)reader.Value;
        }

        public static int ReadInt<TRoot>(this JsonReader reader, TRoot root)
        {
            return (int)(long)reader.Value;
        }

        public static long ReadLong<TRoot>(this JsonReader reader, TRoot root)
        {
            return (long)reader.Value;
        }

        public static float ReadFloat<TRoot>(this JsonReader reader, TRoot root)
        {
            return (float)(double)reader.Value;
        }

        public static double ReadDouble<TRoot>(this JsonReader reader, TRoot root)
        {
            return (double)reader.Value;
        }

        public static bool ReadBool<TRoot>(this JsonReader reader, TRoot root)
        {
            return (bool)reader.Value;
        }

        public static string ReadString<TRoot>(this JsonReader reader, TRoot root)
        {
            return (string)reader.Value;
        }

        public static DateTime ReadDateTime<TRoot>(this JsonReader reader, TRoot root)
        {
            if (reader.Value is DateTime)
            {
                return (DateTime)reader.Value;
            }
            else
            {
                return DateTime.Parse((string)reader.Value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
            }
        }

        public static Uri ReadUri<TRoot>(this JsonReader reader, TRoot root)
        {
            return new Uri((string)reader.Value);
        }
    }
}
