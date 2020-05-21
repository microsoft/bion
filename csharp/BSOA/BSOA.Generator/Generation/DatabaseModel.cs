using BSOA.Generator.Schema;
using System.IO;
using System.Text;

namespace BSOA.Generator.Generation
{
    /// <summary>
    ///  Generator for Database classes.
    /// </summary>
    /// <remarks>
    ///  Built on CompanyDatabase.cs, this code uses the known:
    ///   - Table name
    ///   - DB type name
    ///   - Namespace
    /// </remarks>
    public class DatabaseModel
    {
        public static string Code;
        public static string TableMember;
        public static string TableConstructor;

        static DatabaseModel()
        {
            Code = File.ReadAllText(@"Templates\\CompanyDatabase.cs");

            TableMember = CodeSection.Extract(Code, nameof(TableMember));
            TableConstructor = CodeSection.Extract(Code, nameof(TableConstructor));
        }

        public static string Generate(Database database)
        {
            StringBuilder members = new StringBuilder();
            StringBuilder constructors = new StringBuilder();

            foreach (Schema.Table table in database)
            {
                members.Append(TableMember.Replace("Employee", table.Name));
                constructors.Append(TableConstructor .Replace("Employee", table.Name));
            }

            // Empty line between member properties and constructor
            members.AppendLine();

            string resultCode = Code
                .Replace("CompanyDatabase", database.Name)
                .Replace("BSOA.Generator.Templates", database.Namespace);

            resultCode = CodeSection.Replace(resultCode, "TableMembers", members.ToString());
            resultCode = CodeSection.Replace(resultCode, "TableConstructors", constructors.ToString());

            return resultCode;
        }
    }
}
