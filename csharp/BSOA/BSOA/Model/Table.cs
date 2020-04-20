using BSOA.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace BSOA.Model
{
    public abstract class Table<T> : ITable<T>
    {
        protected Dictionary<string, IColumn> Columns { get; private set; }

        public Table()
        {
            Columns = new Dictionary<string, IColumn>();
        }

        public int Count { get; protected set; }

        public abstract T this[int index] { get; }
        public abstract T Add();

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
            [nameof(Columns)] = ReadColumns
        };

        public void Read(ITreeReader reader)
        {
            // Clear previous data
            Count = 0;
            foreach (IColumn column in Columns.Values)
            {
                column.Clear();
            }

            reader.ReadObject(this, setters);
        }

        private static void ReadColumns(ITreeReader reader, Table<T> me)
        {
            reader.Expect(TreeToken.StartObject);
            reader.Read();

            while (reader.TokenType == TreeToken.PropertyName)
            {
                string tableName = reader.ReadAsString();
                reader.Read();

                me.Columns[tableName].Read(reader);
                reader.Read();
            }

            reader.Expect(TreeToken.EndObject);
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
