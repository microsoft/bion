using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ThreadFlowConverter))]
    public partial class ThreadFlow
    { }
    
    public class ThreadFlowConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ThreadFlow));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadThreadFlow();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ThreadFlow)value);
        }
    }
    
    internal static class ThreadFlowJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ThreadFlow>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ThreadFlow>>()
        {
            ["id"] = (reader, root, me) => me.Id = reader.ReadString(root),
            ["message"] = (reader, root, me) => me.Message = reader.ReadMessage(root),
            ["initialState"] = (reader, root, me) => reader.ReadDictionary(root, me.InitialState, JsonReaderExtensions.ReadString, MultiformatMessageStringJsonExtensions.ReadMultiformatMessageString),
            ["immutableState"] = (reader, root, me) => reader.ReadDictionary(root, me.ImmutableState, JsonReaderExtensions.ReadString, MultiformatMessageStringJsonExtensions.ReadMultiformatMessageString),
            ["locations"] = (reader, root, me) => reader.ReadList(root, me.Locations, ThreadFlowLocationJsonExtensions.ReadThreadFlowLocation),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static ThreadFlow ReadThreadFlow(this JsonReader reader, SarifLog root = null)
        {
            ThreadFlow item = (root == null ? new ThreadFlow() : new ThreadFlow(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ThreadFlow item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ThreadFlow item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("id", item.Id, default(string));
                writer.Write("message", item.Message);
                writer.Write("initialState", item.InitialState, default(IDictionary<string, MultiformatMessageString>));
                writer.Write("immutableState", item.ImmutableState, default(IDictionary<string, MultiformatMessageString>));
                writer.WriteList("locations", item.Locations, ThreadFlowLocationJsonExtensions.Write);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
