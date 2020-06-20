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

            Kind = AddColumn(nameof(Kind), database.BuildColumn<String>(nameof(ExceptionData), nameof(Kind), default));
            Message = AddColumn(nameof(Message), database.BuildColumn<String>(nameof(ExceptionData), nameof(Message), default));
            Stack = AddColumn(nameof(Stack), new RefColumn(nameof(SarifLogDatabase.Stack)));
            InnerExceptions = AddColumn(nameof(InnerExceptions), new RefListColumn(nameof(SarifLogDatabase.ExceptionData)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(ExceptionData), nameof(Properties), default));
        }

        public override ExceptionData Get(int index)
        {
            return (index == -1 ? null : new ExceptionData(this, index));
        }
    }
}
