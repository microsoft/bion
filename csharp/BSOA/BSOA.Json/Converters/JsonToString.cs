// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;

namespace BSOA.Json.Converters
{
    public static class JsonToString
    {
        public static string Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static string Read(JsonReader reader)
        {
            return (string)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, string item, string defaultValue = default)
        {
            if (item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, string item)
        {
            writer.WriteValue(item);
        }
    }
}
