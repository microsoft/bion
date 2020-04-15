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
    /// </remarks>
    public interface ITreeReader : IDisposable
    {
        // Read the next token
        bool Read();

        // Identify the token and current byte offset in the file
        TreeToken TokenType { get; }
        long Position { get; }

        // Return the current token as a typed valued - boolean, string, long, and double must be supported
        // Unlike JsonReader, this does not read the next token.
        bool ReadAsBoolean();
        string ReadAsString();
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
        public static void ReadObject<T>(this ITreeReader reader, T instance, Dictionary<string, Setter<T>> setters)
        {
            if (reader.TokenType == TreeToken.None) { reader.Read(); }

            reader.Expect(TreeToken.StartObject);
            reader.Read();

            while (reader.TokenType == TreeToken.PropertyName)
            {
                string propertyName = reader.ReadAsString();
                reader.Read();

                if (!setters.TryGetValue(propertyName, out Setter<T> setter))
                {
                    throw new IOException($"{reader.GetType().Name} encountered unexpected property name parsing {nameof(T)}. Read \"{propertyName}\", expected one of \"{String.Join("; ", setters.Keys)}\"at {reader.Position:n0}");
                }

                setter(reader, instance);
                reader.Read();
            }

            reader.Expect(TreeToken.EndObject);
            // EndObject must be left for caller to handle
        }

        public static void Expect(this ITreeReader reader, TreeToken expected)
        {
            if (reader.TokenType != expected)
            {
                throw new IOException($"{reader.GetType().Name} expected \"{expected}\" but found \"{reader.TokenType}\" at {reader.Position:n0}");
            }
        }
    }
}
