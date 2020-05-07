using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Console.Model
{
    public interface ILimitedList : ICollection
    {
        // int Count { get; }
        // IEnumerator GetEnumerator();

        // bool IsSynchronized { get; }
        // object SyncRoot { get; }

        // Return item type of list
        Type Type { get; }

        // Provide efficient swapping of items
        void Swap(int index1, int index2);

        // Provide removal, but only from end
        void RemoveFromEnd(int count);
    }

    public interface ILimitedList<T> : ILimitedList, IReadOnlyList<T>, ICollection<T>
    {
        // Support getter and setter
        new T this[int index] { get; set; }

        // Add IndexOf to find items; nicer implementation of Contains(T item);
        int IndexOf(T item);
    }

    public abstract class LimitedList<T> : ILimitedList<T>
    {
        public int Count { get; protected set; }

        public Type Type => typeof(T);
        public bool IsSynchronized => false;
        public object SyncRoot => null;
        public bool IsReadOnly => false;

        public abstract T this[int index] { get; set; }
        public abstract void Clear();
        public abstract void Swap(int index1, int index2);
        public abstract void RemoveFromEnd(int count);

        public void Add(T item)
        {
            this[Count++] = item;
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        public int IndexOf(T item)
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

        public void CopyTo(Array array, int index)
        {
            T[] typed = array as T[];
            if (typed == null) { throw new ArgumentException(nameof(array)); }
            CopyTo(typed, index);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null) { throw new ArgumentNullException(nameof(array)); }
            if (arrayIndex < 0) { throw new ArgumentOutOfRangeException(nameof(arrayIndex)); }
            if (array.Length < arrayIndex + Count) { throw new ArgumentException(nameof(array)); }

            for (int i = 0; i < Count; ++i)
            {
                array[arrayIndex + i] = this[i];
            }
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
