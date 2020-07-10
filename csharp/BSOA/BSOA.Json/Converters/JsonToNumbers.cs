// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Numerics;

using Newtonsoft.Json;

namespace BSOA.Json.Converters
{
    public static class JsonToByte
    {
        public static byte Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static byte Read(JsonReader reader)
        {
            return (byte)(long)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, byte item, byte defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, byte item)
        {
            writer.WriteValue(item);
        }
    }

    public static class JsonToSbyte
    {
        public static sbyte Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static sbyte Read(JsonReader reader)
        {
            return (sbyte)(long)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, sbyte item, sbyte defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, sbyte item)
        {
            writer.WriteValue(item);
        }
    }

    public static class JsonToShort
    {
        public static short Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static short Read(JsonReader reader)
        {
            return (short)(long)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, short item, short defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, short item)
        {
            writer.WriteValue(item);
        }
    }

    public static class JsonToUshort
    {
        public static ushort Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static ushort Read(JsonReader reader)
        {
            return (ushort)(long)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, ushort item, ushort defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, ushort item)
        {
            writer.WriteValue(item);
        }
    }

    public static class JsonToInt
    {
        public static int Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static int Read(JsonReader reader)
        {
            return (int)(long)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, int item, int defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, int item)
        {
            writer.WriteValue(item);
        }
    }

    public static class JsonToUint
    {
        public static uint Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static uint Read(JsonReader reader)
        {
            return (uint)(long)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, uint item, uint defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, uint item)
        {
            writer.WriteValue(item);
        }
    }

    public static class JsonToLong
    {
        public static long Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static long Read(JsonReader reader)
        {
            return (long)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, long item, long defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, long item)
        {
            writer.WriteValue(item);
        }
    }

    public static class JsonToUlong
    {
        public static ulong Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static ulong Read(JsonReader reader)
        {
            if (reader.Value is BigInteger)
            {
                return (ulong)((BigInteger)reader.Value);
            }
            else
            {
                return (ulong)(long)reader.Value;
            }
        }

        public static void Write(JsonWriter writer, string propertyName, ulong item, ulong defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, ulong item)
        {
            writer.WriteValue(item);
        }
    }

    public static class JsonToFloat
    {
        public static float Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static float Read(JsonReader reader)
        {
            return (float)(double)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, float item, float defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, float item)
        {
            writer.WriteValue(item);
        }
    }

    public static class JsonToDouble
    {
        public static double Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static double Read(JsonReader reader)
        {
            return (double)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, double item, double defaultValue = default, bool required = false)
        {
            if (required || item != defaultValue)
            {
                writer.WritePropertyName(propertyName);
                writer.WriteValue(item);
            }
        }

        public static void Write(JsonWriter writer, double item)
        {
            writer.WriteValue(item);
        }
    }
}
