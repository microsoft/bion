using BSOA.Generator.Schema;

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BSOA.Generator.Generation
{
    /// <summary>
    ///  Generator for Entity classes.
    /// </summary>
    /// <remarks>
    ///  Built on Team.cs, this code uses the known:
    ///   - Type name
    ///   - DB type name
    ///   - Column names
    ///   - Column types
    ///   - Namespace
    /// </remarks>
    public class EntityModel : ICodeGenerator
    {
        public string Code;
        public Dictionary<string, string> Templates;

        public EntityModel() : this(@"Templates\\Team.cs")
        { }

        public EntityModel(string templateFilePath)
        {
            Code = File.ReadAllText(templateFilePath);
            Templates = CodeSection.AllTemplates(Code);
        }

        public virtual void Generate(Database database, string outputFolder)
        {
            foreach (Table table in database.Tables)
            {
                File.WriteAllText(Path.Combine(outputFolder, $"{table.Name}.cs"), Generate(table, database));
            }
        }

        public virtual string Generate(Table table, Database database)
        {
            StringBuilder properties = new StringBuilder();
            StringBuilder parameterList = new StringBuilder();
            StringBuilder assignmentList = new StringBuilder();
            StringBuilder otherAssignmentList = new StringBuilder();

            foreach (Schema.Column column in table)
            {
                properties.AppendLine(TemplateDefaults.Populate(Templates, "Column", column, table, database));

                if (parameterList.Length > 0) { parameterList.Append(",\r\n"); }
                parameterList.Append($"\t\t\t{column.Type} {TemplateDefaults.CamelCase(column.Name)}");
                assignmentList.AppendLine($"\t\t\t{column.Name} = {TemplateDefaults.CamelCase(column.Name)};");
                otherAssignmentList.AppendLine($"\t\t\t{column.Name} = other.{column.Name};");
            }

            string finalCode = Code
                .Replace("Team", table.Name)
                .Replace("CompanyDatabase", database.Name)
                .Replace("BSOA.Generator.Templates", database.Namespace);

            finalCode = CodeSection.Replace(finalCode, "Columns", properties.ToString());
            finalCode = CodeSection.Replace(finalCode, "ParameterList", parameterList.ToString() + "\r\n");
            finalCode = CodeSection.Replace(finalCode, "AssignmentList", assignmentList.ToString());
            finalCode = CodeSection.Replace(finalCode, "OtherAssignmentList", otherAssignmentList.ToString());

            return finalCode;
        }
    }
}
