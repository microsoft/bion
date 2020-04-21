using BSOA.IO;
using System.Collections;
using System.Collections.Generic;

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
    public abstract class Table<T> : ITable<T>
    {
        /// <summary>
        ///  Columns by Name in Table.
        /// </summary>
        protected Dictionary<string, IColumn> Columns { get; private set; }

        public Table()
        {
            Columns = new Dictionary<string, IColumn>();
        }

        /// <summary>
        ///  Count of items in this Table.
        ///  Column counts may differ, because they are only increased when each property is set to a non-default value.
        /// </summary>
        public int Count { get; protected set; }

        /// <summary>
        ///  Get the item at a given index in this table.
        /// </summary>
        /// <param name="index">0-based index of item to return</param>
        /// <returns>SoA item instance at index</returns>
        public abstract T this[int index] { get; }

        /// <summary>
        ///  Add a new item to the end of this table.
        /// </summary>
        /// <returns>New SoA item instance</returns>
        public T Add()
        {
            Count++;
            return this[Count - 1];
        }

        /// <summary>
        ///  Remove all items from table.
        /// </summary>
        public void Clear()
        {
            Count = 0;

            foreach (IColumn column in Columns.Values)
            {
                column.Clear();
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

        public IEnumerator<T> GetEnumerator()
        {
            return new TableEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TableEnumerator<T>(this);
        }

        private static Dictionary<string, Setter<Table<T>>> setters = new Dictionary<string, Setter<Table<T>>>()
        {
            [nameof(Count)] = (r, me) => me.Count = r.ReadAsInt32(),
            [nameof(Columns)] = (r, me) => r.ReadDictionaryItems(me.Columns, throwOnUnknown: r.Settings.Strict),
        };

        public void Read(ITreeReader reader)
        {
            Clear();

            // Read Columns, skipping unknown columns if Settings.Strict == false
            reader.ReadObject(this, setters);
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();

            writer.Write(nameof(Count), Count);

            writer.WritePropertyName(nameof(Columns));
            writer.WriteDictionary(Columns);

            writer.WriteEndObject();
        }
    }
}
