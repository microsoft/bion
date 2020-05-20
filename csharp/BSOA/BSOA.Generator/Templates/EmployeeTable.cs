using BSOA.Column;
using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Table for 'Employee' entity.
    /// </summary>
    public partial class EmployeeTable : Table<Employee>
    {
        internal CompanyDatabase Database;
        internal IColumn<string> Name;

        public EmployeeTable(CompanyDatabase database) : base()
        {
            Database = database;

            Name = AddColumn(nameof(Name), new StringColumn());
        }

        public override Employee Get(int index)
        {
            return (index == -1 ? null : new Employee(this, index));
        }
    }
}
