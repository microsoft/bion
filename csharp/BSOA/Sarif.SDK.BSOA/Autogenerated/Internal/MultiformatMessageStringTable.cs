// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'MultiformatMessageString'
    /// </summary>
    internal partial class MultiformatMessageStringTable : Table<MultiformatMessageString>
    {
        internal SarifLogDatabase Database;

        internal IColumn<String> Text;
        internal IColumn<String> Markdown;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal MultiformatMessageStringTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), database.BuildColumn<String>(nameof(MultiformatMessageString), nameof(Text), default));
            Markdown = AddColumn(nameof(Markdown), database.BuildColumn<String>(nameof(MultiformatMessageString), nameof(Markdown), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(MultiformatMessageString), nameof(Properties), default));
        }

        public override MultiformatMessageString Get(int index)
        {
            return (index == -1 ? null : new MultiformatMessageString(this, index));
        }
    }
}
