// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ThreadFlow'
    /// </summary>
    internal partial class ThreadFlowTable : Table<ThreadFlow>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Id;
        internal RefColumn Message;
        internal IColumn<IDictionary<string, MultiformatMessageString>> InitialState;
        internal IColumn<IDictionary<string, MultiformatMessageString>> ImmutableState;
        internal RefListColumn Locations;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ThreadFlowTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>(default(string)));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            InitialState = AddColumn(nameof(InitialState), ColumnFactory.Build<IDictionary<string, MultiformatMessageString>>(default(IDictionary<string, MultiformatMessageString>)));
            ImmutableState = AddColumn(nameof(ImmutableState), ColumnFactory.Build<IDictionary<string, MultiformatMessageString>>(default(IDictionary<string, MultiformatMessageString>)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.ThreadFlowLocation)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, SerializedPropertyInfo>>(default(IDictionary<string, SerializedPropertyInfo>)));
        }

        public override ThreadFlow Get(int index)
        {
            return (index == -1 ? null : new ThreadFlow(this, index));
        }
    }
}
