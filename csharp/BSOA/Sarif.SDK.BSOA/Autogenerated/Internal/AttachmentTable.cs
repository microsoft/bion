// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'Attachment'
    /// </summary>
    internal partial class AttachmentTable : Table<Attachment>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Description;
        internal RefColumn ArtifactLocation;
        internal RefListColumn Regions;
        internal RefListColumn Rectangles;
        internal IColumn<IDictionary<String, SerializedPropertyInfo>> Properties;

        internal AttachmentTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            ArtifactLocation = AddColumn(nameof(ArtifactLocation), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Regions = AddColumn(nameof(Regions), new RefListColumn(nameof(SarifLogDatabase.Region)));
            Rectangles = AddColumn(nameof(Rectangles), new RefListColumn(nameof(SarifLogDatabase.Rectangle)));
            Properties = AddColumn(nameof(Properties), database.BuildColumn<IDictionary<String, SerializedPropertyInfo>>(nameof(Attachment), nameof(Properties), default));
        }

        public override Attachment Get(int index)
        {
            return (index == -1 ? null : new Attachment(this, index));
        }
    }
}
