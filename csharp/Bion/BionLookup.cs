using System;
using System.Collections.Generic;
using System.IO;

namespace Bion
{
    /// <summary>
    ///  BionLookup implements the Lookup Dictionary part of BION,
    ///  which is a set of Property Names and per-Property Name values
    ///  which can be referred to be indices.
    /// </summary>
    public class BionLookup : IDisposable
    {
        public const int PropertyNameLengthLimit = 32;
        public const int PropertyNameCountLimit = 16384;

        public const int ValueLengthLimit = 128;
        public const int ValueCountLimit = 64;

        private List<LookupEntry> _lookupArray;
        private Dictionary<string, LookupEntry> _lookupDictionary;
        private Stream _writeToStream;

        public bool IsReadOnly { get; private set; }

        /// <summary>
        ///  Build a writeable LookupDictionary for the current document.
        ///  Instances constructed this way must be serialized by the caller
        ///  after they have been populated.
        /// </summary>
        private BionLookup() : this(false, null)
        { }

        private BionLookup(bool isReadOnly, Stream writeToStream)
        {
            _lookupArray = new List<LookupEntry>();
            _lookupDictionary = new Dictionary<string, LookupEntry>();
            _writeToStream = writeToStream;
            IsReadOnly = isReadOnly;
        }

        /// <summary>
        ///  Load a read only LookupDictionary from an existing stream.
        /// </summary>
        /// <param name="stream">Stream to read from</param>
        public static BionLookup OpenRead(Stream stream)
        {
            BionLookup result = new BionLookup(true, null);
            using (BionReader reader = new BionReader(stream))
            {
                result.Read(reader);
            }
            return result;
        }

        /// <summary>
        ///  Load a read only LookupDictionary from an existing file.
        /// </summary>
        /// <param name="filePath">File Path to read from</param>
        public static BionLookup OpenRead(string filePath)
        {
            return BionLookup.OpenRead(new FileStream(filePath, FileMode.Open));
        }

        /// <summary>
        ///  Create a writeable LookupDictionary to be written to the target stream.
        /// </summary>
        /// <param name="stream">Stream to write to</param>
        public static BionLookup OpenWrite(Stream stream)
        {
            return new BionLookup(false, stream);
        }

        /// <summary>
        ///  Create a writeable LookupDictionary to be written to the target file.
        /// </summary>
        /// <param name="filePath">File Path to write to</param>
        public static BionLookup OpenWrite(string filePath)
        {
            return BionLookup.OpenWrite(new FileStream(filePath, FileMode.Create));
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

        /// <summary>
        ///  Get the Property Name for the given lookup index.
        /// </summary>
        /// <param name="index">Index of Property Name</param>
        /// <returns>Property Name for index</returns>
        public string PropertyName(short index)
        {
            if (index < 0 || index >= _lookupArray.Count)
            {
                throw new ArgumentOutOfRangeException("index");
            }
            return _lookupArray[index].PropertyName;
        }

        /// <summary>
        ///  Get the Value for the given Property Name and Value indices.
        /// </summary>
        /// <param name="propertyIndex">Index of Property Name</param>
        /// <param name="valueIndex">Index of Value</param>
        /// <returns>Value for index combination</returns>
        public string Value(short propertyIndex, short valueIndex)
        {
            if (propertyIndex < 0 || propertyIndex >= _lookupArray.Count) throw new ArgumentOutOfRangeException("propertyName");

            LookupEntry entry = _lookupArray[propertyIndex];
            if (valueIndex < 0 || valueIndex >= entry.ValueCount) throw new ArgumentOutOfRangeException("index");
            return entry.Values[valueIndex];
        }

        /// <summary>
        ///  Serialize this Lookup via a BionWriter
        /// </summary>
        /// <param name="writer">BionWriter to write to</param>
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

        /// <summary>
        ///  Serialize this Lookup to the provided Stream
        /// </summary>
        /// <param name="stream">Stream to write to</param>
        public void Write(Stream stream)
        {
            // Write, making sure not to have yet another nested LookupDictionary
            using (BionWriter writer = new BionWriter(stream, lookupDictionary: null))
            {
                Write(writer);
            }
        }

        /// <summary>
        ///  Deserialize this Lookup from a BionReader
        /// </summary>
        /// <param name="reader">BionReader to read from</param>
        public void Read(BionReader reader)
        {
            reader.Read(BionToken.StartObject);

            short countRead = 0;
            while(reader.Read())
            {
                if (reader.TokenType == BionToken.EndObject) break;

                reader.Expect(BionToken.PropertyName);
                LookupEntry entry = new LookupEntry(reader.CurrentString(), countRead);

                reader.Read(BionToken.StartArray);

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

        public void Dispose()
        {
            if(_writeToStream != null)
            {
                Write(_writeToStream);
                _writeToStream.Dispose();
                _writeToStream = null;
            }
        }

        private class LookupEntry
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
    }
}
