// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Tool'
    /// </summary>
    internal partial class ToolTable : Table<Tool>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Driver;
        internal RefListColumn Extensions;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ToolTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Driver = AddColumn(nameof(Driver), new RefColumn(nameof(SarifLogDatabase.ToolComponent)));
            Extensions = AddColumn(nameof(Extensions), new RefListColumn(nameof(SarifLogDatabase.ToolComponent)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Tool Get(int index)
        {
            return (index == -1 ? null : new Tool(this, index));
        }
    }
}
