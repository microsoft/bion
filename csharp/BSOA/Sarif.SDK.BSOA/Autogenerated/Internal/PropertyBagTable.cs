using BSOA.Column;
using BSOA.Model;

using System;
using System.Collections.Generic;

namespace Microsoft.CodeAnalysis.Sarif
{
    /// <summary>
    ///  BSOA GENERATED Table for 'PropertyBag'
    /// </summary>
    internal partial class PropertyBagTable : Table<PropertyBag>
    {
        internal SarifLogDatabase Database;

        internal IColumn<IList<string>> Tags;

        internal PropertyBagTable(SarifLogDatabase database) : base()
        {
            Database = database;

            Tags = AddColumn(nameof(Tags), ColumnFactory.Build<IList<string>>());
        }

        public override PropertyBag Get(int index)
        {
            return (index == -1 ? null : new PropertyBag(this, index));
        }
    }
}
