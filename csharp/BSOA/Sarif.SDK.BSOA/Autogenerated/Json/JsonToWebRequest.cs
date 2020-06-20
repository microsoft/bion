using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToWebRequest))]
    public partial class WebRequest
    { }
    
    internal class JsonToWebRequest : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, WebRequest>> setters = new Dictionary<string, Action<JsonReader, SarifLog, WebRequest>>()
        {
            ["index"] = (reader, root, me) => me.Index = JsonToInt.Read(reader, root),
            ["protocol"] = (reader, root, me) => me.Protocol = JsonToString.Read(reader, root),
            ["version"] = (reader, root, me) => me.Version = JsonToString.Read(reader, root),
            ["target"] = (reader, root, me) => me.Target = JsonToString.Read(reader, root),
            ["method"] = (reader, root, me) => me.Method = JsonToString.Read(reader, root),
            ["headers"] = (reader, root, me) => me.Headers = JsonToIDictionary<String, String>.Read(reader, root, null, JsonToString.Read),
            ["parameters"] = (reader, root, me) => me.Parameters = JsonToIDictionary<String, String>.Read(reader, root, null, JsonToString.Read),
            ["body"] = (reader, root, me) => me.Body = JsonToArtifactContent.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static WebRequest Read(JsonReader reader, SarifLog root = null)
        {
            WebRequest item = (root == null ? new WebRequest() : new WebRequest(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, WebRequest item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, WebRequest item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToInt.Write(writer, "index", item.Index, -1);
                JsonToString.Write(writer, "protocol", item.Protocol, default);
                JsonToString.Write(writer, "version", item.Version, default);
                JsonToString.Write(writer, "target", item.Target, default);
                JsonToString.Write(writer, "method", item.Method, default);
                JsonToIDictionary<String, String>.Write(writer, "headers", item.Headers, JsonToString.Write);
                JsonToIDictionary<String, String>.Write(writer, "parameters", item.Parameters, JsonToString.Write);
                JsonToArtifactContent.Write(writer, "body", item.Body);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(WebRequest));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (WebRequest)value);
        }
    }
}
