// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ThreadFlowLocation'
    /// </summary>
    internal partial class ThreadFlowLocationTable : Table<ThreadFlowLocation>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> Index;
        internal RefColumn Location;
        internal RefColumn Stack;
        internal IColumn<IList<string>> Kinds;
        internal RefListColumn Taxa;
        internal IColumn<string> Module;
        internal IColumn<IDictionary<string, MultiformatMessageString>> State;
        internal IColumn<int> NestingLevel;
        internal IColumn<int> ExecutionOrder;
        internal IColumn<DateTime> ExecutionTimeUtc;
        internal IColumn<int> Importance;
        internal RefColumn WebRequest;
        internal RefColumn WebResponse;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal ThreadFlowLocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Stack = AddColumn(nameof(Stack), new RefColumn(nameof(SarifLogDatabase.Stack)));
            Kinds = AddColumn(nameof(Kinds), ColumnFactory.Build<IList<string>>(default));
            Taxa = AddColumn(nameof(Taxa), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            Module = AddColumn(nameof(Module), ColumnFactory.Build<string>(default));
            State = AddColumn(nameof(State), new DictionaryColumn<string, MultiformatMessageString>(new StringColumn(), new MultiformatMessageStringColumn(this.Database)));
            NestingLevel = AddColumn(nameof(NestingLevel), ColumnFactory.Build<int>(default));
            ExecutionOrder = AddColumn(nameof(ExecutionOrder), ColumnFactory.Build<int>(-1));
            ExecutionTimeUtc = AddColumn(nameof(ExecutionTimeUtc), ColumnFactory.Build<DateTime>(default));
            Importance = AddColumn(nameof(Importance), ColumnFactory.Build<int>((int)ThreadFlowLocationImportance.Important));
            WebRequest = AddColumn(nameof(WebRequest), new RefColumn(nameof(SarifLogDatabase.WebRequest)));
            WebResponse = AddColumn(nameof(WebResponse), new RefColumn(nameof(SarifLogDatabase.WebResponse)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override ThreadFlowLocation Get(int index)
        {
            return (index == -1 ? null : new ThreadFlowLocation(this, index));
        }
    }
}
