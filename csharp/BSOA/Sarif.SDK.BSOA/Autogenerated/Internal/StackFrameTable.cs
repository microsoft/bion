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
        internal IColumn<string> Module;
        internal IColumn<int> ThreadId;
        internal IColumn<IList<string>> Parameters;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal StackFrameTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Module = AddColumn(nameof(Module), ColumnFactory.Build<string>(default(string)));
            ThreadId = AddColumn(nameof(ThreadId), ColumnFactory.Build<int>(default(int)));
            Parameters = AddColumn(nameof(Parameters), ColumnFactory.Build<IList<string>>(default(IList<string>)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, SerializedPropertyInfo>>(default(IDictionary<string, SerializedPropertyInfo>)));
        }

        public override StackFrame Get(int index)
        {
            return (index == -1 ? null : new StackFrame(this, index));
        }
    }
}
