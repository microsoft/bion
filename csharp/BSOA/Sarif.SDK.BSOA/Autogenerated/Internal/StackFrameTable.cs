// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'StackFrame'
    /// </summary>
    internal partial class StackFrameTable : Table<StackFrame>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Location;
        internal IColumn<String> Module;
        internal IColumn<int> ThreadId;
        internal IColumn<IList<String>> Parameters;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal StackFrameTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Module = AddColumn(nameof(Module), database.BuildColumn<String>(nameof(StackFrame), nameof(Module), default));
            ThreadId = AddColumn(nameof(ThreadId), database.BuildColumn<int>(nameof(StackFrame), nameof(ThreadId), default));
            Parameters = AddColumn(nameof(Parameters), database.BuildColumn<IList<String>>(nameof(StackFrame), nameof(Parameters), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(StackFrame), nameof(Properties), default));
        }

        public override StackFrame Get(int index)
        {
            return (index == -1 ? null : new StackFrame(this, index));
        }
    }
}
