using BSOA.Generator.Schema;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BSOA.Generator
{
    public class CodeGenerator
    {
        public TemplateType Type { get; set; }
        public string Code { get; set; }
        public Dictionary<string, string> Templates { get; set; }
        public Dictionary<string, string> PostReplacements { get; set; }

        public string OutputPathFormatString { get; set; }

        public CodeGenerator(TemplateType type, string templateFilePath, string outputPathFormatString)
        {
            Type = type;
            Code = File.ReadAllText(templateFilePath);
            Templates = CodeSection.AllTemplates(Code);

            OutputPathFormatString = outputPathFormatString;
        }

        public void Generate(string outputFolder, Database database)
        {
            if (Type == TemplateType.Database)
            {
                Generate(outputFolder, null, database);
            }
            else
            {
                foreach (Table table in database.Tables)
                {
                    Generate(outputFolder, table, database);
                }
            }
        }

        public void Generate(string outputFolder, Table table, Database database)
        {
            // Find every section in the template which ends with 'List'
            List<string> lists = Templates.Keys.Where((name) => name.EndsWith("List")).Select((name) => name.Substring(0, name.Length - 4)).ToList();

            Dictionary<string, StringBuilder> listValues = new Dictionary<string, StringBuilder>();
            foreach (string list in lists)
            {
                listValues[list] = new StringBuilder();
            }

            // Add a section to the list for each Table (for DB templates) or Column (for Table templates)
            if (Type == TemplateType.Database)
            {
                foreach (Schema.Table t in database.Tables)
                {
                    foreach (string list in lists)
                    {
                        listValues[list].Append(Templates[list].Replace(TemplateDefaults.TableName, t.Name));
                    }
                }
            }
            else
            {
                foreach (Schema.Column column in table.Columns)
                {
                    foreach (string list in lists)
                    {
                        listValues[list].Append(CodeSection.Populate(Templates, list, column));
                    }
                }
            }

            string finalCode = Code;

            // Replace every list in the template with the generated result
            foreach (string list in lists)
            {
                string valueForList = listValues[list].ToString();
                valueForList = valueForList.TrimEnd().TrimEnd(',') + Environment.NewLine;
                finalCode = CodeSection.Replace(finalCode, list + "List", valueForList);
            }

            // Replace the Database Name and Namespace defaults with the current values
            finalCode = finalCode
                .Replace(TemplateDefaults.Namespace, database.Namespace)
                .Replace(TemplateDefaults.DatabaseName, database.Name)
                .Replace(TemplateDefaults.RootTableName, database.RootTableName);

            // Replace the Table default with the Table name (for table templates)
            if (Type == TemplateType.Table)
            {
                finalCode = finalCode.Replace(TemplateDefaults.TableName, table.Name);
            }

            // Make any post-replacements
            finalCode = CodeSection.MakeReplacements(finalCode, PostReplacements);

            // Write to desired output folder
            string filePath = Path.Combine(outputFolder, string.Format(OutputPathFormatString, table?.Name ?? database.Name));
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, finalCode);
        }
    }
}
