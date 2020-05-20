using System.Collections;
using System.Collections.Generic;

namespace BSOA.Generator.Schema
{
    public class Table : IEnumerable<Column>
    {
        public string Name { get; set; }
        public IList<Column> Columns { get; set; }

        public Table(string name)
        {
            Name = name;
            Columns = new List<Column>();
        }

        public void Add(Column column)
        {
            Columns.Add(column);
        }

        public IEnumerator<Column> GetEnumerator()
        {
            return Columns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Columns.GetEnumerator();
        }
    }
}
