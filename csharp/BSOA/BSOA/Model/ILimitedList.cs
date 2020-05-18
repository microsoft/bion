using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Model
{
    /// <summary>
    ///  ILimitedList&lt;T&gt; provides the basic contract that Columns and Tables must support.
    /// </summary>
    /// <remarks>
    ///  ILimitedList&lt;T&gt; is like IList&lt;T&gt;, but restricts adding and removing items to the end.
    /// </remarks>
    /// <typeparam name="T">Type of List items</typeparam>
    public interface ILimitedList<T> : ILimitedList, IReadOnlyList<T>, ICollection<T>
    {
        new int Count { get; }

        // Support 'native' add (provide a new instance to fill out)
        T Add();

        // Support getter and setter
        new T this[int index] { get; set; }

        // Add IndexOf to find items; nicer implementation of Contains(T item);
        int IndexOf(T item);
    }

    /// <summary>
    ///  ILimitedList contains all non-type-specific members of ILimitedList&lt;T&gt;,
    ///  allowing containers of multiple columns to interact with them in limited ways.
    /// </summary>
    public interface ILimitedList : ICollection
    {
        // Report the count in the List (serialization skipped if empty)
        new int Count { get; }
        
        // IEnumerator GetEnumerator();
        // bool IsSynchronized { get; }
        // object SyncRoot { get; }

        // Return item type of list
        Type Type { get; }

        // Provide efficient swapping of items
        void Swap(int index1, int index2);

        // Provide removal, but only from end
        void RemoveFromEnd(int count);

        // Provide copy from another list on untyped interface
        void CopyItem(int toIndex, ILimitedList fromList, int fromIndex);
    }
}
