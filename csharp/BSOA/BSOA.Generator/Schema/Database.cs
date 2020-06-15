// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace BSOA.Generator.Schema
{
    public class Database
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public string RootTableName { get; set; }
        public IList<Table> Tables { get; set; }

        public Database(string name, string ns, string rootTableName)
        {
            Name = name;
            Namespace = ns;
            RootTableName = rootTableName;
            Tables = new List<Table>();
        }
    }
}
