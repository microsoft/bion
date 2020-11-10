// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

namespace BSOA.Test.Model.Log
{
    [JsonConverter(typeof(JsonToRun))]
    public partial class Run
    { }

    internal class JsonToRun : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, Run, Run>> setters = new Dictionary<string, Action<JsonReader, Run, Run>>()
        {
            ["results"] = (reader, root, me) => me.Results = JsonToIList<Result>.Read(reader, root, null, JsonToResult.Read),
            ["rules"] = (reader, root, me) => me.Rules = JsonToIList<Rule>.Read(reader, root, null, JsonToRule.Read)
        };

        public static Run Read(JsonReader reader, Run root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }

            Run item = new Run();

            // Run is root object
            root = item;

            reader.ReadObject(root, item, setters);

            // Trim after read to consolidate 'during read' content
            item.DB.Trim();

            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Run item, bool required = false)
        {
            if (required || item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Run item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToIList<Result>.Write(writer, "results", item.Results, JsonToResult.Write);
                JsonToIList<Rule>.Write(writer, "rules", item.Rules, JsonToRule.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Run));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Run)value);
        }
    }
}
