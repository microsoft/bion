using Newtonsoft.Json;
using System;

namespace BSOA.Json
{
    /// <summary>
    ///  PrimitiveArrayReader provides per-type conversion of values read from JsonReader (as longs)
    ///  into specific numeric types. 
    ///  DotNet can't compile (T)(long)_reader.Value and won't convert (T)(object)(long)_reader.Value at runtime.
    /// </summary>
    public static class PrimitiveArrayReader
    {
        public static void ReadArray<T>(this JsonReader reader, T[] array, int count)
        {
            if (typeof(T) == typeof(byte))
            {
                FillArray(reader, count, (byte[])(object)array);
            }
            else if(typeof(T) == typeof(char))
            {
                FillArray(reader, count, (char[])(object)array);
            }
            else if (typeof(T) == typeof(sbyte))
            {
                FillArray(reader, count, (sbyte[])(object)array);
            }
            else if (typeof(T) == typeof(ushort))
            {
                FillArray(reader, count, (ushort[])(object)array);
            }
            else if (typeof(T) == typeof(short))
            {
                FillArray(reader, count, (short[])(object)array);
            }
            else if (typeof(T) == typeof(int))
            {
                FillArray(reader, count, (int[])(object)array);
            }
            else if (typeof(T) == typeof(uint))
            {
                FillArray(reader, count, (uint[])(object)array);
            }
            else if (typeof(T) == typeof(long))
            {
                FillArray(reader, count, (long[])(object)array);
            }
            else if (typeof(T) == typeof(ulong))
            {
                FillArray(reader, count, (ulong[])(object)array);
            }
            else if (typeof(T) == typeof(float))
            {
                FillArray(reader, count, (float[])(object)array);
            }
            else if (typeof(T) == typeof(double))
            {
                FillArray(reader, count, (double[])(object)array);
            }
            else
            {
                throw new NotSupportedException($"JsonTreeReader ReadBlockArray not supported for type '{typeof(T).Name}");
            }
        }

        #region Type-Specific ReadArrays - identical code except cast type
        private static void FillArray(JsonReader reader, int count, byte[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (byte)(long)reader.Value;
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, char[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = ((string)reader.Value)[0];
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, sbyte[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (sbyte)(long)reader.Value;
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, short[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (short)(long)reader.Value;
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, ushort[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (ushort)(long)reader.Value;
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, int[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (int)(long)reader.Value;
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, uint[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (uint)(long)reader.Value;
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, long[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (long)reader.Value;
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, ulong[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (ulong)(long)reader.Value;
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, float[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (float)(double)reader.Value;
                reader.Read();
            }
        }

        private static void FillArray(JsonReader reader, int count, double[] array)
        {
            for (int i = 0; i < count; ++i)
            {
                array[i] = (double)reader.Value;
                reader.Read();
            }
        }
        #endregion
    }
}
