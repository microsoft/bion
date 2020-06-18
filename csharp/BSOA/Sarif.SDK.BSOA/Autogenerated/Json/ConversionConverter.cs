using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(ConversionConverter))]
    public partial class Conversion
    { }
    
    public class ConversionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Conversion));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.ReadConversion();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.Write((Conversion)value);
        }
    }
    
    internal static class ConversionJsonExtensions
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Conversion>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Conversion>>()
        {
            ["tool"] = (reader, root, me) => me.Tool = reader.ReadTool(root),
            ["invocation"] = (reader, root, me) => me.Invocation = reader.ReadInvocation(root),
            ["analysisToolLogFiles"] = (reader, root, me) => reader.ReadList(root, me.AnalysisToolLogFiles, ArtifactLocationJsonExtensions.ReadArtifactLocation),
            ["properties"] = (reader, root, me) => reader.ReadDictionary(root, me.Properties, JsonReaderExtensions.ReadString, SerializedPropertyInfoJsonExtensions.ReadSerializedPropertyInfo)
        };

        public static Conversion ReadConversion(this JsonReader reader, SarifLog root = null)
        {
            Conversion item = (root == null ? new Conversion() : new Conversion(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(this JsonWriter writer, string propertyName, Conversion item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                writer.Write(item);
            }
        }

        public static void Write(this JsonWriter writer, Conversion item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                writer.Write("tool", item.Tool);
                writer.Write("invocation", item.Invocation);
                writer.WriteList("analysisToolLogFiles", item.AnalysisToolLogFiles, ArtifactLocationJsonExtensions.Write);
                writer.Write("properties", item.Properties, default);
                writer.WriteEndObject();
            }
        }
    }
}
