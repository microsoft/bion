using Newtonsoft.Json;
using System;
using System.IO;

namespace Bion.Json
{
    public class JsonBionComparer
    {
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
            switch (type)
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
