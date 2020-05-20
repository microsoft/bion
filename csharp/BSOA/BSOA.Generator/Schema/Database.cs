using System.Collections;
using System.Collections.Generic;

namespace BSOA.Generator.Schema
{
    public class Database : IEnumerable<Table>
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public IList<Table> Tables { get; set; }

        public Database(string name, string ns)
        {
            Name = name;
            Namespace = ns;
            Tables = new List<Table>();
        }

        public void Add(Table table)
        {
            Tables.Add(table);
        }

        public IEnumerator<Table> GetEnumerator()
        {
            return Tables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Tables.GetEnumerator();
        }
    }
}
