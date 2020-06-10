using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

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
        internal IColumn<IDictionary<string, string>> Properties;

        internal AttachmentTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            ArtifactLocation = AddColumn(nameof(ArtifactLocation), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Regions = AddColumn(nameof(Regions), new RefListColumn(nameof(SarifLogDatabase.Region)));
            Rectangles = AddColumn(nameof(Rectangles), new RefListColumn(nameof(SarifLogDatabase.Rectangle)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override Attachment Get(int index)
        {
            return (index == -1 ? null : new Attachment(this, index));
        }
    }
}
