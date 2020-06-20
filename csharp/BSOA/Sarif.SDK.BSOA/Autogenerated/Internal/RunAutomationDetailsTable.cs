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
            Id = AddColumn(nameof(Id), database.BuildColumn<String>(nameof(RunAutomationDetails), nameof(Id), default));
            Guid = AddColumn(nameof(Guid), database.BuildColumn<String>(nameof(RunAutomationDetails), nameof(Guid), default));
            CorrelationGuid = AddColumn(nameof(CorrelationGuid), database.BuildColumn<String>(nameof(RunAutomationDetails), nameof(CorrelationGuid), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(RunAutomationDetails), nameof(Properties), default));
        }

        public override RunAutomationDetails Get(int index)
        {
            return (index == -1 ? null : new RunAutomationDetails(this, index));
        }
    }
}
