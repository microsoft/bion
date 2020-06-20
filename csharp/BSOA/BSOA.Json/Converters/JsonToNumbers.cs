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

        public static void Write(JsonWriter writer, string propertyName, byte item, byte defaultValue = default)
        {
            if (item != defaultValue)
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

    public static class JsonToUShort
    {
        public static ushort Read<TRoot>(JsonReader reader, TRoot root)
        {
            return Read(reader);
        }

        public static ushort Read(JsonReader reader)
        {
            return (ushort)(long)reader.Value;
        }

        public static void Write(JsonWriter writer, string propertyName, ushort item, ushort defaultValue = default)
        {
            if (item != defaultValue)
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

        public static void Write(JsonWriter writer, string propertyName, int item, int defaultValue = default)
        {
            if (item != defaultValue)
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

        public static void Write(JsonWriter writer, string propertyName, long item, long defaultValue = default)
        {
            if (item != defaultValue)
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

        public static void Write(JsonWriter writer, string propertyName, float item, float defaultValue = default)
        {
            if (item != defaultValue)
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

        public static void Write(JsonWriter writer, string propertyName, double item, double defaultValue = default)
        {
            if (item != defaultValue)
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
