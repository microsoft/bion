using System;
using System.Collections.Generic;
using System.IO;

namespace BSOA.IO
{
    public delegate void Setter<T>(ITreeReader reader, T me);

    /// <summary>
    ///  ITreeReader is a generic interface for hierarchical deserialization.
    ///  Types in a hierarchy can implement ITreeSerializable to support format-agnostic serialization.
    /// </summary>
    /// <remarks>
    ///  ITreeReader supports a clean subset of Newtonsoft's JsonReader and serialization
    ///  follows the same rules as JSON - one root element, objects have propertyName-value pairs,
    ///  arrays have values only.
    ///  
    ///  ITreeReader adds ReadBlockArray, which is critical for great I/O performance for large data
    ///  
    ///  ROUGH EDGES:
    ///   - ReadAs different from JsonReader (don't read next token; need a typed way to get current token value)
    ///   - ReadBlockArray must be called before next Read(), but other calls don't have to be.
    ///   - ReadObject needs a way to set defaults for missing and null properties.
    ///   - Probably want a way to read structs (can't pass instance to setter to set).
    ///   - ReadDictionary/ReadArray helpers, if written, can't support both primitives and nested ITreeSerializable types easily.
    ///   - Probably want serializability implemented in a companion class (to allow implementing for built-in types)
    /// </remarks>
    public interface ITreeReader : IDisposable
    {
        // Expose settings for Reader, so specific serialization can consider them
        TreeSerializationSettings Settings { get; }

        // Read the next token
        bool Read();

        // Identify the token and current byte offset in the file
        TreeToken TokenType { get; }
        long Position { get; }

        // Return the current token as a typed valued - boolean, string, int, long, and double must be supported
        // Unlike JsonReader, this does not read the next token.
        bool ReadAsBoolean();
        string ReadAsString();
        int ReadAsInt32();
        long ReadAsInt64();
        double ReadAsDouble();

        /// <summary>
        ///  ReadBlockArray supports direct deserialization of arrays of primitive types.
        /// </summary>
        /// <remarks>
        ///  ITreeReaders must implement support for:
        ///  char | byte | sbyte | short | ushort | int | uint | long | ulong | float | double
        ///  
        ///  ITreeReaders must handle null and empty arrays.
        ///  ReadBlockArray should leave one token, so that Read() called afterward advances to the next token.
        /// </remarks>
        T[] ReadBlockArray<T>() where T : unmanaged;
    }

    public static class TreeReaderExtensions
    {
        #region ReadValue for other types
        public static short ReadAsInt16(this ITreeReader reader)
        {
            return (short)reader.ReadAsInt64();
        }

        public static int ReadAsInt32(this ITreeReader reader)
        {
            return (int)reader.ReadAsInt64();
        }

        public static DateTime ReadAsDateTime(this ITreeReader reader)
        {
            long utcTicks = reader.ReadAsInt64();
            return new DateTime(utcTicks, DateTimeKind.Utc);
        }

        public static Guid ReadAsGuid(this ITreeReader reader)
        {
            string writtenValue = reader.ReadAsString();
            return Guid.Parse(writtenValue);
        }
        #endregion

        public static List<T> ReadList<T>(this ITreeReader reader, Func<T> ctor) where T : ITreeSerializable
        {
            if (reader.TokenType == TreeToken.Null) { return null; }
            List<T> result = new List<T>();

            reader.Expect(TreeToken.StartArray);
            reader.Read();

            while (reader.TokenType != TreeToken.EndArray)
            {
                T item = ctor();
                item.Read(reader);

                result.Add(item);

                reader.Read();
            }

            return result;
        }

        public static Dictionary<string, T> ReadStringDictionary<T>(this ITreeReader reader) where T : ITreeSerializable, new()
        {
            return ReadStringDictionary<T>(reader, () => new T());
        }

        public static Dictionary<string, T> ReadStringDictionary<T>(this ITreeReader reader, Func<T> ctor) where T : ITreeSerializable
        {
            if (reader.TokenType == TreeToken.Null) { return null; }
            Dictionary<string, T> result = new Dictionary<string, T>();

            reader.Expect(TreeToken.StartObject);
            reader.Read();

            while (reader.TokenType == TreeToken.PropertyName)
            {
                string key = reader.ReadAsString();
                reader.Read();

                T value = ctor();
                value.Read(reader);

                result[key] = value;

                reader.Read();
            }

            reader.Expect(TreeToken.EndObject);
            return result;
        }

        public static Dictionary<int, T> ReadIntDictionary<T>(this ITreeReader reader) where T : ITreeSerializable, new()
        {
            return ReadIntDictionary<T>(reader, () => new T());
        }

        public static Dictionary<int, T> ReadIntDictionary<T>(this ITreeReader reader, Func<T> ctor) where T : ITreeSerializable
        {
            if (reader.TokenType == TreeToken.Null) { return null; }
            Dictionary<int, T> result = new Dictionary<int, T>();

            reader.Expect(TreeToken.StartArray);
            reader.Read();

            int[] keys = reader.ReadBlockArray<int>();

            reader.Read();
            reader.Expect(TreeToken.StartArray);
            reader.Read();

            for (int i = 0; i < keys.Length; ++i)
            {
                int key = keys[i];

                T value = ctor();
                value.Read(reader);

                result[key] = value;

                reader.Read();
            }

            reader.Expect(TreeToken.EndArray);
            reader.Read();

            reader.Expect(TreeToken.EndArray);
            return result;
        }

        /// <summary>
        ///  Read existing items in an existing Dictionary instance.
        ///  Used with Dictionaries of specific things which may or may not be present in the file, like Table.Columns.
        /// </summary>
        /// <typeparam name="T">Type of values in Dictionary</typeparam>
        /// <param name="reader">ITreeReader to read from</param>
        /// <param name="dictionary">Dictionary containing items to read</param>
        /// <param name="throwOnUnknown">True to throw for property name not in Dictionary, false to quietly skip over it</param>
        public static void ReadDictionaryItems<T>(this ITreeReader reader, Dictionary<string, T> dictionary, bool throwOnUnknown = true) where T : ITreeSerializable
        {
            if (reader.TokenType == TreeToken.Null) { return; }

            reader.Expect(TreeToken.StartObject);
            reader.Read();

            while (reader.TokenType == TreeToken.PropertyName)
            {
                string itemName = reader.ReadAsString();
                reader.Read();

                if (dictionary.TryGetValue(itemName, out T item))
                {
                    item.Read(reader);
                    reader.Read();
                }
                else
                {
                    if (throwOnUnknown)
                    {
                        throw new IOException($"Found unknown {typeof(T).Name} property \"{itemName}\", expected one of \"{String.Join("; ", dictionary.Keys)}\" at {reader.Position:n0} using {reader.GetType().Name}.");
                    }
                    else
                    {
                        reader.Skip();
                    }
                }
            }

            reader.Expect(TreeToken.EndObject);
        }

        /// <summary>
        ///  ReadObject wraps the loop to read each property in an object and call the corresponding
        ///  setter to set it.
        /// </summary>
        /// <remarks>
        ///  This works for classes only, as the instance can't be passed by ref.
        ///  Structs can serialize as arrays or directly implement the loop to decode themselves.
        ///  Setters take a reference to the instance so that the Dictionary can be static per type,
        ///  which is critical for acceptable performance.
        /// </remarks>
        /// <typeparam name="T">Type being deserialized</typeparam>
        /// <param name="reader">ITreeReader being read from</param>
        /// <param name="instance">T instance being initialized</param>
        /// <param name="setters">Dictionary of setter per field name</param>
        /// <param name="throwOnUnknown">Throw if property name not in setters found</param>
        public static void ReadObject<T>(this ITreeReader reader, T instance, Dictionary<string, Setter<T>> setters, bool throwOnUnknown = true) where T : ITreeSerializable
        {
            // Ensure object state reset before Read
            instance.Clear();

            // Null means default state
            if (reader.TokenType == TreeToken.Null) { return; }

            reader.Expect(TreeToken.StartObject);
            reader.Read();

            while (reader.TokenType == TreeToken.PropertyName)
            {
                string propertyName = reader.ReadAsString();
                reader.Read();

                if (setters.TryGetValue(propertyName, out Setter<T> setter))
                {
                    setter(reader, instance);
                    reader.Read();
                }
                else
                {
                    if (throwOnUnknown)
                    {
                        throw new IOException($"Found unknown {typeof(T).Name} property, \"{propertyName}\", expected one of \"{String.Join("; ", setters.Keys)}\" at {reader.Position:n0} using {reader.GetType().Name}.");
                    }
                    else
                    {
                        reader.Skip();
                    }
                }
            }

            reader.Expect(TreeToken.EndObject);
            // EndObject must be left for caller to handle
        }

        /// <summary>
        ///  Skip the current token. 
        ///  Reads a single token, or skips over an entire subtree for containers.
        /// </summary>
        /// <param name="reader">ITreeReader to skip next token from</param>
        public static void Skip(this ITreeReader reader)
        {
            int depth = 0;

            do
            {
                switch (reader.TokenType)
                {
                    case TreeToken.StartArray:
                    case TreeToken.StartObject:
                        depth++;
                        break;
                    case TreeToken.EndArray:
                    case TreeToken.EndObject:
                        depth--;
                        break;
                }

                reader.Read();
            } while (depth > 0);
        }

        /// <summary>
        ///  Verify the current token is a required value, otherwise throw a good exception.
        /// </summary>
        /// <param name="reader">ITreeReader to check</param>
        /// <param name="expected">Current TreeToken expected</param>
        public static void Expect(this ITreeReader reader, TreeToken expected)
        {
            if (reader.TokenType != expected)
            {
                throw new IOException($"{reader.GetType().Name} expected \"{expected}\" but found \"{reader.TokenType}\" at {reader.Position:n0}");
            }
        }
    }
}
