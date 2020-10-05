// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

namespace BSOA.Benchmarks.Model
{
    [JsonConverter(typeof(JsonToResult))]
    public partial class Result
    { }
    
    internal class JsonToResult : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, Run, Result>> setters = new Dictionary<string, Action<JsonReader, Run, Result>>()
        {
            ["ruleId"] = (reader, root, me) => me.RuleId = JsonToString.Read(reader, root),
            ["message"] = (reader, root, me) => me.Message = JsonToString.Read(reader, root),
            ["startLine"] = (reader, root, me) => me.StartLine = JsonToInt.Read(reader, root),
            ["whenDetectedUtc"] = (reader, root, me) => me.WhenDetectedUtc = JsonToDateTime.Read(reader, root),
            ["baselineState"] = (reader, root, me) => me.BaselineState = JsonToEnum<BaselineState>.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, String>.Read(reader, root, null, JsonToString.Read),
            ["tags"] = (reader, root, me) => me.Tags = JsonToIList<String>.Read(reader, root, null, JsonToString.Read)
        };

        public static Result Read(JsonReader reader, Run root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Result item = (root == null ? new Result() : new Result(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Result item, bool required = false)
        {
            if (required || item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Result item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "ruleId", item.RuleId, default);
                JsonToString.Write(writer, "message", item.Message, default);
                JsonToInt.Write(writer, "startLine", item.StartLine, default);
                JsonToDateTime.Write(writer, "whenDetectedUtc", item.WhenDetectedUtc, default);
                JsonToEnum<BaselineState>.Write(writer, "baselineState", item.BaselineState, default(BaselineState));
                JsonToIDictionary<String, String>.Write(writer, "properties", item.Properties, JsonToString.Write);
                JsonToIList<String>.Write(writer, "tags", item.Tags, JsonToString.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Result));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Result)value);
        }
    }
}
