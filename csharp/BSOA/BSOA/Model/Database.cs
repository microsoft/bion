// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.GC;
using BSOA.IO;

namespace BSOA.Model
{
    /// <summary>
    ///  BSOA Database is the container class for an overall set of tables.
    ///  Projects using BSOA will have the 'root' type inherit from Database.
    /// </summary>
    public abstract class Database : IDatabase
    {
        public string RootTableName { get; }
        public Dictionary<string, ITable> Tables { get; }

        public Database(string rootTableName)
        {
            RootTableName = rootTableName;
            Tables = new Dictionary<string, ITable>();
        }

        public abstract void GetOrBuildTables();

        protected U GetOrBuild<U>(string name, Func<U> builder) where U : ITable
        {
            if (Tables.TryGetValue(name, out ITable table))
            {
                return (U)table;
            }
            else
            {
                U newTable = builder();
                Tables[name] = newTable;
                return newTable;
            }
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

        /// <summary>
        ///  Garbage Collect the tables in this database, removing any
        ///  unreachable rows.
        /// </summary>
        public bool Collect()
        {
            DatabaseCollector collector = new DatabaseCollector(this);
            return collector.Collect();
        }

        public void Read(ITreeReader reader)
        {
            Clear();

            // Read Tables, skipping unknown tables if Settings.Strict == false
            reader.ReadDictionaryItems(Tables, throwOnUnknown: reader.Settings.Strict);
        }

        public void Write(ITreeWriter writer)
        {
            // Garbage Collect before writing
            Collect();

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
