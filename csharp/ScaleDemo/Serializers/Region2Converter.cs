// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using Newtonsoft.Json;

namespace ScaleDemo
{
    public class Region2Converter : JsonConverter<Region2>
    {
        public static Region2Converter Instance = new Region2Converter();
        private Dictionary<string, Action<JsonReader, Region2>> regionFieldParsers = new Dictionary<string, Action<JsonReader, Region2>>();

        private Region2Converter()
        {
            regionFieldParsers = new Dictionary<string, Action<JsonReader, Region2>>();
            BuildRegionParsers();
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, [AllowNull] Region2 value, JsonSerializer serializer)
        {
            // Use default for serializing
            throw new NotImplementedException();
        }

        public override Region2 ReadJson(JsonReader reader, Type objectType, [AllowNull] Region2 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Region2 result = new Region2(); //existingValue ?? new Region2();
            //Expect(reader, JsonToken.StartObject, "Region2");

            while (reader.TokenType == JsonToken.PropertyName)
            {
                string propertyName = reader.Value.ToString();

                if (!regionFieldParsers.TryGetValue(propertyName, out Action<JsonReader, Region2> parser))
                {
                    throw new NotImplementedException($"Unknown property Region.{propertyName}. Stopping.");
                }

                parser(reader, result);
                reader.Read();
            }

            //Expect(reader, JsonToken.EndObject, "Region2");
            return result;
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
