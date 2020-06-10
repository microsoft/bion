using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'LocationRelationship'
    /// </summary>
    internal partial class LocationRelationshipTable : Table<LocationRelationship>
    {
        internal SarifLogDatabase Database;

        internal IColumn<int> Target;
        internal IColumn<IList<string>> Kinds;
        internal RefColumn Description;
        internal IColumn<IDictionary<string, string>> Properties;

        internal LocationRelationshipTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Target = AddColumn(nameof(Target), ColumnFactory.Build<int>());
            Kinds = AddColumn(nameof(Kinds), ColumnFactory.Build<IList<string>>());
            Description = AddColumn(nameof(Description), new RefColumn(nameof(SarifLogDatabase.Message)));
            Properties = AddColumn(nameof(Properties), ColumnFactory.Build<IDictionary<string, string>>());
        }

        public override LocationRelationship Get(int index)
        {
            return (index == -1 ? null : new LocationRelationship(this, index));
        }
    }
}
