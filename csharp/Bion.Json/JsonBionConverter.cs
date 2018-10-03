using Newtonsoft.Json;
using System;
using System.IO;

namespace Bion.Json
{
    public class JsonBionConverter
    {
        public static void JsonToBion(string jsonPath, string BionPath)
        {
            using (JsonTextReader reader = new JsonTextReader(new StreamReader(jsonPath)))
            using (BionWriter writer = new BionWriter(new FileStream(BionPath, FileMode.Create)))
            {
                JsonToBion(reader, writer);
            }
        }

        public static void JsonToBion(JsonTextReader reader, BionWriter writer)
        {
            while(reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonToken.StartObject:
                        writer.WriteStartObject();
                        break;
                    case JsonToken.StartArray:
                        writer.WriteStartArray();
                        break;
                    case JsonToken.EndObject:
                        writer.WriteEndObject();
                        break;
                    case JsonToken.EndArray:
                        writer.WriteEndArray();
                        break;
                    case JsonToken.PropertyName:
                        writer.WritePropertyName((string)reader.Value);
                        break;
                    case JsonToken.String:
                        string value = (string)reader.Value;
                        writer.WriteValue(value);
                        break;
                    case JsonToken.Integer:
                        writer.WriteValue((long)reader.Value);
                        break;

                    case JsonToken.Boolean:
                        writer.WriteValue((bool)reader.Value);
                        break;
                    case JsonToken.Null:
                        writer.WriteNull();
                        break;
                    case JsonToken.Float:
                        writer.WriteValue((double)reader.Value);
                        break;
                    default:
                        throw new NotImplementedException($"JsonToBion not implemented for {reader.TokenType}.");
                }
            }
        }

        public static void BionToJson(string BionPath, string jsonPath)
        {
            using (BionReader reader = new BionReader(new FileStream(BionPath, FileMode.Open)))
            using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(jsonPath)))
            {
                writer.Formatting = Formatting.Indented;
                BionToJson(reader, writer);
            }
        }

        public static void BionToJson(BionReader reader, JsonTextWriter writer)
        {
            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case BionToken.StartObject:
                        writer.WriteStartObject();
                        break;
                    case BionToken.StartArray:
                        writer.WriteStartArray();
                        break;
                    case BionToken.EndObject:
                        writer.WriteEndObject();
                        break;
                    case BionToken.EndArray:
                        writer.WriteEndArray();
                        break;
                    case BionToken.PropertyName:
                        writer.WritePropertyName(reader.CurrentString());
                        break;
                    case BionToken.String:
                        writer.WriteValue(reader.CurrentString());
                        break;
                    case BionToken.Integer:
                        writer.WriteValue(reader.CurrentInteger());
                        break;
                    case BionToken.Float:
                        writer.WriteValue(reader.CurrentFloat());
                        break;
                    case BionToken.True:
                    case BionToken.False:
                        writer.WriteValue(reader.CurrentBool());
                        break;
                    case BionToken.Null:
                        writer.WriteNull();
                        break;
                    default:
                        throw new NotImplementedException($"BionToJson not implemented for {reader.TokenType}.");
                }
            }
        }

        public static bool Compare(string jsonPath, string BionPath)
        {
            using (JsonTextReader jsonReader = new JsonTextReader(new StreamReader(jsonPath)))
            using (BionReader BionReader = new BionReader(new FileStream(BionPath, FileMode.Open)))
            {
                return Compare(jsonReader, BionReader);
            }
        }

        public static bool Compare(JsonTextReader jsonReader, BionReader BionReader)
        {
            while (true)
            {
                bool moreJson = jsonReader.Read();
                bool moreBion = BionReader.Read();
                AssertEqual(jsonReader, BionReader, moreJson, moreBion, $".Read() return value");
                if (moreJson == false) break;

                JsonToken BionToken = Convert(BionReader.TokenType);
                AssertEqual(jsonReader, BionReader, jsonReader.TokenType, BionToken, "Token Type");

                switch (BionToken)
                {
                    case JsonToken.PropertyName:
                    case JsonToken.String:
                        AssertEqual(jsonReader, BionReader, (string)jsonReader.Value, BionReader.CurrentString(), "text value");
                        break;
                    case JsonToken.Integer:
                        AssertEqual(jsonReader, BionReader, (long)jsonReader.Value, BionReader.CurrentInteger(), "integer value");
                        break;
                    case JsonToken.Float:
                        AssertEqual(jsonReader, BionReader, (double)jsonReader.Value, BionReader.CurrentFloat(), "float value");
                        break;
                    case JsonToken.Boolean:
                        AssertEqual(jsonReader, BionReader, (bool)jsonReader.Value, BionReader.CurrentBool(), "bool value");
                        break;
                }
            }

            return true;
        }

        private static void AssertEqual<T>(JsonTextReader jsonReader, BionReader BionReader, T jsonValue, T BionValue, string message)
        {
            if (!jsonValue.Equals(BionValue))
            {
                throw new DataMisalignedException($"Compare found different {message}\r\n at Json: ({jsonReader.LineNumber}, {jsonReader.LinePosition}), Bion: ({BionReader.BytesRead:n0}).\r\n Json: {jsonValue}, Bion: {BionValue}");
            }
        }

        public static JsonToken Convert(BionToken type)
        {
            switch(type)
            {
                case BionToken.StartObject:
                    return JsonToken.StartObject;
                case BionToken.StartArray:
                    return JsonToken.StartArray;
                case BionToken.EndObject:
                    return JsonToken.EndObject;
                case BionToken.EndArray:
                    return JsonToken.EndArray;
                case BionToken.PropertyName:
                    return JsonToken.PropertyName;
                case BionToken.String:
                    return JsonToken.String;
                case BionToken.Float:
                    return JsonToken.Float;
                case BionToken.Integer:
                    return JsonToken.Integer;
                case BionToken.False:
                case BionToken.True:
                    return JsonToken.Boolean;
                case BionToken.Null:
                    return JsonToken.Null;
                default:
                    throw new NotImplementedException($"Don't know conversion for {type} to JsonToken.");
            }
        }
    }
}
