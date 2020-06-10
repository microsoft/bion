using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'SpecialLocations'
    /// </summary>
    internal partial class SpecialLocationsTable : Table<SpecialLocations>
    {
        internal SarifLogDatabase Database;

        internal RefColumn DisplayBase;
        internal IColumn<IDictionary<string, string>> Properties;

        internal SpecialLocationsTable(SarifLogDatabase database) : base()
        {
            Database = database;

            DisplayBase = AddColumn(nameof(DisplayBase), new RefColumn(nameof(SarifLogDatabase.ArtifactLocation)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override SpecialLocations Get(int index)
        {
            return (index == -1 ? null : new SpecialLocations(this, index));
        }
    }
}
