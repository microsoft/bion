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
    }
}
