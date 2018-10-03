using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Bion
{
    public class LookupEntry
    {
        public string PropertyName;
        public ushort Index;
        public List<string> Values;

        public LookupEntry(string propertyName, ushort index)
        {
            this.PropertyName = propertyName;
            this.Index = index;
            this.Values = null;
        }

        public void AddValue(string value)
        {
            if (Values == null) Values = new List<string>();
            Values.Add(value);
        }
    }

    public class LookupDictionary
    {
        public const int PropertyNameLengthCutoff = 32;
        public const int PropertyNameCountLimit = 16383;

        private List<LookupEntry> _lookupArray;
        private Dictionary<string, LookupEntry> _lookupDictionary;

        public LookupDictionary()
        {
            _lookupArray = new List<LookupEntry>();
            _lookupDictionary = new Dictionary<string, LookupEntry>();
        }

        public bool TryLookup(string propertyName, out ushort index)
        {
            index = ushort.MaxValue;
            
            // If already indexed, return index
            if(_lookupDictionary.TryGetValue(propertyName, out LookupEntry value))
            {
                index = value.Index;
                return true;
            }

            // If property is too long, don't add it to the lookup
            if (propertyName.Length > PropertyNameLengthCutoff) return false;

            // If lookup is full, fail
            ushort currentCount = (ushort)_lookupDictionary.Count;
            if (currentCount >= PropertyNameCountLimit) return false;

            // Add and return new index
            LookupEntry entry = new LookupEntry(propertyName, currentCount);
            _lookupDictionary.Add(propertyName, entry);
            _lookupArray.Add(entry);
            index = currentCount;
            return true;
        }

        public void Write(Stream stream)
        {
            using (JsonTextWriter writer = new JsonTextWriter(new StreamWriter(stream)))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                foreach(LookupEntry entry in _lookupArray)
                {
                    writer.WritePropertyName(entry.PropertyName);
                    writer.WriteStartArray();

                    if(entry.Values != null)
                    {
                        foreach(string value in entry.Values)
                        {
                            writer.WriteValue(value);
                        }
                    }

                    writer.WriteEndArray();
                }

                writer.WriteEndObject();
            }
        }

        public void Read(Stream stream)
        {
            using (JsonTextReader reader = new JsonTextReader(new StreamReader(stream)))
            {
                // StartObject
                reader.Read();

                ushort countRead = 0;
                while(reader.Read())
                {
                    if (reader.TokenType != JsonToken.PropertyName) throw new InvalidDataException();
                    LookupEntry entry = new LookupEntry((string)reader.Value, countRead);

                    // StartArray
                    reader.Read();

                    // EndArray
                    reader.Read();
                    while(reader.TokenType != JsonToken.EndArray)
                    {
                        entry.AddValue((string)reader.Value);
                        reader.Read();
                    }

                    _lookupArray.Add(entry);
                    _lookupDictionary.Add(entry.PropertyName, entry);
                    countRead++;
                }
            }
        }
    }
}
