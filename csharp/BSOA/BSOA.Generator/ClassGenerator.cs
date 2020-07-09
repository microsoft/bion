// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using BSOA.Generator.Schema;

namespace BSOA.Generator
{
    public class ClassGenerator
    {
        public TemplateType Type { get; set; }
        public string Code { get; set; }
        public Dictionary<string, string> Templates { get; set; }
        public PostReplacements PostReplacements { get; set; }

        public string OutputPathFormatString { get; set; }

        public ClassGenerator(TemplateType type, string templateFilePath, string outputPathFormatString, PostReplacements postReplacements = null)
        {
            Type = type;
            Code = File.ReadAllText(templateFilePath);
            Templates = CodeSection.AllTemplates(Code);
            PostReplacements = postReplacements;

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

            string filePath = Path.Combine(outputFolder, string.Format(OutputPathFormatString, table?.Name ?? database.Name));

            // Make any post-replacements
            finalCode = PostReplacements.Apply(filePath, finalCode);

            // Write to desired output folder
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, finalCode);
        }
    }
}
