// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

using BSOA.Extensions;
using BSOA.IO;

namespace BSOA.Collections
{
    /// <summary>
    ///  BitVector provides set operations, tracking whether each item is included with a bit in an int[].
    ///  BitVector is an extremely compact way to represent a set from [0, Count).
    /// </summary>
    public class BitVector : IEnumerable<int>, ITreeSerializable
    {
        private const uint FirstBit = 0x1U << 31;
        private uint[] _array;

        public BitVector(bool defaultValue, int capacity)
        {
            DefaultValue = defaultValue;
            _array = null;
            Capacity = capacity;
            Count = (DefaultValue ? capacity : 0);
        }

        public bool DefaultValue { get; private set; }
        public int Count { get; private set; }
        public int Capacity { get; private set; }
        public uint[] Array => _array;

        public bool this[int index]
        {
            get
            {
                if (index < 0) { throw new IndexOutOfRangeException(nameof(index)); }
                if (_array == null || _array.Length <= (index >> 5)) { return DefaultValue; }
                return (_array[index >> 5] & (FirstBit >> (index & 31))) != 0UL;
            }
            set
            {
                if (index < 0) { throw new IndexOutOfRangeException(); }

                if (index >= Capacity)
                {
                    if (DefaultValue) { Count += (index + 1 - Capacity); }
                    Capacity = index + 1;
                }

                if (_array == null || _array.Length <= (index >> 5))
                {
                    if (value == DefaultValue) { return; }
                    ArrayExtensions.ResizeTo(ref _array, ((Capacity + 31) >> 5), (DefaultValue ? ~0U : 0U), minSize: 4);
                }

                if (value)
                {
                    Count += (int)(~_array[index >> 5] >> (31 - (index & 31)) & 1);
                    _array[index >> 5] |= (FirstBit >> (index & 31));
                }
                else
                {
                    Count -= (int)(_array[index >> 5] >> (31 - (index & 31)) & 1);
                    _array[index >> 5] &= ~(FirstBit >> (index & 31));
                }
            }
        }

        public void SetAll(bool value)
        {
            // Ensure array created
            ArrayExtensions.ResizeTo(ref _array, ((Capacity + 31) >> 5), (DefaultValue ? ~0U : 0U), minSize: 4);

            // Set everything
            uint toSet = (value ? ~0U : 0U);
            int blocksToSet = ((Capacity + 31) >> 5);
            for (int i = 0; i < blocksToSet; ++i)
            {
                _array[i] = toSet;
            }

            // Set values back to default on last segment, if needed
            if (value != DefaultValue)
            {
                int edge = (Capacity & 31);
                if (edge < 31)
                {
                    _array[blocksToSet - 1] ^= (~0U) >> edge;
                }
            }

            Count = (value ? Capacity : 0);
        }

        public void RemoveFromEnd(int count)
        {
            int newCapacity = Capacity - count;

            // Reset values above new capacity
            for (int i = newCapacity; i < Capacity; ++i)
            {
                this[i] = DefaultValue;
            }

            // Track reduced size
            Capacity = newCapacity;
            if (DefaultValue) { Count = Count - count; }
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new BitVectorEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new BitVectorEnumerator(this);
        }

        public bool Add(int item)
        {
            bool wasSet = this[item];
            this[item] = true;
            return (!wasSet);
        }

        public bool Contains(int item)
        {
            return this[item];
        }

        public bool Remove(int item)
        {
            bool wasSet = this[item];
            this[item] = false;
            return wasSet;
        }

        public void UnionWith(IEnumerable<int> other)
        {
            foreach (int item in other)
            {
                this[item] = true;
            }
        }

        public void ExceptWith(IEnumerable<int> other)
        {
            foreach (int item in other)
            {
                this[item] = false;
            }
        }

        public void Clear()
        {
            Count = 0;
            Capacity = 0;
            _array = null;
        }

        public void Trim()
        { }

        private static Dictionary<string, Setter<BitVector>> setters = new Dictionary<string, Setter<BitVector>>()
        {
            [Names.Count] = (r, me) => me.Count = r.ReadAsInt32(),
            [Names.Capacity] = (r, me) => me.Capacity = r.ReadAsInt32(),
            [Names.Array] = (r, me) => me._array = r.ReadBlockArray<uint>()

        };

        public void Read(ITreeReader reader)
        {
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();

            if (DefaultValue == false && Count == 0)
            {
                // If all values are default false, only write Capacity; Count defaults back to zero on read
                writer.Write(Names.Capacity, Capacity);
            }
            else if (DefaultValue == true && Count == Capacity)
            {
                // If all values are default true, only write Count+Capacity (Array will be re-created with all default)
                writer.Write(Names.Count, Count);
                writer.Write(Names.Capacity, Capacity);
            }
            else if (Capacity > 0)
            {
                writer.Write(Names.Count, Count);
                writer.Write(Names.Capacity, Capacity);
                writer.WritePropertyName(Names.Array);
                writer.WriteBlockArray(Array, 0, Math.Min(Array.Length, (Capacity + 31) >> 5));
            }

            writer.WriteEndObject();
        }
    }

    public struct BitVectorEnumerator : IEnumerator<int>
    {
        private BitVector _vector;
        public int Current { get; private set; }
        object IEnumerator.Current => Current;

        public BitVectorEnumerator(BitVector vector)
        {
            _vector = vector;
            Current = -1;
        }

        public void Reset()
        {
            Current = -1;
        }

        public bool MoveNext()
        {
            Current++;

            // Look for the next set bit
            for (; Current < _vector.Capacity; ++Current)
            {
                if (_vector[Current]) { return true; }
            }

            return false;
        }

        public void Dispose()
        { }
    }
}
