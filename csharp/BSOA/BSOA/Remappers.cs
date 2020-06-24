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
        void RemoveValues(ArraySlice<T> slice, BitVector vector);

        // Replace all values >= 'remapFrom' with the swapped values in replaceWith.
        void RemapAbove(ArraySlice<T> slice, int remapFrom, int[] replaceWith);
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

        public void RemoveValues(ArraySlice<int> values, BitVector vector)
        {
            if (vector.Count == 0) { return; }

            int[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                vector[array[i]] = false;
            }
        }

        public void RemapAbove(ArraySlice<int> values, int remapFrom, int[] replaceWith)
        {
            int[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                if (array[i] >= remapFrom)
                {
                    array[i] = replaceWith[(array[i] - remapFrom)];
                }
            }
        }
    }

    public class ByteRemapper : IRemapper<byte>
    {
        public static IRemapper<byte> Instance = new ByteRemapper();

        private ByteRemapper()
        { }

        public void RemoveValues(ArraySlice<byte> values, BitVector vector)
        {
            byte[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                vector[array[i]] = false;
            }
        }

        public void RemapAbove(ArraySlice<byte> values, int remapFrom, int[] replaceWith)
        {
            byte[] array = values.Array;
            int end = values.Index + values.Count;

            for (int i = values.Index; i < end; ++i)
            {
                if (array[i] >= remapFrom)
                {
                    array[i] = (byte)replaceWith[(array[i] - remapFrom)];
                }
            }
        }
    }
}
