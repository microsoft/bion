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
    }
}
