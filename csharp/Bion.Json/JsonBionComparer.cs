using Newtonsoft.Json;
using System;
using System.IO;

namespace Bion.Json
{
    public class JsonBionComparer
    {
        public static bool Compare(string jsonPath, string bionPath)
        {
            using (JsonTextReader jsonReader = new JsonTextReader(new StreamReader(jsonPath)))
            using (BionReader BionReader = new BionReader(new FileStream(bionPath, FileMode.Open)))
            {
                return Compare(jsonReader, BionReader);
            }
        }

        public static bool Compare(JsonTextReader jsonReader, BionReader bionReader)
        {
            while (true)
            {
                bool moreJson = jsonReader.Read();
                bool moreBion = bionReader.Read();
                AssertEqual(jsonReader, bionReader, moreJson, moreBion, $".Read() return value");
                if (moreJson == false) break;

                JsonToken BionToken = Convert(bionReader.TokenType);
                AssertEqual(jsonReader, bionReader, jsonReader.TokenType, BionToken, "Token Type");

                switch (BionToken)
                {
                    case JsonToken.PropertyName:
                    case JsonToken.String:
                        AssertEqual(jsonReader, bionReader, (string)jsonReader.Value, bionReader.CurrentString(), "text value");
                        break;
                    case JsonToken.Integer:
                        AssertEqual(jsonReader, bionReader, (long)jsonReader.Value, bionReader.CurrentInteger(), "integer value");
                        break;
                    case JsonToken.Float:
                        AssertEqual(jsonReader, bionReader, (double)jsonReader.Value, bionReader.CurrentFloat(), "float value");
                        break;
                    case JsonToken.Boolean:
                        AssertEqual(jsonReader, bionReader, (bool)jsonReader.Value, bionReader.CurrentBool(), "bool value");
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
