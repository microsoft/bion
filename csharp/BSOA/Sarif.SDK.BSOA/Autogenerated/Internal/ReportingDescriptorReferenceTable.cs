// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ReportingDescriptorReference'
    /// </summary>
    internal partial class ReportingDescriptorReferenceTable : Table<ReportingDescriptorReference>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Id;
        internal IColumn<int> Index;
        internal IColumn<string> Guid;
        internal RefColumn ToolComponent;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ReportingDescriptorReferenceTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>(default));
            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>(default));
            ToolComponent = AddColumn(nameof(ToolComponent), new RefColumn(nameof(SarifLogDatabase.ToolComponentReference)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override ReportingDescriptorReference Get(int index)
        {
            return (index == -1 ? null : new ReportingDescriptorReference(this, index));
        }
    }
}
