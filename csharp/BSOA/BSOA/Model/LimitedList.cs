// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections;
using System.Collections.Generic;

using BSOA.Collections;
using BSOA.Extensions;

namespace BSOA.Model
{
    /// <summary>
    ///  LimitedList provides base implementations of all ILimitedList&lt;T&gt;
    ///  members possible.
    /// </summary>
    /// <typeparam name="T">Type of Items in list</typeparam>
    public abstract class LimitedList<T> : ILimitedList<T>
    {
        public Type Type => typeof(T);
        public bool IsSynchronized => false;
        public object SyncRoot => null;
        public bool IsReadOnly => false;

        // Descendants must implement these minimal members
        public abstract int Count { get; }
        public abstract T this[int index] { get; set; }
        public abstract void Clear();
        public abstract void RemoveFromEnd(int count);

        public virtual T Add()
        {
            return this[Count];
        }

        public virtual void Add(T item)
        {
            this[Count] = item;
        }

        public virtual bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public virtual void Swap(int index1, int index2)
        {
            T value1 = this[index1];
            this[index1] = this[index2];
            this[index2] = value1;
        }

        public virtual void CopyItem(int toIndex, ILimitedList fromList, int fromIndex)
        {
            ILimitedList<T> other = fromList as ILimitedList<T>;
            if (other == null) { throw new ArgumentException(nameof(fromList)); }

            this[toIndex] = other[fromIndex];
        }

        public virtual int IndexOf(T item)
        {
            for (int i = 0; i < Count; ++i)
            {
                if (item.Equals(this[i])) { return i; }
            }

            return -1;
        }

        public bool Contains(T item)
        {
            return (IndexOf(item) != -1);
        }

        public virtual void CopyTo(Array array, int index)
        {
            T[] typed = array as T[];
            if (typed == null) { throw new ArgumentException(nameof(array)); }
            EnumerableExtensions.CopyTo(this, this.Count, typed, index);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            EnumerableExtensions.CopyTo(this, this.Count, array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }
    }
}
