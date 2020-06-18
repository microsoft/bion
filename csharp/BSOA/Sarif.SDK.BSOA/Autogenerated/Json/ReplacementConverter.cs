using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ReplacementConverter))]
    public partial class Replacement
    { }
    
    public class ReplacementConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Replacement));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadReplacement();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Replacement)value);
        }
    }
    
    internal static class ReplacementJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Replacement>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Replacement>>()
        {
            ["deletedRegion"] = (reader, root, me) => me.DeletedRegion = reader.ReadRegion(root),
            ["insertedContent"] = (reader, root, me) => me.InsertedContent = reader.ReadArtifactContent(root),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Replacement ReadReplacement(this JsonReader reader, SarifLog root = null)
        {
            Replacement item = (root == null ? new Replacement() : new Replacement(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Replacement item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Replacement item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("deletedRegion", item.DeletedRegion);
                writer.Write("insertedContent", item.InsertedContent);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
