using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ExceptionData'
    /// </summary>
    internal partial class ExceptionDataTable : Table<ExceptionData>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Kind;
        internal IColumn<string> Message;
        internal RefColumn Stack;
        internal RefListColumn InnerExceptions;
        internal IColumn<IDictionary<string, string>> Properties;

        internal ExceptionDataTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Kind = AddColumn(nameof(Kind), ColumnFactory.Build<string>());
            Message = AddColumn(nameof(Message), ColumnFactory.Build<string>());
            Stack = AddColumn(nameof(Stack), new RefColumn(nameof(SarifLogDatabase.Stack)));
            InnerExceptions = AddColumn(nameof(InnerExceptions), new RefListColumn(nameof(SarifLogDatabase.ExceptionData)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override ExceptionData Get(int index)
        {
            return (index == -1 ? null : new ExceptionData(this, index));
        }
    }
}
