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

        internal IColumn<string> Name;
        internal IColumn<int> Index;
        internal IColumn<string> FullyQualifiedName;
        internal IColumn<string> DecoratedName;
        internal IColumn<int> ParentIndex;
        internal IColumn<string> Kind;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal LogicalLocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>());
            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            FullyQualifiedName = AddColumn(nameof(FullyQualifiedName), ColumnFactory.Build<string>());
            DecoratedName = AddColumn(nameof(DecoratedName), ColumnFactory.Build<string>());
            ParentIndex = AddColumn(nameof(ParentIndex), ColumnFactory.Build<int>(-1));
            Kind = AddColumn(nameof(Kind), ColumnFactory.Build<string>());
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override LogicalLocation Get(int index)
        {
            return (index == -1 ? null : new LogicalLocation(this, index));
        }
    }
}
