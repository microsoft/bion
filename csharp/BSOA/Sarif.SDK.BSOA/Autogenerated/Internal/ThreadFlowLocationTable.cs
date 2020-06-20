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
        internal IColumn<IList<String>> Kinds;
        internal RefListColumn Taxa;
        internal IColumn<String> Module;
        internal IColumn<IDictionary<String, MultiformatMessageString>> State;
        internal IColumn<int> NestingLevel;
        internal IColumn<int> ExecutionOrder;
        internal IColumn<DateTime> ExecutionTimeUtc;
        internal IColumn<int> Importance;
        internal RefColumn WebRequest;
        internal RefColumn WebResponse;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ThreadFlowLocationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Index = AddColumn(nameof(Index), database.BuildColumn<int>(nameof(ThreadFlowLocation), nameof(Index), -1));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Stack = AddColumn(nameof(Stack), new RefColumn(nameof(SarifLogDatabase.Stack)));
            Kinds = AddColumn(nameof(Kinds), database.BuildColumn<IList<String>>(nameof(ThreadFlowLocation), nameof(Kinds), default));
            Taxa = AddColumn(nameof(Taxa), new RefListColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            Module = AddColumn(nameof(Module), database.BuildColumn<String>(nameof(ThreadFlowLocation), nameof(Module), default));
            State = AddColumn(nameof(State), database.BuildColumn<IDictionary<String, MultiformatMessageString>>(nameof(ThreadFlowLocation), nameof(State), default));
            NestingLevel = AddColumn(nameof(NestingLevel), database.BuildColumn<int>(nameof(ThreadFlowLocation), nameof(NestingLevel), default));
            ExecutionOrder = AddColumn(nameof(ExecutionOrder), database.BuildColumn<int>(nameof(ThreadFlowLocation), nameof(ExecutionOrder), -1));
            ExecutionTimeUtc = AddColumn(nameof(ExecutionTimeUtc), database.BuildColumn<DateTime>(nameof(ThreadFlowLocation), nameof(ExecutionTimeUtc), default));
            Importance = AddColumn(nameof(Importance), database.BuildColumn<int>(nameof(ThreadFlowLocation), nameof(Importance), (int)ThreadFlowLocationImportance.Important));
            WebRequest = AddColumn(nameof(WebRequest), new RefColumn(nameof(SarifLogDatabase.WebRequest)));
            WebResponse = AddColumn(nameof(WebResponse), new RefColumn(nameof(SarifLogDatabase.WebResponse)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ThreadFlowLocation), nameof(Properties), default));
        }

        public override ThreadFlowLocation Get(int index)
        {
            return (index == -1 ? null : new ThreadFlowLocation(this, index));
        }
    }
}
