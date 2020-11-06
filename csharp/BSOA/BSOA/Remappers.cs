// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using BSOA.Collections;

namespace BSOA
{
    /// <summary>
    ///  IRemappers collect distinct values and provide remapping of them (replacement of them)
    ///  to enable garbage collection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRemapper<T> where T : unmanaged, IEquatable<T>
    {
        // Remove all values found in slice from vector (to find remaining unused values)
        void AddValues(ArraySlice<T> slice, BitVector vector);

        // Replace all values in slice with replacements in array
        void Remap(ArraySlice<T> slice, int[] replacements);
    }

    public static class RemapperFactory
    {
        public static IRemapper<T> Build<T>() where T : unmanaged, IEquatable<T>
        {
            if (typeof(T) == typeof(int)) { return (IRemapper<T>)IntRemapper.Instance; }
            if (typeof(T) == typeof(byte)) { return (IRemapper<T>)ByteRemapper.Instance; }
            throw new NotImplementedException($"IRemapper is not implemented for {typeof(T).Name}.");
        }
    }

    public class IntRemapper : IRemapper<int>
    {
        public static IRemapper<int> Instance = new IntRemapper();

        private IntRemapper()
        { }

        public void AddValues(ArraySlice<int> values, BitVector vector)
        {
            if (vector.Count == 0) { return; }

            int[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                vector[array[i]] = true;
            }
        }

        public void Remap(ArraySlice<int> values, int[] replacements)
        {
            int[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                int value = array[i];
                if (value >= 0 && value < replacements.Length)
                {
                    array[i] = replacements[value];
                }
            }
        }
    }

    public class ByteRemapper : IRemapper<byte>
    {
        public static IRemapper<byte> Instance = new ByteRemapper();

        private ByteRemapper()
        { }

        public void AddValues(ArraySlice<byte> values, BitVector vector)
        {
            byte[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                vector[array[i]] = true;
            }
        }

        public void Remap(ArraySlice<byte> values, int[] replacements)
        {
            byte[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                int value = array[i];
                if (value >= 0 && value < replacements.Length)
                {
                    array[i] = (byte)replacements[value];
                }
            }
        }
    }
}
