using System;
using System.Collections.Generic;

using BSOA.Column;
using BSOA.Model;

namespace BSOA.Demo.Model
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'LogicalLocation' entity.
    /// </summary>
    public partial class LogicalLocationTable : Table<LogicalLocation>
    {
        internal SarifLogBsoa Database;

        internal IColumn<string> Name;
        internal IColumn<int> Index;
        internal IColumn<string> FullyQualifiedName;
        internal IColumn<string> DecoratedName;
        internal IColumn<int> ParentIndex;
        internal IColumn<string> Kind;

        public LogicalLocationTable(SarifLogBsoa database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), ColumnFactory.Build<string>(null));
            Index = AddColumn(nameof(Index), ColumnFactory.Build<int>(-1));
            FullyQualifiedName = AddColumn(nameof(FullyQualifiedName), ColumnFactory.Build<string>(null));
            DecoratedName = AddColumn(nameof(DecoratedName), ColumnFactory.Build<string>(null));
            ParentIndex = AddColumn(nameof(ParentIndex), ColumnFactory.Build<int>(-1));
            Kind = AddColumn(nameof(Kind), ColumnFactory.Build<string>(null));
        }

        public override LogicalLocation Get(int index)
        {
            return (index == -1 ? null : new LogicalLocation(this, index));
        }
    }
}
