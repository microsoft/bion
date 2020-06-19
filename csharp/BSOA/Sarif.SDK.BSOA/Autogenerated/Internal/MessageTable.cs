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

        internal IColumn<string> Text;
        internal IColumn<string> Markdown;
        internal IColumn<string> Id;
        internal IColumn<IList<string>> Arguments;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal MessageTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), ColumnFactory.Build<string>(default));
            Markdown = AddColumn(nameof(Markdown), ColumnFactory.Build<string>(default));
            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>(default));
            Arguments = AddColumn(nameof(Arguments), ColumnFactory.Build<IList<string>>(default));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Message Get(int index)
        {
            return (index == -1 ? null : new Message(this, index));
        }
    }
}
