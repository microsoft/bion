// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Globalization;

using Newtonsoft.Json;

namespace BSOA.Json.Converters
{
    public static class JsonToDateTime
    {
        public const string DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'";

        public static DateTime Read<TRoot>(JsonReader reader, TRoot root)
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

        public static void Write(JsonWriter writer, string propertyName, DateTime item, DateTime defaultValue = default(DateTime))
        {
            if (item.ToUniversalTime() != defaultValue.ToUniversalTime())
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item.ToUniversalTime().ToString(DateTimeFormat));
            }
        }

        public static void Write(JsonWriter writer, DateTime item)
        {
            writer.WriteValue(item.ToUniversalTime().ToString(DateTimeFormat));
        }
    }
}
