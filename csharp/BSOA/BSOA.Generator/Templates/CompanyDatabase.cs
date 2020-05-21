using BSOA.Model;

namespace BSOA.Generator.Templates
{
    /// <summary>
    ///  GENERATED: BSOA Database
    /// </summary>
    public partial class CompanyDatabase : Database
    {
        internal static CompanyDatabase Current { get; private set; }

        // <TableMembers>
        //   <TableMember>
        internal EmployeeTable Employee { get; }
        //   </TableMember>
        internal TeamTable Team { get; }
        // </TableMembers>

        // <Properties>
        public ILimitedList<Team> Teams => Team;
        // </Properties>

        public CompanyDatabase()
        {
            Current = this;

            // <TableConstructors>
            //   <TableConstructor>
            Employee = AddTable(nameof(Employee), new EmployeeTable(this));
            //   </TableConstructor>
            Team = AddTable(nameof(Team), new TeamTable(this));
            // </TableConstructors>
        }
    }
}
