using Newtonsoft.Json.Converters;

using System;
using System.Collections.Generic;

namespace Newtonsoft.Json
{
    public static class JsonWriterExtensions
    {
        public const string DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'";

        public static void Write<T>(this JsonWriter writer, string propertyName, IList<T> list, IList<T> defaultList = null)
        {
            if (list?.Count > 0)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(list);
            }
        }

        public static void Write<T>(this JsonWriter writer, IList<T> list, IList<T> defaultList = null)
        {
            if (list == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartArray();

            for (int i = 0; i < list.Count; ++i)
            {
                writer.WriteValue(list[i]);
            }

            writer.WriteEndArray();
        }

        public static void WriteList<T>(this JsonWriter writer, string propertyName, IList<T> list, Action<JsonWriter, T> writeItem)
        {
            if (list?.Count > 0)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteList(list, writeItem);
            }
        }

        public static void WriteList<T>(this JsonWriter writer, IList<T> list, Action<JsonWriter, T> writeItem)
        {
            if (list == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartArray();

            for (int i = 0; i < list.Count; ++i)
            {
                writeItem(writer, list[i]);
            }

            writer.WriteEndArray();
        }

        public static void WriteDictionary<TValue>(this JsonWriter writer, string propertyName, IDictionary<string, TValue> dictionary, Action<JsonWriter, TValue> writeValue)
        {
            if (dictionary?.Count > 0)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteDictionary(dictionary, writeValue);
            }
        }

        public static void WriteDictionary<TValue>(this JsonWriter writer, IDictionary<string, TValue> dictionary, Action<JsonWriter, TValue> writeValue)
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

        public static void Write<TValue>(this JsonWriter writer, string propertyName, IDictionary<string, TValue> dictionary, IDictionary<string, TValue> defaultValue = null)
        {
            if (dictionary?.Count > 0)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(dictionary);
            }
        }

        public static void Write<TValue>(this JsonWriter writer, IDictionary<string, TValue> dictionary, IDictionary<string, TValue> defaultValue = null)
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
                writer.WriteValue(pair.Value);
            }

            writer.WriteEndObject();
        }

        private static StringEnumConverter _enumConverter = new StringEnumConverter(camelCaseText: true);
        public static void WriteEnum<TEnum>(this JsonWriter writer, string propertyName, TEnum value, TEnum defaultValue = default(TEnum)) where TEnum : System.Enum
        {
            if (!value.Equals(defaultValue))
            {
                writer.WritePropertyName(propertyName);
                _enumConverter.WriteJson(writer, value, null);
            }
        }

        public static void Write<T>(this JsonWriter writer, string propertyName, T item, T defaultValue) where T : unmanaged, IEquatable<T>
        {
            if (!item.Equals(defaultValue))
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(this JsonWriter writer, string propertyName, string item, string defaultValue = null)
        {
            if (item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(this JsonWriter writer, string propertyName, bool item, bool defaultValue = default(bool))
        {
            if (item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(this JsonWriter writer, string propertyName, DateTime item, DateTime defaultValue = default(DateTime))
        {
            if (item.ToUniversalTime() != defaultValue.ToUniversalTime())
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item.ToUniversalTime().ToString(DateTimeFormat));
            }
        }

        public static void Write(this JsonWriter writer, string propertyName, Uri item, Uri defaultValue = default(Uri))
        {
            if (item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item.OriginalString);
            }
        }
    }
}
