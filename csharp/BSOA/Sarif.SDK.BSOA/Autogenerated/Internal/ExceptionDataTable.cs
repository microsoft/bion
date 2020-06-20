// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ExceptionData'
    /// </summary>
    internal partial class ExceptionDataTable : Table<ExceptionData>
    {
        internal SarifLogDatabase Database;

        internal IColumn<String> Kind;
        internal IColumn<String> Message;
        internal RefColumn Stack;
        internal RefListColumn InnerExceptions;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal ExceptionDataTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Kind = AddColumn(nameof(Kind), ColumnFactory.Build<String>(default));
            Message = AddColumn(nameof(Message), ColumnFactory.Build<String>(default));
            Stack = AddColumn(nameof(Stack), new RefColumn(nameof(SarifLogDatabase.Stack)));
            InnerExceptions = AddColumn(nameof(InnerExceptions), new RefListColumn(nameof(SarifLogDatabase.ExceptionData)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<String, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override ExceptionData Get(int index)
        {
            return (index == -1 ? null : new ExceptionData(this, index));
        }
    }
}
