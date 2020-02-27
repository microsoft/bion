using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace ScaleDemo
{
    public class RegionTableConverter : JsonConverter<RegionTable>
    {
        public static RegionTableConverter Instance = new RegionTableConverter();
        private Dictionary<string, Action<JsonReader, Region4>> regionFieldParsers = new Dictionary<string, Action<JsonReader, Region4>>();

        public RegionTableConverter()
        {
            regionFieldParsers = new Dictionary<string, Action<JsonReader, Region4>>();
            BuildRegionParsers();
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, [AllowNull] RegionTable value, JsonSerializer serializer)
        {
            // Use default for serializing
            throw new NotImplementedException();
        }

        public override RegionTable ReadJson(JsonReader reader, Type objectType, [AllowNull] RegionTable existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            RegionTable result = existingValue ?? new RegionTable();
            Expect(reader, JsonToken.StartArray, "List<Region4>");

            while (reader.TokenType != JsonToken.EndArray)
            {
                Region4 item = result.Add();
                DeserializeRegion4(reader, item);
            }

            Expect(reader, JsonToken.EndArray, "List<Region4>");
            return result;
        }

        public void DeserializeRegion4(JsonReader reader, Region4 result)
        {
            Expect(reader, JsonToken.StartObject, "Region4");

            while (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();

                if (!regionFieldParsers.TryGetValue(propertyName, out Action<JsonReader, Region4> parser))
                {
                    throw new NotImplementedException($"Unknown property Region.{propertyName}. Stopping.");
                }

                parser(reader, result);
                reader.Read();
            }

            Expect(reader, JsonToken.EndObject, "Region4");
        }

        private void BuildRegionParsers()
        {
            regionFieldParsers["startLine"] = (reader, region) => { region.StartLine = reader.ReadAsInt32().Value; };
            regionFieldParsers["startColumn"] = (reader, region) => { region.StartColumn = reader.ReadAsInt32().Value; };
            regionFieldParsers["endLine"] = (reader, region) => { region.EndLine = reader.ReadAsInt32().Value; };
            regionFieldParsers["endColumn"] = (reader, region) => { region.EndColumn = reader.ReadAsInt32().Value; };

            regionFieldParsers["byteLength"] = (reader, region) => { region.ByteLength = reader.ReadAsInt32().Value; };
            regionFieldParsers["byteOffset"] = (reader, region) => { region.ByteOffset = reader.ReadAsInt32().Value; };
            regionFieldParsers["charLength"] = (reader, region) => { region.CharLength = reader.ReadAsInt32().Value; };
            regionFieldParsers["charOffset"] = (reader, region) => { region.CharOffset = reader.ReadAsInt32().Value; };
        }

        private static void Expect(JsonReader reader, JsonToken requiredToken, string parseContext)
        {
            if (reader.TokenType == JsonToken.None) { reader.Read(); }

            if (reader.TokenType != requiredToken)
            {
                throw new IOException($"Reader found invalid token type '{requiredToken}' while parsing {parseContext}.");
            }

            reader.Read();
        }
    }
}
