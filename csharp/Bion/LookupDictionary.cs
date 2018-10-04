using System;
using System.Collections.Generic;
using System.IO;

namespace Bion
{
    internal class LookupEntry
    {
        public string PropertyName;
        public short Index;
        public List<string> Values;
        public short ValueCount => (short)(Values?.Count ?? 0);

        public LookupEntry(string propertyName, short index)
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
        public const int PropertyNameLengthLimit = 32;
        public const int PropertyNameCountLimit = 16384;

        public const int ValueLengthLimit = 64;
        public const int ValueCountLimit = 64;

        private List<LookupEntry> _lookupArray;
        private Dictionary<string, LookupEntry> _lookupDictionary;

        public bool IsReadOnly { get; private set; }

        /// <summary>
        ///  Build a writeable LookupDictionary for the current document.
        /// </summary>
        public LookupDictionary()
        {
            _lookupArray = new List<LookupEntry>();
            _lookupDictionary = new Dictionary<string, LookupEntry>();
            IsReadOnly = false;
        }

        /// <summary>
        ///  Load a read only LookupDictionary from an existing stream.
        /// </summary>
        /// <param name="stream"></param>
        public LookupDictionary(Stream stream) : this()
        {
            using (BionReader reader = new BionReader(stream))
            {
                Read(reader);
                IsReadOnly = true;
            }
        }

        /// <summary>
        ///  Return the lookup index for a Property Name, if available
        /// </summary>
        /// <param name="propertyName">Property Name to look up</param>
        /// <param name="index">Index of Property Name, -1 if not indexed</param>
        /// <returns>True if indexed, False if not in index and couldn't be added</returns>
        public bool TryLookup(string propertyName, out short index)
        {
            index = -1;
            if (propertyName == null) return false;
            
            // If already indexed, return index
            if(_lookupDictionary.TryGetValue(propertyName, out LookupEntry value))
            {
                index = value.Index;
                return true;
            }

            // Don't add an entry if this is ReadOnly, the Property Name is too long, or the Lookup is full
            if (IsReadOnly) return false;
            if (propertyName.Length > PropertyNameLengthLimit) return false;
            if (_lookupDictionary.Count >= PropertyNameCountLimit) return false;

            // Add and return new index
            LookupEntry entry = new LookupEntry(propertyName, (short)_lookupDictionary.Count);
            _lookupDictionary.Add(propertyName, entry);
            _lookupArray.Add(entry);
            index = entry.Index;
            return true;
        }

        /// <summary>
        ///  Return the lookup index for a value, if available
        /// </summary>
        /// <param name="propertyName">Property Name for value to look up</param>
        /// <param name="value">Value to look up</param>
        /// <param name="index">Index of Value, -1 if not indexed</param>
        /// <returns>True if indexed, False if not in index and couldn't be added</returns>
        public bool TryLookup(string propertyName, string value, out short index)
        {
            index = -1;
            if (propertyName == null || value == null) return false;

            // Look for Property Name entry
            if (!_lookupDictionary.TryGetValue(propertyName, out LookupEntry entry)) return false;

            // Look for value in entry; return existing index if found
            if (entry.Values != null)
            {
                index = (short)entry.Values.IndexOf(value);
                if (index >= 0) return true;
            }

            // Don't add a value if this is ReadOnly, the value is too long, or the Lookup is full
            if (IsReadOnly) return false;
            if (value.Length > ValueLengthLimit) return false;
            if (entry.ValueCount >= ValueCountLimit) return false;

            // Add and return new index
            index = entry.ValueCount;
            entry.AddValue(value);
            return true;
        }

        public string PropertyName(short index)
        {
            if (index < 0 || index >= _lookupArray.Count) throw new ArgumentOutOfRangeException("index");
            return _lookupArray[index].PropertyName;
        }

        public string Value(string propertyName, short index)
        {
            if (!_lookupDictionary.TryGetValue(propertyName, out LookupEntry entry)) throw new ArgumentOutOfRangeException("propertyName");
            if (index < 0 || index >= entry.ValueCount) throw new ArgumentOutOfRangeException("index");
            return entry.Values[index];
        }

        public void Write(BionWriter writer)
        {
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

        public void Write(Stream stream)
        {
            // Write, making sure not to have yet another nested LookupDictionary
            using (BionWriter writer = new BionWriter(stream, lookupStream: null))
            {
                Write(writer);
            }
        }

        public void Read(BionReader reader)
        {
            reader.Read();
            reader.Expect(BionToken.StartObject);

            short countRead = 0;
            while(reader.Read())
            {
                if (reader.TokenType == BionToken.EndObject) break;

                reader.Expect(BionToken.PropertyName);
                LookupEntry entry = new LookupEntry(reader.CurrentString(), countRead);

                reader.Read();
                reader.Expect(BionToken.StartArray);

                reader.Read();
                while(reader.TokenType != BionToken.EndArray)
                {
                    reader.Expect(BionToken.String);
                    entry.AddValue(reader.CurrentString());
                    reader.Read();
                }

                _lookupArray.Add(entry);
                _lookupDictionary.Add(entry.PropertyName, entry);
                countRead++;
            }
        }

        public void Read(Stream stream)
        {
            // Read, making sure not to have yet another nested LookupDictionary
            using (BionReader reader = new BionReader(stream, lookupStream: null))
            {
                Read(reader);
            }
        }
    }
}
