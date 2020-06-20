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

        internal IColumn<String> Guid;
        internal IColumn<int> Kind;
        internal IColumn<int> Status;
        internal IColumn<String> Justification;
        internal RefColumn Location;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal SuppressionTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Guid = AddColumn(nameof(Guid), database.BuildColumn<String>(nameof(Suppression), nameof(Guid), default));
            Kind = AddColumn(nameof(Kind), database.BuildColumn<int>(nameof(Suppression), nameof(Kind), (int)default(SuppressionKind)));
            Status = AddColumn(nameof(Status), database.BuildColumn<int>(nameof(Suppression), nameof(Status), (int)default(SuppressionStatus)));
            Justification = AddColumn(nameof(Justification), database.BuildColumn<String>(nameof(Suppression), nameof(Justification), default));
            Location = AddColumn(nameof(Location), new RefColumn(nameof(SarifLogDatabase.Location)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Suppression), nameof(Properties), default));
        }

        public override Suppression Get(int index)
        {
            return (index == -1 ? null : new Suppression(this, index));
        }
    }
}
