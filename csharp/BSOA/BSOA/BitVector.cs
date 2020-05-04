using BSOA.Extensions;
using System.Collections;
using System.Collections.Generic;

namespace BSOA
{
    /// <summary>
    ///  BitVector provides set operations, tracking whether each item is included with a bit in an int[].
    ///  BitVector is an extremely compact way to represent a set from [0, Count).
    /// </summary>
    public class BitVector : IEnumerable<int>
    {
        private const uint FirstBit = 0x1U << 31;
        private bool _defaultValue;
        private uint[] _vector;

        public BitVector(bool defaultValue, int capacity)
        {
            _defaultValue = defaultValue;
            _vector = null;
            ArrayExtensions.ResizeTo(ref _vector, capacity, (_defaultValue ? ~0U : 0U));
            Capacity = capacity;
        }

        public int Capacity { get; private set; }
        public uint[] Vector => _vector;

        public bool this[int index]
        {
            get
            {
                if (index >= Capacity) { return _defaultValue; }
                return (_vector[index >> 5] & (FirstBit >> (index & 31))) != 0UL;
            }
            set
            {
                if (index >= Capacity)
                {
                    Capacity = index + 1;

                    if (_vector == null || _vector.Length <= index)
                    {
                        ArrayExtensions.ResizeTo(ref _vector, (Capacity / 32), (_defaultValue ? ~0U : 0U));
                    }
                }

                if (value)
                {
                    _vector[index >> 5] |= (FirstBit >> (index & 31));
                }
                else
                {
                    _vector[index >> 5] &= ~(FirstBit >> (index & 31));
                }
            }
        }

        public void Clear()
        {
            Capacity = 0;
            _vector = null;
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
