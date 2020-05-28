using System.Collections;
using System.Collections.Generic;

namespace BSOA.Generator.Schema
{
    public class Database
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
    }
}
