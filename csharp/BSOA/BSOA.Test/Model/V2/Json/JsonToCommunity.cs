// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

namespace BSOA.Test.Model.V2
{
    [JsonConverter(typeof(JsonToCommunity))]
    public partial class Community
    { }

    internal class JsonToCommunity : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, Community, Community>> setters = new Dictionary<string, Action<JsonReader, Community, Community>>()
        {
            ["people"] = (reader, root, me) => JsonToIList<Person>.Read(reader, root, me.People, JsonToPerson.Read)
        };

        public static Community Read(JsonReader reader, Community root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }

            Community item = new Community();

            // Community is root object
            root = item;

            reader.ReadObject(root, item, setters);

            // Trim after read to consolidate 'during read' content
            item.DB.Trim();

            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Community item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Community item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToIList<Person>.Write(writer, "people", item.People, JsonToPerson.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Community));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Community)value);
        }
    }
}
