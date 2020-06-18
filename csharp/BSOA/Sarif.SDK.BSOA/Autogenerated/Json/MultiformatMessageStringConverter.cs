using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(MultiformatMessageStringConverter))]
    public partial class MultiformatMessageString
    { }
    
    public class MultiformatMessageStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(MultiformatMessageString));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadMultiformatMessageString();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((MultiformatMessageString)value);
        }
    }
    
    internal static class MultiformatMessageStringJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, MultiformatMessageString>> setters = new Dictionary<string, Action<JsonReader, SarifLog, MultiformatMessageString>>()
        {
            ["text"] = (reader, root, me) => me.Text = reader.ReadString(root),
            ["markdown"] = (reader, root, me) => me.Markdown = reader.ReadString(root),
            ["properties"] = (reader, root, me) => Readers.PropertyBagConverter.Instance.ReadJson(reader, null, me.Properties, null)
        };

        public static MultiformatMessageString ReadMultiformatMessageString(this JsonReader reader, SarifLog root = null)
        {
            MultiformatMessageString item = (root == null ? new MultiformatMessageString() : new MultiformatMessageString(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, MultiformatMessageString item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, MultiformatMessageString item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("text", item.Text, default);
                writer.Write("markdown", item.Markdown, default);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
