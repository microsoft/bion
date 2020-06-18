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

        internal IColumn<string> Text;
        internal IColumn<string> Markdown;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal MultiformatMessageStringTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), ColumnFactory.Build<string>(default));
            Markdown = AddColumn(nameof(Markdown), ColumnFactory.Build<string>(default));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new StringColumn(), new SerializedPropertyInfoColumn()));
        }

        public override MultiformatMessageString Get(int index)
        {
            return (index == -1 ? null : new MultiformatMessageString(this, index));
        }
    }
}
