// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;

namespace BSOA.Json.Converters
{
    public static class JsonToBool
    {
        public static bool Read<TRoot>(JsonReader reader, TRoot root)
        {
            return (bool)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, bool item, bool defaultValue = default)
        {
            if (item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, bool item)
        {
            writer.WriteValue(item);
        }
    }
}
