using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ExceptionDataConverter))]
    public partial class ExceptionData
    { }
    
    public class ExceptionDataConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(ExceptionData));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadExceptionData();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((ExceptionData)value);
        }
    }
    
    internal static class ExceptionDataJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, ExceptionData>> setters = new Dictionary<string, Action<JsonReader, SarifLog, ExceptionData>>()
        {
            ["kind"] = (reader, root, me) => me.Kind = reader.ReadString(root),
            ["message"] = (reader, root, me) => me.Message = reader.ReadString(root),
            ["stack"] = (reader, root, me) => me.Stack = reader.ReadStack(root),
            ["innerExceptions"] = (reader, root, me) => reader.ReadList(root, me.InnerExceptions, ExceptionDataJsonExtensions.ReadExceptionData),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static ExceptionData ReadExceptionData(this JsonReader reader, SarifLog root = null)
        {
            ExceptionData item = (root == null ? new ExceptionData() : new ExceptionData(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, ExceptionData item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, ExceptionData item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("kind", item.Kind, default(string));
                writer.Write("message", item.Message, default(string));
                writer.Write("stack", item.Stack);
                writer.WriteList("innerExceptions", item.InnerExceptions, ExceptionDataJsonExtensions.Write);
                writer.Write("properties", item.Properties, default(IDictionary<string, SerializedPropertyInfo>));
                writer.WriteEndObject();
            }
        }
    }
}
