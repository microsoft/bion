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

            Text = AddColumn(nameof(Text), database.BuildColumn<String>(nameof(Message), nameof(Text), default));
            Markdown = AddColumn(nameof(Markdown), database.BuildColumn<String>(nameof(Message), nameof(Markdown), default));
            Id = AddColumn(nameof(Id), database.BuildColumn<String>(nameof(Message), nameof(Id), default));
            Arguments = AddColumn(nameof(Arguments), database.BuildColumn<IList<String>>(nameof(Message), nameof(Arguments), default));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Message), nameof(Properties), default));
        }

        public override Message Get(int index)
        {
            return (index == -1 ? null : new Message(this, index));
        }
    }
}
