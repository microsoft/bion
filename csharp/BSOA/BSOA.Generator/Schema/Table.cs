// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace BSOA.Generator.Schema
{
    public class Table
    {
        public string Name { get; set; }
        public IList<Column> Columns { get; set; }

        public Table(string name)
        {
            Name = name;
            Columns = new List<Column>();
        }
    }
}
