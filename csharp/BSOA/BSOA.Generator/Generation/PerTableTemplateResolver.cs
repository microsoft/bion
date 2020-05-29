using BSOA.Generator.Schema;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BSOA.Generator.Generation
{
    /// <summary>
    ///  Construct a file from a database template.
    ///  This will replace all &lt;...List&gt; sections in the file with per-table populated values.
    /// </summary>
    public class PerTableTemplateResolver : ICodeGenerator
    {
        public string FileNameSuffix;
        public string Code;
        public Dictionary<string, string> Templates;

        public PerTableTemplateResolver() : this("", @"Templates\\CompanyDatabase.cs")
        { }

        public PerTableTemplateResolver(string fileNameSuffix, string templateFilePath)
        {
            FileNameSuffix = fileNameSuffix;
            Code = File.ReadAllText(templateFilePath);
            Templates = CodeSection.AllTemplates(Code);
        }

        public virtual void Generate(Database database, string outputPath)
        {
            File.WriteAllText(Path.Combine(outputPath, $"{database.Name}{FileNameSuffix}.cs"), Generate(database));
        }

        public virtual string Generate(Database database)
        {
            // Find every section in the template which ends with 'List'
            List<string> lists = Templates.Keys.Where((name) => name.EndsWith("List")).Select((name) => name.Substring(0, name.Length - 4)).ToList();

            Dictionary<string, StringBuilder> listValues = new Dictionary<string, StringBuilder>();
            foreach (string list in lists)
            {
                listValues[list] = new StringBuilder();
            }

            // For each list, concatenate a string with a value for every column.
            // This uses column-type-specific templates when found, or generic templates
            foreach (Schema.Table table in database.Tables)
            {
                foreach (string list in lists)
                {
                    listValues[list].Append(Templates[list].Replace(TemplateDefaults.TableName, table.Name));
                }
            }

            string finalCode = Code;

            // Replace every list in the template with the per-column generated result
            foreach (string list in lists)
            {
                string valueForList = listValues[list].ToString();
                valueForList = valueForList.TrimEnd().TrimEnd(',') + Environment.NewLine;
                finalCode = CodeSection.Replace(finalCode, list + "List", valueForList);
            }

            finalCode = finalCode
                .Replace(TemplateDefaults.DatabaseName, database.Name)
                .Replace(TemplateDefaults.Namespace, database.Namespace);

            return finalCode;
        }
    }
}
