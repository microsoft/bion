using Bion.IO;
using Bion.Text;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Bion.Json
{
    public class JsonBionConverter
    {
        public static void JsonToBion(string jsonPath, string bionPath, string toDictionaryPath = null)
        {
            using (WordCompressor compressor = (String.IsNullOrEmpty(toDictionaryPath) ? null : WordCompressor.OpenWrite(toDictionaryPath)))
            {
                string toPath = (compressor == null ? bionPath : Path.ChangeExtension(bionPath, ".preopt.bion"));

                using (JsonTextReader reader = new JsonTextReader(new StreamReader(jsonPath)))
                using (BionWriter writer = new BionWriter(File.Create(toPath), compressor))
                {
                    JsonToBion(reader, writer);
                }

                if (compressor != null)
                {
                    using (BionReader reader = new BionReader(File.OpenRead(toPath), compressor))
                    using (BufferedWriter writer = new BufferedWriter(File.Create(bionPath)))
                    {
                        reader.RewriteOptimized(writer);
                    }

                    File.Delete(toPath);

                    // :/ Rewrite compressor; pre-optimize pass calls Dispose which writes it too early.
                    compressor.Write(File.OpenWrite(toDictionaryPath));
                }
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
                    case JsonToken.Date:
                        writer.WriteValue(((DateTime)reader.Value).ToString("yyyy-MM-ddThh:mm:ss.FFFFFFFZ"));
                        break;
                    case JsonToken.Comment:
                        // Nothing Written
                        break;
                    default:
                        throw new NotImplementedException($"JsonToBion not implemented for {reader.TokenType}.");
                }
            }
        }

        public static void BionToJson(string bionPath, string jsonPath, string fromDictionaryPath = null)
        {
            using (WordCompressor compressor = (String.IsNullOrEmpty(fromDictionaryPath) ? null : WordCompressor.OpenRead(fromDictionaryPath)))
            using (BionReader reader = new BionReader(File.OpenRead(bionPath), compressor))
            using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(jsonPath)))
            {
                writer.Formatting = Formatting.Indented;
                BionToJson(reader, writer);
            }
        }

        public static void BionToJson(BionReader reader, JsonTextWriter writer)
        {
            long previousPosition = 0;

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
                        throw new NotImplementedException($"BionToJson not implemented for {reader.TokenType} @{previousPosition}.");
                }

                previousPosition = reader.BytesRead;
                if (previousPosition == 30204) System.Diagnostics.Debugger.Break();
            }
        }
    }
}
