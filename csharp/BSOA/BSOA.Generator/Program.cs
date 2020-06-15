// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using BSOA.Generator.Schema;
using BSOA.Json;

namespace BSOA.Generator
{
    /// <summary>
    ///  BSOA.Generator generates a BSOA object model (Entity classes, Table classes, and a Database class)
    ///  from schema information in a JSON format. See Schemas\ for examples.
    ///  
    ///  This code uses Regexes and string replacement to generate the output files from the templates.
    ///  Roslyn is much more flexible, but Roslyn generation code is long and complex.
    ///  [Ex: See https://github.com/microsoft/jschema/blob/master/src/Json.Schema.ToDotNet/ClassGenerator.cs#L1129]
    ///  
    ///  You provide templates for each class you want generated.
    ///  Templates use known values for the namespace, database name, table name, and column properties.
    ///  See Generation\TemplateDefaults.cs for the expected known values.
    ///  
    ///  Within each template, the code will find all &lt;[TemplateName]List&gt; comment blocks.
    ///  It will generate per-column or per-table replacements by replacing the value from the
    ///  &lt;[ColumnTypeCategory][TemplateName]&gt; or &lt;[TemplateName]&gt; block, and then replace
    ///  the list block with the created output. The code then replaces the Database name, Table name, and namespace.
    ///  
    ///  This logic is straightforward and means you can make a working template which can be unit tested,
    ///  annotate it with comments indicating where to make replacements, and then get predictable generated
    ///  outputs for any schema.
    /// </summary>
    class Program
    {
        private const string DefaultTemplateFolderPath = @"Templates";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: BSOA.Generator <SchemaJsonFile> <OutputFolder> [<TemplateFolderPath>]?");
                return;
            }

            string schemaPath = args[0];
            string outputFolder = (args.Length > 1 ? args[1] : @"Model");
            string templateFolderPath = (args.Length > 2 ? args[2] : DefaultTemplateFolderPath).TrimEnd('\\');

            Console.WriteLine($"Generating BSOA object model from schema\r\n  '{schemaPath}' at \r\n  '{outputFolder}'...");

            Database db = AsJson.Load<Database>(schemaPath);

            if (Directory.Exists(outputFolder)) { Directory.Delete(outputFolder, true); }
            Directory.CreateDirectory(outputFolder);

            Dictionary<string, string> postReplacements = new Dictionary<string, string>()
            {
                ["^[ \t]+\\[DefaultValue\\(null\\)\\][ \t\r]*\n"] = "",
                ["^[ \t]+\\[DefaultValue\\(0\\)\\][ \t\r]*\n"] = "",
                ["^[ \t]+\\[DefaultValue\\(\\)\\][ \t\r]*\n"] = "",
                [Regex.Escape("PropertyBag : PropertyBagHolder, ")] = "PropertyBag : ",
                [Regex.Escape(@"[DefaultValue(DateTime.MinValue)]")] = "[JsonConverter(typeof(DateTimeConverter))]",
                
                ["EnumConverter\\)\\)\\][^\n]*\n\\s+public SarifVersion"] = @"SarifVersionConverter))]
        public SarifVersion",

                [@"Name = ""schemaUri"""] = @"Name = ""$schema""",

                ["public Uri SchemaUri"] = @"[JsonConverter(typeof(UriConverter))]
        public Uri SchemaUri",

                ["public IDictionary<string, string> Properties"] = "internal override IDictionary<string, string> Properties",

                [Regex.Escape(@"[DataMember(Name = ""properties"", IsRequired = false, EmitDefaultValue = false)]
        [DefaultValue(-1)]")] = @"[DataMember(Name = ""properties"", IsRequired = false, EmitDefaultValue = false)]",

                ["ColumnFactory.Build<IDictionary<string, MultiformatMessageString>>\\(\\)\\);"] = "new DictionaryColumn<string, MultiformatMessageString>(new StringColumn(), new MultiformatMessageStringColumn(this.Database)));",
                ["ColumnFactory.Build<IDictionary<string, ArtifactLocation>>\\(\\)\\);"] = "new DictionaryColumn<string, ArtifactLocation>(new StringColumn(), new ArtifactLocationColumn(this.Database)));"
            };

            // Generate Database class
            new CodeGenerator(TemplateType.Database, TemplatePath(templateFolderPath, @"Internal\CompanyDatabase.cs"), @"Internal\{0}.cs") { PostReplacements = postReplacements }
                .Generate(outputFolder, db);

            // Generate Tables
            new CodeGenerator(TemplateType.Table, TemplatePath(templateFolderPath, @"Internal\TeamTable.cs"), @"Internal\{0}Table.cs") { PostReplacements = postReplacements }
                .Generate(outputFolder, db);

            // Generate Entities
            new CodeGenerator(TemplateType.Table, TemplatePath(templateFolderPath, @"Team.cs"), "{0}.cs") { PostReplacements = postReplacements }
                .Generate(outputFolder, db);

            // Generate Root Entity (overwrite normal style)
            new CodeGenerator(TemplateType.Table, TemplatePath(templateFolderPath, @"Company.cs"), @"{0}.cs") { PostReplacements = postReplacements }
                .Generate(outputFolder, db.Tables.Where((table) => table.Name.Equals(db.RootTableName)).First(), db);

            Console.WriteLine("Done.");
            Console.WriteLine();
        }

        static string TemplatePath(string templateFolderPath, string templateFilePath)
        {
            string path = Path.Combine(templateFolderPath, templateFilePath);
            return File.Exists(path) ? path : Path.Combine(DefaultTemplateFolderPath, templateFilePath);
        }
    }
}
