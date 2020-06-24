// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace BSOA.Json.Converters
{
    public static class JsonToIList<TItem>
    {
        public static IList<TItem> Read<TRoot>(JsonReader reader, TRoot root, IList<TItem> list, Func<JsonReader, TRoot, TItem> readItem)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }

            reader.Expect(JsonToken.StartArray);
            reader.Read();

            IList<TItem> result = (list ?? new List<TItem>());

            while (reader.TokenType != JsonToken.EndArray)
            {
                result.Add(readItem(reader, root));
                reader.Read();
            }

            reader.Expect(JsonToken.EndArray);
            return result;
        }

        public static void Write(JsonWriter writer, string propertyName, IList<TItem> list, Action<JsonWriter, TItem> writeItem)
        {
            if (list?.Count > 0)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, list, writeItem);
            }
        }

        public static void Write(JsonWriter writer, IList<TItem> list, Action<JsonWriter, TItem> writeItem)
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
    }
}
