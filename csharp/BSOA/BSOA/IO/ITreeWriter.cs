using System;
using System.Collections.Generic;
using System.Linq;

namespace BSOA.IO
{
    /// <summary>
    ///  ITreeWriter is a generic interface for hierarchical serialization.
    ///  Types in a hierarchy can implement ITreeSerializable to support format-agnostic serialization.
    /// </summary>
    /// <remarks>
    ///  ITreeWriter supports a clean subset of Newtonsoft's JsonWriter and serialization
    ///  follows the same rules as JSON - one root element, objects have propertyName-value pairs,
    ///  arrays have values only.
    ///  
    ///  Following the JSON model makes it easy for classes to adapt JSON serialization support
    ///  into agnostic support, and developers don't have to learn a new mental model or new rules
    ///  for the serialization.
    ///  
    ///  ITreeWriter adds WriteBlockArray, which is critical for great I/O performance for large data.
    /// </remarks>
    public interface ITreeWriter : IDisposable
    {
        // ITreeWriter has the same Start/End Object and Array as JSON and JsonWriter
        void WriteStartObject();
        void WriteEndObject();
        void WriteStartArray();
        void WriteEndArray();

        // Objects must contain PropertyName/Value pairs, Arrays must contain bare values
        void WritePropertyName(string name);

        // The Null literal is supported
        void WriteNull();

        // Values of bool, string, long, and double must be directly supported.
        // Extension methods will translate other types in a consistent way.
        void WriteValue(bool value);
        void WriteValue(string value);
        void WriteValue(long value);
        void WriteValue(double value);

        /// <summary>
        ///  WriteBlockArray supports direct serialization of arrays of primitive types.
        /// </summary>
        /// <remarks>
        ///  ITreeWriters must implement support for:
        ///  char | byte | sbyte | short | ushort | int | uint | long | ulong | float | double
        ///  
        ///  ITreeWriters must handle null and empty arrays.
        /// </remarks>
        void WriteBlockArray<T>(T[] array, int index = 0, int count = -1) where T : unmanaged;
    }

    public static class TreeWriterExtensions
    {
        #region WriteValue for other types
        public static void WriteValue(this ITreeWriter writer, DateTime value)
        {
            long writtenValue = value.ToUniversalTime().Ticks;
            writer.WriteValue(writtenValue);
        }

        public static void WriteValue(this ITreeWriter writer, Guid value)
        {
            string writtenValue = value.ToString("D");
            writer.WriteValue(writtenValue);
        }
        #endregion

        #region Write methods write a PropertyName and Value in one call
        public static void Write(this ITreeWriter writer, string name, bool value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        public static void Write(this ITreeWriter writer, string name, string value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        public static void Write(this ITreeWriter writer, string name, long value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        public static void Write(this ITreeWriter writer, string name, double value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        public static void WriteBlockArray<T>(this ITreeWriter writer, string name, T[] array, int index = 0, int count = -1) where T : unmanaged
        {
            writer.WritePropertyName(name);
            writer.WriteBlockArray<T>(array, index, count);
        }

        public static void Write(this ITreeWriter writer, string name, DateTime value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }

        public static void Write(this ITreeWriter writer, string name, Guid value)
        {
            writer.WritePropertyName(name);
            writer.WriteValue(value);
        }
        #endregion

        public static void WriteList<T>(this ITreeWriter writer, IReadOnlyList<T> list) where T : ITreeSerializable
        {
            writer.WriteStartArray();

            foreach (T item in list)
            {
                item.Write(writer);
            }

            writer.WriteEndArray();
        }

        public static void WriteDictionary<T>(this ITreeWriter writer, IReadOnlyDictionary<string, T> dictionary) where T : ITreeSerializable
        {
            writer.WriteStartObject();

            foreach (var item in dictionary)
            {
                writer.WritePropertyName(item.Key);
                item.Value.Write(writer);
            }

            writer.WriteEndObject();
        }

        public static void WriteDictionary<T>(this ITreeWriter writer, IReadOnlyDictionary<int, T> dictionary) where T : ITreeSerializable
        {
            writer.WriteStartArray();

            int[] keys = dictionary.Keys.ToArray();
            writer.WriteBlockArray(keys);

            writer.WriteStartArray();

            for (int i = 0; i < keys.Length; ++i)
            {
                dictionary[keys[i]].Write(writer);
            }

            writer.WriteEndArray();

            writer.WriteEndArray();
        }

        // WriteObject supports writing a subcomponent with name and value in one call.
        public static void WriteObject(this ITreeWriter writer, string name, ITreeSerializable component)
        {
            if (component != null)
            {
                writer.WritePropertyName(name);
                component.Write(writer);
            }
        }
    }
}
