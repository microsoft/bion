using BSOA.Generator.Schema;
using BSOA.Json;

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BSOA.Generator
{
    public class FullGenerator
    {
        private string SchemaPath { get; }
        private string OutputFolder { get; }
        private string TemplateDefaultPath { get; }
        private string TemplateOverridesFolderPath { get; }
        private string PostReplacementsPath { get; }

        public FullGenerator(string schemaPath, string outputFolder, string templateOverridesFolderPath = null, string postReplacementPath = null)
        {
            SchemaPath = schemaPath;
            OutputFolder = outputFolder;
            TemplateDefaultPath = FindTemplateDefaultPath();
            TemplateOverridesFolderPath = templateOverridesFolderPath;
            PostReplacementsPath = postReplacementPath;
        }

        public void Generate()
        {
            Console.WriteLine($"Generating BSOA object model from schema\r\n  '{SchemaPath}' at \r\n  '{OutputFolder}'...");

            Database db = AsJson.Load<Database>(SchemaPath);

            if (Directory.Exists(OutputFolder)) { Directory.Delete(OutputFolder, true); }
            Directory.CreateDirectory(OutputFolder);

            PostReplacements postReplacements = new PostReplacements();
            if (PostReplacementsPath != null)
            {
                postReplacements = AsJson.Load<PostReplacements>(PostReplacementsPath);
            }

            // List and Dictionary read and write methods need a writeValue delegate passed
            AddDefaultPostReplacements(postReplacements);

            // Generate Database class
            new ClassGenerator(TemplateType.Database, TemplatePath(@"Internal\CompanyDatabase.cs"), @"Internal\{0}.cs", postReplacements)
                .Generate(OutputFolder, db);

            // Generate Tables
            new ClassGenerator(TemplateType.Table, TemplatePath(@"Internal\TeamTable.cs"), @"Internal\{0}Table.cs", postReplacements)
                .Generate(OutputFolder, db);

            // Generate Entities
            new ClassGenerator(TemplateType.Table, TemplatePath(@"Team.cs"), "{0}.cs", postReplacements)
                .Generate(OutputFolder, db);

            // Generate Root Entity (overwrite normal entity form)
            new ClassGenerator(TemplateType.Table, TemplatePath(@"Company.cs"), @"{0}.cs", postReplacements)
                .Generate(OutputFolder, db.Tables.Where((table) => table.Name.Equals(db.RootTableName)).First(), db);

            // Generate Entity Json Converter
            new ClassGenerator(TemplateType.Table, TemplatePath(@"Json\JsonToTeam.cs"), @"Json\JsonTo{0}.cs", postReplacements)
                .Generate(OutputFolder, db);

            // Generate Root Entity Json Converter (overwrite normal entity form)
            new ClassGenerator(TemplateType.Table, TemplatePath(@"Json\JsonToCompany.cs"), @"Json\JsonTo{0}.cs", postReplacements)
                .Generate(OutputFolder, db.Tables.Where((table) => table.Name.Equals(db.RootTableName)).First(), db);

            Console.WriteLine("Done.");
            Console.WriteLine();
        }

        private static void AddDefaultPostReplacements(PostReplacements postReplacements)
        {
            postReplacements.Replacements.Add(new PostReplacement(
                replace: "me.([^ ]+) = JsonToIList<([^>]+)>.Read\\(reader, root\\)",
                with: "me.$1 = JsonToIList<$2>.Read(reader, root, null, JsonTo$2.Read)"
            ));

            postReplacements.Replacements.Add(new PostReplacement(
                replace: "JsonToIList<([^>]+)>.Write\\(writer, ([^,]+), item.([^,]+), default\\);",
                with: "JsonToIList<$1>.Write(writer, $2, item.$3, JsonTo$1.Write);"
            ));

            postReplacements.Replacements.Add(new PostReplacement(
                replace: "me.([^ ]+) = JsonToIDictionary<String, ([^>]+)>.Read\\(reader, root\\)",
                with: @"me.$1 = JsonToIDictionary<String, $2>.Read(reader, root, null, JsonTo$2.Read)"
            ));

            postReplacements.Replacements.Add(new PostReplacement(
                replace: "JsonToIDictionary<String, ([^>]+)>.Write\\(writer, ([^,]+), item.([^,]+), default\\);",
                with: "JsonToIDictionary<String, $1>.Write(writer, $2, item.$3, JsonTo$1.Write);"
            ));
            
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonTobyte.", with: "JsonToByte.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonTosbyte.", with: "JsonToSByte.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToshort.", with: "JsonToShort.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToushort.", with: "JsonToUShort.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToint.", with: "JsonToInt.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonTouint.", with: "JsonToUInt.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonTolong.", with: "JsonToLong.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToulong.", with: "JsonToULong.", arePlainText: true));

            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonTofloat.", with: "JsonToFloat.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonTodouble.", with: "JsonToDouble.", arePlainText: true));

            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToInt8.", with: "JsonToSByte.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToUInt8.", with: "JsonToByte.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToInt16.", with: "JsonToShort.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToUInt16.", with: "JsonToUShort.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToInt32.", with: "JsonToInt.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToUInt32.", with: "JsonToUInt.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToInt64.", with: "JsonToLong.", arePlainText: true));
            postReplacements.Replacements.Add(new PostReplacement(replace: "JsonToUInt64.", with: "JsonToULong.", arePlainText: true));
        }

        private string FindTemplateDefaultPath()
        {
            string templatePath;
            string exePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            templatePath = Path.Combine(exePath, "Templates");
            if (Directory.Exists(templatePath)) { return templatePath; }

            return Path.Combine(Environment.CurrentDirectory, "Templates");
        }

        private string TemplatePath(string relativeFilePath)
        {
            string candidatePath;

            if (!string.IsNullOrEmpty(TemplateOverridesFolderPath))
            {
                candidatePath = Path.Combine(TemplateOverridesFolderPath, relativeFilePath);
                if (File.Exists(candidatePath)) { return candidatePath; }
            }

            return Path.Combine(TemplateDefaultPath, relativeFilePath);
        }
    }
}
