// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'RunAutomationDetails'
    /// </summary>
    internal partial class RunAutomationDetailsTable : Table<RunAutomationDetails>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Description;
        internal IColumn<String> Id;
        internal IColumn<String> Guid;
        internal IColumn<String> CorrelationGuid;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal RunAutomationDetailsTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            Id = AddColumn(nameof(Id), ColumnFactory.Build<String>(default));
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<String>(default));
            CorrelationGuid = AddColumn(nameof(CorrelationGuid), ColumnFactory.Build<String>(default));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override RunAutomationDetails Get(int index)
        {
            return (index == -1 ? null : new RunAutomationDetails(this, index));
        }
    }
}
