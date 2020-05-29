using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Database
    /// </summary>
    public partial class CompanyDatabase : Database
    {
        internal static CompanyDatabase Current { get; private set; }

        // <TableMemberList>
        //   <TableMember>
        internal EmployeeTable Employee { get; }
        //   </TableMember>
        internal TeamTable Team { get; }
        // </TableMemberList>

        public CompanyDatabase()
        {
            Current = this;

            // <TableConstructorList>
            //   <TableConstructor>
            Employee = AddTable(nameof(Employee), new EmployeeTable(this));
            //   </TableConstructor>
            Team = AddTable(nameof(Team), new TeamTable(this));
            // </TableConstructorList>
        }
    }
}
