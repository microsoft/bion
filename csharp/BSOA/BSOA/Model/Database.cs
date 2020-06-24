// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.IO;

namespace BSOA.Model
{
    /// <summary>
    ///  BSOA Database is the container class for an overall set of tables.
    ///  Projects using BSOA will have the 'root' type inherit from Database.
    /// </summary>
    public class Database : ITreeSerializable
    {
        protected Dictionary<string, ITable> Tables { get; private set; }

        public Database()
        {
            Tables = new Dictionary<string, ITable>();
        }

        protected U AddTable<U>(string name, U table) where U : ITable
        {
            Tables[name] = table;
            return table;
        }

        public IColumn<T> BuildColumn<T>(string tableName, string columnName, T defaultValue = default)
        {
            return (IColumn<T>)BuildColumn(tableName, columnName, typeof(T), defaultValue);
        }

        public virtual IColumn BuildColumn(string tableName, string columnName, Type type, object defaultValue = null)
        {
            return ColumnFactory.Build(type, defaultValue);
        }

        /// <summary>
        ///  Remove all items from table.
        /// </summary>
        public void Clear()
        {
            foreach (ITable table in Tables.Values)
            {
                table.Clear();
            }
        }

        /// <summary>
        ///  Remove excess capacity and prepare for serialization
        /// </summary>
        public void Trim()
        {
            foreach (ITable table in Tables.Values)
            {
                table.Trim();
            }
        }

        public void Read(ITreeReader reader)
        {
            Clear();

            // Read Tables, skipping unknown tables if Settings.Strict == false
            reader.ReadDictionaryItems(Tables, throwOnUnknown: reader.Settings.Strict);
        }

        public void Write(ITreeWriter writer)
        {
            // Write non-empty tables only
            writer.WriteStartObject();

            foreach (var pair in Tables)
            {
                if (pair.Value.Count > 0)
                {
                    writer.WritePropertyName(pair.Key);
                    pair.Value.Write(writer);
                }
            }

            writer.WriteEndObject();
        }
    }
}
