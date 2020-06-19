// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Suppression'
    /// </summary>
    internal partial class SuppressionTable : Table<Suppression>
    {
        internal SarifLogDatabase Database;

        internal IColumn<string> Guid;
        internal IColumn<int> Kind;
        internal IColumn<int> Status;
        internal IColumn<string> Justification;
        internal RefColumn Location;
        internal IColumn<IDictionary<string, SerializedPropertyInfo>> Properties;

        internal SuppressionTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Guid = AddColumn(nameof(Guid), ColumnFactory.Build<string>(default));
            Kind = AddColumn(nameof(Kind), ColumnFactory.Build<int>((int)default(SuppressionKind)));
            Status = AddColumn(nameof(Status), ColumnFactory.Build<int>((int)default(SuppressionStatus)));
            Justification = AddColumn(nameof(Justification), ColumnFactory.Build<string>(default));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Properties = AddColumn(nameof(Properties), new DictionaryColumn<string, SerializedPropertyInfo>(new DistinctColumn<string>(new StringColumn()), new SerializedPropertyInfoColumn()));
        }

        public override Suppression Get(int index)
        {
            return (index == -1 ? null : new Suppression(this, index));
        }
    }
}
