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
        internal IColumn<string> Id;
        internal IColumn<string> Guid;
        internal IColumn<string> CorrelationGuid;
        internal IColumn<IDictionary<string, string>> Properties;

        internal RunAutomationDetailsTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>());
            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>());
            CorrelationGuid = AddColumn(nameof(CorrelationGuid), ColumnFactory.Build<string>());
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override RunAutomationDetails Get(int index)
        {
            return (index == -1 ? null : new RunAutomationDetails(this, index));
        }
    }
}
