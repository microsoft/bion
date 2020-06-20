// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'LogicalLocation'
    /// </summary>
    internal partial class LogicalLocationTable : Table<LogicalLocation>
    {
        internal SarifLogDatabase Database;

        internal IColumn<String> Name;
        internal IColumn<int> Index;
        internal IColumn<String> FullyQualifiedName;
        internal IColumn<String> DecoratedName;
        internal IColumn<int> ParentIndex;
        internal IColumn<String> Kind;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal LogicalLocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), ColumnFactory.Build<String>(default));
            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            FullyQualifiedName = AddColumn(nameof(FullyQualifiedName), ColumnFactory.Build<String>(default));
            DecoratedName = AddColumn(nameof(DecoratedName), ColumnFactory.Build<String>(default));
            ParentIndex = AddColumn(nameof(ParentIndex), ColumnFactory.Build<int>(-1));
            Kind = AddColumn(nameof(Kind), ColumnFactory.Build<String>(default));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override LogicalLocation Get(int index)
        {
            return (index == -1 ? null : new LogicalLocation(this, index));
        }
    }
}
