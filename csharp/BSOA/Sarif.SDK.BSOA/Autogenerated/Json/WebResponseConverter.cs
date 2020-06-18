using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(WebResponseConverter))]
    public partial class WebResponse
    { }
    
    public class WebResponseConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(WebResponse));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadWebResponse();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((WebResponse)value);
        }
    }
    
    internal static class WebResponseJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, WebResponse>> setters = new Dictionary<string, Action<JsonReader, SarifLog, WebResponse>>()
        {
            ["index"] = (reader, root, me) => me.Index = reader.ReadInt(root),
            ["protocol"] = (reader, root, me) => me.Protocol = reader.ReadString(root),
            ["version"] = (reader, root, me) => me.Version = reader.ReadString(root),
            ["statusCode"] = (reader, root, me) => me.StatusCode = reader.ReadInt(root),
            ["reasonPhrase"] = (reader, root, me) => me.ReasonPhrase = reader.ReadString(root),
            ["headers"] = (reader, root, me) => reader.ReadDictionary(root, me.Headers, JsonReaderExtensions.ReadString, JsonReaderExtensions.ReadString),
            ["body"] = (reader, root, me) => me.Body = reader.ReadArtifactContent(root),
            ["noResponseReceived"] = (reader, root, me) => me.NoResponseReceived = reader.ReadBool(root),
            ["properties"] = (reader, root, me) => me.Properties = (IDictionary<string, SerializedPropertyInfo>)Readers.PropertyBagConverter.Instance.ReadJson(reader, null, null, null)
        };

        public static WebResponse ReadWebResponse(this JsonReader reader, SarifLog root = null)
        {
            WebResponse item = (root == null ? new WebResponse() : new WebResponse(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, WebResponse item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, WebResponse item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("index", item.Index, -1);
                writer.Write("protocol", item.Protocol, default);
                writer.Write("version", item.Version, default);
                writer.Write("statusCode", item.StatusCode, default);
                writer.Write("reasonPhrase", item.ReasonPhrase, default);
                writer.Write("headers", item.Headers, default);
                writer.Write("body", item.Body);
                writer.Write("noResponseReceived", item.NoResponseReceived, false);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
