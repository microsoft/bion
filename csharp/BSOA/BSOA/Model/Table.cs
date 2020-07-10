// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading;

using BSOA.IO;

namespace BSOA.Model
{
    /// <summary>
    ///  BSOA Table is the container class for all SoA model types.
    ///  Each type is a struct referencing an index in a table, and each property in the struct
    ///  references the value at that index in the matching column.
    /// </summary>
    /// <remarks>
    ///  See BSOA.Demo
    /// </remarks>
    /// <typeparam name="T">SoA Item type this is a Table of</typeparam>
    public abstract class Table<T> : LimitedList<T>, ITable<T> where T : IRow<T>
    {
        private int _count;
        protected Dictionary<string, IColumn> Columns { get; private set; }

        protected Table()
        {
            Columns = new Dictionary<string, IColumn>();
        }

        public abstract T Get(int index);

        public override int Count => _count;

        public override T this[int index]
        {
            get
            {
                if (index < 0) { throw new IndexOutOfRangeException(); }
                return Get(index);
            }

            set
            {
                if (index < 0) { throw new IndexOutOfRangeException(); }
                if (index >= Count) { _count = index + 1; }
                Get(index).CopyFrom(value);
            }
        }

        /// <summary>
        ///  Get index in this table for item.
        ///  Copies item if it's from another database.
        /// </summary>
        /// <param name="value">Item to get local index for</param>
        /// <returns>Existing index for local item, new index for copied item, -1 for null</returns>
        public int LocalIndex(T value)
        {
            if (value == null) { return -1; }

            if (object.ReferenceEquals(value.Table, this))
            {
                // Already here
                return value.Index;
            }
            else
            {
                // Copy from other table
                T result = Add();
                result.CopyFrom(value);

                return result.Index;
            }
        }



        /// <summary>
        ///  Add a new item to the end of this table.
        /// </summary>
        /// <returns>New SoA item instance</returns>
        public override T Add()
        {
            int newIndex = Interlocked.Increment(ref _count) - 1;
            return Get(newIndex);
        }

        public override void Add(T item)
        {
            if (!object.ReferenceEquals(item.Table, this) || item.Index != Count - 1)
            {
                Add().CopyFrom(item);
            }
        }

        /// <summary>
        ///  Add a Column to the table set.
        /// </summary>
        /// <remarks>
        ///  Typed tables should add columns to determine types, define default values, and similar.
        ///  Typed tables should have hardcoded properties per column for fastest use by the item types.
        ///  Columns must be provided to the table for it to facilitate serialization of them.
        /// </remarks>
        /// <typeparam name="U">Type of Column being added</typeparam>
        /// <param name="name">Name of column</param>
        /// <param name="column">IColumn instance for column</param>
        /// <returns>Column instance, for easy assignment to hardcoded properties</returns>
        protected U AddColumn<U>(string name, U column) where U : IColumn
        {
            Columns[name] = column;
            return column;
        }

        public override void Swap(int index1, int index2)
        {
            foreach (IColumn column in Columns.Values)
            {
                column.Swap(index1, index2);
            }
        }

        public override void RemoveFromEnd(int count)
        {
            int newCount = (Count - count);
            foreach (IColumn column in Columns.Values)
            {
                if (column.Count > newCount)
                {
                    column.RemoveFromEnd(column.Count - newCount);
                }
            }

            _count = newCount;
        }

        /// <summary>
        ///  Remove all items from table.
        /// </summary>
        public override void Clear()
        {
            _count = 0;

            foreach (IColumn column in Columns.Values)
            {
                column.Clear();
            }
        }

        /// <summary>
        ///  Remove excess capacity and prepare for serialization
        /// </summary>
        public void Trim()
        {
            foreach (IColumn column in Columns.Values)
            {
                column.Trim();
            }
        }

        private static Dictionary<string, Setter<Table<T>>> setters = new Dictionary<string, Setter<Table<T>>>()
        {
            [Names.Count] = (r, me) => me._count = r.ReadAsInt32(),
            [Names.Columns] = (r, me) => r.ReadDictionaryItems(me.Columns, throwOnUnknown: r.Settings.Strict),
        };

        public void Read(ITreeReader reader)
        {
            // Read Columns, skipping unknown columns if Settings.Strict == false
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();

            writer.Write(Names.Count, Count);

            // Write non-empty columns only
            writer.WritePropertyName(Names.Columns);
            writer.WriteStartObject();

            foreach (var pair in Columns)
            {
                if (pair.Value.Count > 0)
                {
                    writer.Write(pair.Key, pair.Value);
                }
            }

            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
