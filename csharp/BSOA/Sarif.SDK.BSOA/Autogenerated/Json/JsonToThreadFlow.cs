using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToThreadFlow))]
    public partial class ThreadFlow
    { }
    
    internal class JsonToThreadFlow : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ThreadFlow>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ThreadFlow>>()
        {
            ["id"] = (reader, root, me) => me.Id = JsonToString.Read(reader, root),
            ["message"] = (reader, root, me) => me.Message = JsonToMessage.Read(reader, root),
            ["initialState"] = (reader, root, me) => me.InitialState = JsonToIDictionary<String, MultiformatMessageString>.Read(reader, root, null, JsonToMultiformatMessageString.Read),
            ["immutableState"] = (reader, root, me) => me.ImmutableState = JsonToIDictionary<String, MultiformatMessageString>.Read(reader, root, null, JsonToMultiformatMessageString.Read),
            ["locations"] = (reader, root, me) => JsonToIList<ThreadFlowLocation>.Read(reader, root, me.Locations, JsonToThreadFlowLocation.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ThreadFlow Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            ThreadFlow item = (root == null ? new ThreadFlow() : new ThreadFlow(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ThreadFlow item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ThreadFlow item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "id", item.Id, default);
                JsonToMessage.Write(writer, "message", item.Message);
                JsonToIDictionary<String, MultiformatMessageString>.Write(writer, "initialState", item.InitialState, JsonToMultiformatMessageString.Write);
                JsonToIDictionary<String, MultiformatMessageString>.Write(writer, "immutableState", item.ImmutableState, JsonToMultiformatMessageString.Write);
                JsonToIList<ThreadFlowLocation>.Write(writer, "locations", item.Locations, JsonToThreadFlowLocation.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ThreadFlow));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ThreadFlow)value);
        }
    }
}
