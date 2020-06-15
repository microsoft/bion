// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'CodeFlow'
    /// </summary>
    internal partial class CodeFlowTable : Table<CodeFlow>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Message;
        internal RefListColumn ThreadFlows;
        internal IColumn<IDictionary<string, string>> Properties;

        internal CodeFlowTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            ThreadFlows = AddColumn(nameof(ThreadFlows), new RefListColumn(nameof(SarifLogDatabase.ThreadFlow)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override CodeFlow Get(int index)
        {
            return (index == -1 ? null : new CodeFlow(this, index));
        }
    }
}
