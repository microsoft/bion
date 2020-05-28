using BSOA.Generator.Schema;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace BSOA.Generator.Generation
{
    /// <summary>
    ///  Generator for Table classes.
    /// </summary>
    /// <remarks>
    ///  Built on TeamTable.cs, this code uses the known:
    ///   - Type name
    ///   - DB type name
    ///   - Column names
    ///   - Column types
    ///   - Namespace
    /// </remarks>
    public class TableModel : ICodeGenerator
    {
        public string Code;
        public Dictionary<string, string> Templates;

        public TableModel() : this(@"Templates\\TeamTable.cs")
        { }

        public TableModel(string templateFilePath)
        {
            Code = File.ReadAllText(templateFilePath);
            Templates = CodeSection.AllTemplates(Code);
        }

        public virtual void Generate(Database database, string outputPath)
        {
            foreach (Table table in database.Tables)
            {
                File.WriteAllText(Path.Combine(outputPath, $"{table.Name}Table.cs"), Generate(table, database));
            }
        }

        public virtual string Generate(Table table, Database database)
        {
            StringBuilder members = new StringBuilder();
            StringBuilder constructors = new StringBuilder();

            foreach (Schema.Column column in table.Columns)
            {
                members.Append(TemplateDefaults.Populate(Templates, "ColumnMember", column, table, database));
                constructors.Append(TemplateDefaults.Populate(Templates, "ColumnConstructor", column, table, database));
            }

            // Empty line between column properties and constructor
            members.AppendLine();

            string resultCode = Code
                .Replace("Team", table.Name)
                .Replace("CompanyDatabase", database.Name)
                .Replace("BSOA.Generator.Templates", database.Namespace);

            resultCode = CodeSection.Replace(resultCode, "ColumnMembers", members.ToString());
            resultCode = CodeSection.Replace(resultCode, "ColumnConstructors", constructors.ToString());

            return resultCode;
        }
    }
}
