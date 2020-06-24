// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using Newtonsoft.Json;

namespace BSOA.Json.Converters
{
    public static class JsonToUri
    {
        public static Uri Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static Uri Read(JsonReader reader)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            else
            {
                return new Uri((string)reader.Value, UriKind.RelativeOrAbsolute);
            }
        }

        public static void Write(JsonWriter writer, string propertyName, Uri item, Uri defaultValue = default)
        {
            if (item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item?.OriginalString);
            }
        }

        public static void Write(JsonWriter writer, Uri item)
        {
            writer.WriteValue(item?.OriginalString);
        }
    }
}
