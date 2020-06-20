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

            Id = AddColumn(nameof(Id), ColumnFactory.Build<String>(default));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            InitialState = AddColumn(nameof(InitialState), new DictionaryColumn<String, MultiformatMessageString>(new DistinctColumn<string>(new StringColumn()), new MultiformatMessageStringColumn(this.Database)));
            ImmutableState = AddColumn(nameof(ImmutableState), new DictionaryColumn<String, MultiformatMessageString>(new DistinctColumn<string>(new StringColumn()), new MultiformatMessageStringColumn(this.Database)));
            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.ThreadFlowLocation)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override ThreadFlow Get(int index)
        {
            return (index == -1 ? null : new ThreadFlow(this, index));
        }
    }
}
