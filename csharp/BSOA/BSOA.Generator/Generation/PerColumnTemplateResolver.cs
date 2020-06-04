using BSOA.Generator.Schema;

using Microsoft.VisualBasic.CompilerServices;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BSOA.Generator.Generation
{
    /// <summary>
    ///  Construct a file from a per-table template.
    ///  This will replace all &lt;...List&gt; sections in the file with per-column populated values.
    /// </summary>
    public class PerColumnTemplateResolver : ICodeGenerator
    {
        public string FileNameSuffix { get; set; }
        public string Code { get; set; }
        public Dictionary<string, string> Templates { get; set; }
        public Dictionary<string, string> PostReplacements { get; set; }

        public PerColumnTemplateResolver() : this("", @"Templates\\Team.cs")
        { }

        public PerColumnTemplateResolver(string fileNameSuffix, string templateFilePath)
        {
            FileNameSuffix = fileNameSuffix;
            Code = File.ReadAllText(templateFilePath);
            Templates = CodeSection.AllTemplates(Code);
        }

        public virtual void Generate(Database database, string outputFolder)
        {
            foreach (Table table in database.Tables)
            {
                File.WriteAllText(Path.Combine(outputFolder, $"{table.Name}{FileNameSuffix}.cs"), Generate(table, database));
            }
        }

        public virtual string Generate(Table table, Database database)
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
            foreach (Schema.Column column in table.Columns)
            {
                foreach (string list in lists)
                {
                    listValues[list].Append(CodeSection.Populate(Templates, list, column));
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

            // Replace the Table Name, Database Name, and Namespace defaults with the current values
            finalCode = finalCode
                .Replace(TemplateDefaults.TableName, table.Name)
                .Replace(TemplateDefaults.DatabaseName, database.Name)
                .Replace(TemplateDefaults.Namespace, database.Namespace);

            finalCode = CodeSection.MakeReplacements(finalCode, PostReplacements);

            return finalCode;
        }
    }
}
