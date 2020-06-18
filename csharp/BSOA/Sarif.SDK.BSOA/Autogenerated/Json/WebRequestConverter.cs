using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(WebRequestConverter))]
    public partial class WebRequest
    { }
    
    public class WebRequestConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(WebRequest));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadWebRequest();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((WebRequest)value);
        }
    }
    
    internal static class WebRequestJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, WebRequest>> setters = new Dictionary<string, Action<JsonReader, SarifLog, WebRequest>>()
        {
            ["index"] = (reader, root, me) => me.Index = reader.ReadInt(root),
            ["protocol"] = (reader, root, me) => me.Protocol = reader.ReadString(root),
            ["version"] = (reader, root, me) => me.Version = reader.ReadString(root),
            ["target"] = (reader, root, me) => me.Target = reader.ReadString(root),
            ["method"] = (reader, root, me) => me.Method = reader.ReadString(root),
            ["headers"] = (reader, root, me) => reader.ReadDictionary(root, me.Headers, JsonReaderExtensions.ReadString, JsonReaderExtensions.ReadString),
            ["parameters"] = (reader, root, me) => reader.ReadDictionary(root, me.Parameters, JsonReaderExtensions.ReadString, JsonReaderExtensions.ReadString),
            ["body"] = (reader, root, me) => me.Body = reader.ReadArtifactContent(root),
            ["properties"] = (reader, root, me) => me.Properties = (IDictionary<string, SerializedPropertyInfo>)Readers.PropertyBagConverter.Instance.ReadJson(reader, null, null, null)
        };

        public static WebRequest ReadWebRequest(this JsonReader reader, SarifLog root = null)
        {
            WebRequest item = (root == null ? new WebRequest() : new WebRequest(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, WebRequest item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, WebRequest item)
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
                writer.Write("target", item.Target, default);
                writer.Write("method", item.Method, default);
                writer.Write("headers", item.Headers, default);
                writer.Write("parameters", item.Parameters, default);
                writer.Write("body", item.Body);
                writer.WriteDictionary("properties", item.Properties, SerializedPropertyInfoJsonExtensions.Write);
                writer.WriteEndObject();
            }
        }
    }
}
