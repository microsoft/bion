// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Rectangle'
    /// </summary>
    internal partial class RectangleTable : Table<Rectangle>
    {
        internal SarifLogDatabase Database;

        internal IColumn<double> Top;
        internal IColumn<double> Left;
        internal IColumn<double> Bottom;
        internal IColumn<double> Right;
        internal RefColumn Message;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal RectangleTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Top = AddColumn(nameof(Top), ColumnFactory.Build<double>(default));
            Left = AddColumn(nameof(Left), ColumnFactory.Build<double>(default));
            Bottom = AddColumn(nameof(Bottom), ColumnFactory.Build<double>(default));
            Right = AddColumn(nameof(Right), ColumnFactory.Build<double>(default));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Rectangle Get(int index)
        {
            return (index == -1 ? null : new Rectangle(this, index));
        }
    }
}
