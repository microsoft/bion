using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Bion.Console
{
    public class JsonStatistics
    {
        public const int PropertyNameLengthCutoff = 32;

        public HashSet<string> ShortPropertyNames { get; private set; }
        public int LongPropertyNameCount { get; private set; }
        public int MaxDepth { get; private set; }

        private int CurrentDepth;

        public JsonStatistics(string jsonFilePath)
        {
            ShortPropertyNames = new HashSet<string>();
            Build(jsonFilePath);
        }

        private void Build(string jsonFilePath)
        {
            using (JsonTextReader reader = new JsonTextReader(new StreamReader(jsonFilePath)))
            {
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName)
                    {
                        string name = (string)reader.Value;
                        if (name.Length < PropertyNameLengthCutoff)
                        {
                            this.ShortPropertyNames.Add(name);
                        }
                        else
                        {
                            this.LongPropertyNameCount++;
                        }
                    }
                    
                    if (reader.TokenType == JsonToken.StartArray || reader.TokenType == JsonToken.StartObject)
                    {
                        CurrentDepth++;
                        if (CurrentDepth > MaxDepth) MaxDepth = CurrentDepth;
                    }
                    else if(reader.TokenType == JsonToken.EndArray || reader.TokenType == JsonToken.EndObject)
                    {
                        CurrentDepth--;
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"MaxDepth: {MaxDepth:n0}, LongPropertyCount: {LongPropertyNameCount:n0}, ShortPropertyCount: {ShortPropertyNames.Count:n0}";
        }
    }
}
