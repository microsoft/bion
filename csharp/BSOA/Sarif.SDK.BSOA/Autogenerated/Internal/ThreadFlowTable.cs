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

        internal IColumn<String> Id;
        internal RefColumn Message;
        internal IColumn<IDictionary<String, MultiformatMessageString>> InitialState;
        internal IColumn<IDictionary<String, MultiformatMessageString>> ImmutableState;
        internal RefListColumn Locations;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ThreadFlowTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Id = AddColumn(nameof(Id), database.BuildColumn<String>(nameof(ThreadFlow), nameof(Id), default));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            InitialState = AddColumn(nameof(InitialState), database.BuildColumn<IDictionary<String, MultiformatMessageString>>(nameof(ThreadFlow), nameof(InitialState), default));
            ImmutableState = AddColumn(nameof(ImmutableState), database.BuildColumn<IDictionary<String, MultiformatMessageString>>(nameof(ThreadFlow), nameof(ImmutableState), default));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.ThreadFlowLocation)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ThreadFlow), nameof(Properties), default));
        }

        public override ThreadFlow Get(int index)
        {
            return (index == -1 ? null : new ThreadFlow(this, index));
        }
    }
}
