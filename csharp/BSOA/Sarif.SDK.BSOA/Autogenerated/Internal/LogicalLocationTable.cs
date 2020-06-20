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

            Name = AddColumn(nameof(Name), database.BuildColumn<String>(nameof(LogicalLocation), nameof(Name), default));
            Index = AddColumn(nameof(Index), database.BuildColumn<int>(nameof(LogicalLocation), nameof(Index), -1));
            FullyQualifiedName = AddColumn(nameof(FullyQualifiedName), database.BuildColumn<String>(nameof(LogicalLocation), nameof(FullyQualifiedName), default));
            DecoratedName = AddColumn(nameof(DecoratedName), database.BuildColumn<String>(nameof(LogicalLocation), nameof(DecoratedName), default));
            ParentIndex = AddColumn(nameof(ParentIndex), database.BuildColumn<int>(nameof(LogicalLocation), nameof(ParentIndex), -1));
            Kind = AddColumn(nameof(Kind), database.BuildColumn<String>(nameof(LogicalLocation), nameof(Kind), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(LogicalLocation), nameof(Properties), default));
        }

        public override LogicalLocation Get(int index)
        {
            return (index == -1 ? null : new LogicalLocation(this, index));
        }
    }
}
