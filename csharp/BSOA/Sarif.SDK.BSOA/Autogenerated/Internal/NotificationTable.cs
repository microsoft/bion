// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Notification'
    /// </summary>
    internal partial class NotificationTable : Table<Notification>
    {
        internal SarifLogDatabase Database;

        internal RefListColumn Locations;
        internal RefColumn Message;
        internal IColumn<int> Level;
        internal IColumn<int> ThreadId;
        internal IColumn<DateTime> TimeUtc;
        internal RefColumn Exception;
        internal RefColumn Descriptor;
        internal RefColumn AssociatedRule;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal NotificationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Locations = AddColumn(nameof(Locations), new RefListColumn(nameof(SarifLogDatabase.Location)));
            Message = AddColumn(nameof(Message), new RefColumn(nameof(SarifLogDatabase.Message)));
            Level = AddColumn(nameof(Level), ColumnFactory.Build<int>((int)FailureLevel.Warning));
            ThreadId = AddColumn(nameof(ThreadId), ColumnFactory.Build<int>(default));
            TimeUtc = AddColumn(nameof(TimeUtc), ColumnFactory.Build<DateTime>(default));
            Exception = AddColumn(nameof(Exception), new RefColumn(nameof(SarifLogDatabase.ExceptionData)));
            Descriptor = AddColumn(nameof(Descriptor), new RefColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            AssociatedRule = AddColumn(nameof(AssociatedRule), new RefColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Notification Get(int index)
        {
            return (index == -1 ? null : new Notification(this, index));
        }
    }
}
