using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'ReportingConfiguration'
    /// </summary>
    internal partial class ReportingConfigurationTable : Table<ReportingConfiguration>
    {
        internal SarifLogDatabase Database;

        internal IColumn<bool> Enabled;
        internal IColumn<int> Level;
        internal IColumn<double> Rank;
        internal RefColumn Parameters;
        internal IColumn<IDictionary<string, string>> Properties;

        internal ReportingConfigurationTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Enabled = AddColumn(nameof(Enabled), ColumnFactory.Build<bool>(true));
            Level = AddColumn(nameof(Level), ColumnFactory.Build<int>((int)FailureLevel.Warning));
            Rank = AddColumn(nameof(Rank), ColumnFactory.Build<double>(-1));
            Parameters = AddColumn(nameof(Parameters), new RefColumn(nameof(SarifLogDatabase.PropertyBag)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override ReportingConfiguration Get(int index)
        {
            return (index == -1 ? null : new ReportingConfiguration(this, index));
        }
    }
}
