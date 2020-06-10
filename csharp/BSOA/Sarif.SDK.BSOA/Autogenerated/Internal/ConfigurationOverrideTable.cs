using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ConfigurationOverride'
    /// </summary>
    internal partial class ConfigurationOverrideTable : Table<ConfigurationOverride>
    {
        internal SarifLogDatabase Database;

        internal RefColumn Configuration;
        internal RefColumn Descriptor;
        internal IColumn<IDictionary<string, string>> Properties;

        internal ConfigurationOverrideTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Configuration = AddColumn(nameof(Configuration), new RefColumn(nameof(SarifLogDatabase.ReportingConfiguration)));
            Descriptor = AddColumn(nameof(Descriptor), new RefColumn(nameof(SarifLogDatabase.ReportingDescriptorReference)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override ConfigurationOverride Get(int index)
        {
            return (index == -1 ? null : new ConfigurationOverride(this, index));
        }
    }
}
