using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToWebResponse))]
    public partial class WebResponse
    { }
    
    internal class JsonToWebResponse : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, WebResponse>> setters = new Dictionary<string, Action<JsonReader, SarifLog, WebResponse>>()
        {
            ["index"] = (reader, root, me) => me.Index = JsonToInt.Read(reader, root),
            ["protocol"] = (reader, root, me) => me.Protocol = JsonToString.Read(reader, root),
            ["version"] = (reader, root, me) => me.Version = JsonToString.Read(reader, root),
            ["statusCode"] = (reader, root, me) => me.StatusCode = JsonToInt.Read(reader, root),
            ["reasonPhrase"] = (reader, root, me) => me.ReasonPhrase = JsonToString.Read(reader, root),
            ["headers"] = (reader, root, me) => me.Headers = JsonToIDictionary<String, String>.Read(reader, root, null, JsonToString.Read),
            ["body"] = (reader, root, me) => me.Body = JsonToArtifactContent.Read(reader, root),
            ["noResponseReceived"] = (reader, root, me) => me.NoResponseReceived = JsonToBool.Read(reader, root),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static WebResponse Read(JsonReader reader, SarifLog root = null)
        {
            WebResponse item = (root == null ? new WebResponse() : new WebResponse(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, WebResponse item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, WebResponse item)
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
                JsonToInt.Write(writer, "statusCode", item.StatusCode, default);
                JsonToString.Write(writer, "reasonPhrase", item.ReasonPhrase, default);
                JsonToIDictionary<String, String>.Write(writer, "headers", item.Headers, JsonToString.Write);
                JsonToArtifactContent.Write(writer, "body", item.Body);
                JsonToBool.Write(writer, "noResponseReceived", item.NoResponseReceived, false);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(WebResponse));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (WebResponse)value);
        }
    }
}
