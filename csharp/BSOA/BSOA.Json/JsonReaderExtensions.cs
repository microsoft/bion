// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
                throw new JsonReaderException($"Expected {token}, found {reader.TokenType} at {reader.Position()}");
            }
        }

        public static string Position(this JsonReader reader)
        {
            JsonTextReader jtr = reader as JsonTextReader;
            return (jtr == null ? $"{reader.Path}" : $"{reader.Path} ({jtr.LineNumber:n0}, {jtr.LinePosition:n0})");
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
                    throw new JsonReaderException($"Unknown property {typeof(TItem).Name}.{propertyName}.");
                }

                setter(reader, root, item);
                reader.Read();
            }

            reader.Expect(JsonToken.EndObject);
        }
    }
}
