// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

namespace BSOA.Benchmarks.Model
{
    [JsonConverter(typeof(JsonToRule))]
    public partial class Rule
    { }
    
    internal class JsonToRule : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, Run, Rule>> setters = new Dictionary<string, Action<JsonReader, Run, Rule>>()
        {
            ["id"] = (reader, root, me) => me.Id = JsonToString.Read(reader, root),
            ["guid"] = (reader, root, me) => me.Guid = JsonToString.Read(reader, root),
            ["helpUri"] = (reader, root, me) => me.HelpUri = JsonToUri.Read(reader, root)
        };

        public static Rule Read(JsonReader reader, Run root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Rule item = (root == null ? new Rule() : new Rule(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Rule item, bool required = false)
        {
            if (required || item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Rule item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "id", item.Id, default);
                JsonToString.Write(writer, "guid", item.Guid, default);
                JsonToUri.Write(writer, "helpUri", item.HelpUri, default);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Rule));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Rule)value);
        }
    }
}
