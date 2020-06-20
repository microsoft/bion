using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToExceptionData))]
    public partial class ExceptionData
    { }
    
    internal class JsonToExceptionData : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ExceptionData>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ExceptionData>>()
        {
            ["kind"] = (reader, root, me) => me.Kind = JsonToString.Read(reader, root),
            ["message"] = (reader, root, me) => me.Message = JsonToString.Read(reader, root),
            ["stack"] = (reader, root, me) => me.Stack = JsonToStack.Read(reader, root),
            ["innerExceptions"] = (reader, root, me) => JsonToIList<ExceptionData>.Read(reader, root, me.InnerExceptions, JsonToExceptionData.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static ExceptionData Read(JsonReader reader, SarifLog root = null)
        {
            ExceptionData item = (root == null ? new ExceptionData() : new ExceptionData(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, ExceptionData item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, ExceptionData item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToString.Write(writer, "kind", item.Kind, default);
                JsonToString.Write(writer, "message", item.Message, default);
                JsonToStack.Write(writer, "stack", item.Stack);
                JsonToIList<ExceptionData>.Write(writer, "innerExceptions", item.InnerExceptions, JsonToExceptionData.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ExceptionData));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (ExceptionData)value);
        }
    }
}
