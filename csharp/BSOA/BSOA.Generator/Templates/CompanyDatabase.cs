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
        internal EmployeeTable Employee { get; }
        //   <TableMember>
        internal TeamTable Team { get; }
        //   </TableMember>
        // </TableMemberList>

        public CompanyDatabase()
        {
            Current = this;

            // <TableConstructorList>
            Employee = AddTable(nameof(Employee), new EmployeeTable(this));
            //   <TableConstructor>
            Team = AddTable(nameof(Team), new TeamTable(this));
            //   </TableConstructor>
            // </TableConstructorList>
        }
    }
}
