using BSOA.Column;
using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'CompanyDatabase' entity.
    /// </summary>
    internal partial class RootTable : Table<Root>
    {
        internal CompanyDatabase Database;

        internal IColumn<long> EmployeeId;
        internal RefListColumn Teams;
        internal RefListColumn Employees;

        public RootTable(CompanyDatabase database) : base()
        {
            Database = database;

            EmployeeId = AddColumn(nameof(EmployeeId), ColumnFactory.Build<long>());
            Teams = AddColumn(nameof(Teams), new RefListColumn(nameof(CompanyDatabase.Team)));
            Employees = AddColumn(nameof(Employees), new RefListColumn(nameof(CompanyDatabase.Employee)));
        }

        public override Root Get(int index)
        {
            return (index == -1 ? null : new Root(this, index));
        }
    }
}
