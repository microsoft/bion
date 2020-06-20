// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Message'
    /// </summary>
    internal partial class MessageTable : Table<Message>
    {
        internal SarifLogDatabase Database;

        internal IColumn<String> Text;
        internal IColumn<String> Markdown;
        internal IColumn<String> Id;
        internal IColumn<IList<String>> Arguments;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal MessageTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), ColumnFactory.Build<String>(default));
            Markdown = AddColumn(nameof(Markdown), ColumnFactory.Build<String>(default));
            Id = AddColumn(nameof(Id), ColumnFactory.Build<String>(default));
            Arguments = AddColumn(nameof(Arguments), ColumnFactory.Build<IList<String>>(default));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Message Get(int index)
        {
            return (index == -1 ? null : new Message(this, index));
        }
    }
}
