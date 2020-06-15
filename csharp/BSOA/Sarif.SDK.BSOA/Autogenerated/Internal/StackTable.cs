// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Stack'
    /// </summary>
    internal partial class StackTable : Table<Stack>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Message;
        internal RefListColumn Frames;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal StackTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            Frames = AddColumn(nameof(Frames), new RefListColumn(nameof(SarifLogDatabase.StackFrame)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override Stack Get(int index)
        {
            return (index == -1 ? null : new Stack(this, index));
        }
    }
}
