// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Message'
    /// </summary>
    internal partial class MessageTable : Table<Message>
    {
        internal TinyDatabase Database;

        internal IColumn<string> Text;
        internal IColumn<string> Markdown;
        internal IColumn<string> Id;

        internal MessageTable(TinyDatabase database) : base()
        {
            Database = database;

            Text = AddColumn(nameof(Text), ColumnFactory.Build<string>(null));
            Markdown = AddColumn(nameof(Markdown), ColumnFactory.Build<string>(null));
            Id = AddColumn(nameof(Id), ColumnFactory.Build<string>(null));
        }

        public override Message Get(int index)
        {
            return (index == -1 ? null : new Message(this, index));
        }
    }
}
