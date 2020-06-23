using BSOA.Json;
using BSOA.Json.Converters;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    [JsonConverter(typeof(JsonToConversion))]
    public partial class Conversion
    { }
    
    internal class JsonToConversion : JsonConverter
    {
        private static Dictionary<string, Action<JsonReader, SarifLog, Conversion>> setters = new Dictionary<string, Action<JsonReader, SarifLog, Conversion>>()
        {
            ["tool"] = (reader, root, me) => me.Tool = JsonToTool.Read(reader, root),
            ["invocation"] = (reader, root, me) => me.Invocation = JsonToInvocation.Read(reader, root),
            ["analysisToolLogFiles"] = (reader, root, me) => JsonToIList<ArtifactLocation>.Read(reader, root, me.AnalysisToolLogFiles, JsonToArtifactLocation.Read),
            ["properties"] = (reader, root, me) => me.Properties = JsonToIDictionary<String, SerializedPropertyInfo>.Read(reader, root, null, JsonToSerializedPropertyInfo.Read)
        };

        public static Conversion Read(JsonReader reader, SarifLog root = null)
        {
            if (reader.TokenType == JsonToken.Null) { return null; }
            
            Conversion item = (root == null ? new Conversion() : new Conversion(root));
            reader.ReadObject(root, item, setters);
            return item;
        }

        public static void Write(JsonWriter writer, string propertyName, Conversion item)
        {
            if (item != null)
            {
                writer.WritePropertyName(propertyName);
                Write(writer, item);
            }
        }

        public static void Write(JsonWriter writer, Conversion item)
        {
            if (item == null)
            {
                writer.WriteNull();
            }
            else
            {
                writer.WriteStartObject();
                JsonToTool.Write(writer, "tool", item.Tool);
                JsonToInvocation.Write(writer, "invocation", item.Invocation);
                JsonToIList<ArtifactLocation>.Write(writer, "analysisToolLogFiles", item.AnalysisToolLogFiles, JsonToArtifactLocation.Write);
                JsonToIDictionary<String, SerializedPropertyInfo>.Write(writer, "properties", item.Properties, JsonToSerializedPropertyInfo.Write);
                writer.WriteEndObject();
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(Conversion));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Read(reader);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            Write(writer, (Conversion)value);
        }
    }
}
