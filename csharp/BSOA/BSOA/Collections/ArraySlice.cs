// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

using BSOA.Extensions;
using BSOA.IO;

namespace BSOA.Collections
{
    public struct ArraySlice<T> : IReadOnlyList<T>, ITreeSerializable, IEquatable<ArraySlice<T>> where T : unmanaged, IEquatable<T>
    {
        public T[] Array { get; private set; }
        public int Index { get; private set; }
        public int Count { get; set; }
        public bool IsExpandable { get; private set; }

        public T this[int index] => Array[Index + index];

        public static ArraySlice<T> Empty = default;

        public ArraySlice(T[] array, int index = 0, int length = -1, bool isExpandable = false)
        {
            if (array == null) { throw new ArgumentNullException("array"); }
            if (length < 0) { length = array.Length - index; }
            if (index < 0 || index > array.Length) { throw new ArgumentOutOfRangeException("index"); }
            if (index + length > array.Length) { throw new ArgumentOutOfRangeException("length"); }

            Array = array;
            Index = index;
            Count = length;
            IsExpandable = isExpandable;
        }

        public void CopyTo(T[] other, int toIndex)
        {
            if (Count > 0)
            {
                System.Array.Copy(Array, Index, other, toIndex, Count);
            }
        }

        public void Trim()
        { }

        public void Clear()
        {
            Array = null;
            Index = 0;
            Count = 0;
            IsExpandable = false;
        }

        public void Read(ITreeReader reader)
        {
            Array = reader.ReadBlockArray<T>();
            Index = 0;
            Count = Array.Length;
            IsExpandable = false;
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteBlockArray(Array, Index, Count);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArraySliceEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ArraySliceEnumerator<T>(this);
        }

        public override int GetHashCode()
        {
            int hashCode = 17;

            for (int i = 0; i < Count; ++i)
            {
                hashCode = unchecked(hashCode * 31) + Array[Index + i].GetHashCode();
            }

            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj is ArraySlice<T>)
            {
                return Equals((ArraySlice<T>)obj);
            }
            else
            {
                return ReadOnlyListExtensions.AreEqual(this, obj as IReadOnlyList<T>);
            }
        }

        public bool Equals(ArraySlice<T> other)
        {
            if (this.Count != other.Count) { return false; }

            for (int i = 0; i < Count; ++i)
            {
                if (!Array[Index + i].Equals(other.Array[other.Index + i])) { return false; }
            }

            return true;
        }

        public static bool operator ==(ArraySlice<T> left, ArraySlice<T> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ArraySlice<T> left, ArraySlice<T> right)
        {
            return !(left == right);
        }
    }
}