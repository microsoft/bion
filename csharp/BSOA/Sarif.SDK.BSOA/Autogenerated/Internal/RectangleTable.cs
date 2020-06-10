using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

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
        internal IColumn<IDictionary<string, string>> Properties;

        internal RectangleTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Top = AddColumn(nameof(Top), ColumnFactory.Build<double>());
            Left = AddColumn(nameof(Left), ColumnFactory.Build<double>());
            Bottom = AddColumn(nameof(Bottom), ColumnFactory.Build<double>());
            Right = AddColumn(nameof(Right), ColumnFactory.Build<double>());
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override Rectangle Get(int index)
        {
            return (index == -1 ? null : new Rectangle(this, index));
        }
    }
}
