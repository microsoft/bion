using BSOA.Generator.Generation;
using BSOA.Generator.Schema;
using BSOA.Json;

using System;
using System.Collections.Generic;
using System.IO;

namespace BSOA.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: BSOA.Generator <SchemaJsonFile> <OutputFolder> [<EntityTemplate>]?");
                return;
            }

            string schemaPath = args[0];
            string outputFolder = (args.Length > 1 ? args[1] : @"Model");
            string entityTemplatePath = (args.Length > 2 ? args[2] : @"Templates\Team.cs");

            Database db = AsJson.Load<Database>(schemaPath);
            Directory.CreateDirectory(outputFolder);

            List<ICodeGenerator> generators = new List<ICodeGenerator>()
            {
                new DatabaseModel(),
                new TableModel(),
                new EntityModel(entityTemplatePath)
            };

            foreach (ICodeGenerator generator in generators)
            {
                generator.Generate(db, outputFolder);
            }
        }

        static Database SarifDemoSchema()
        {
            Database db = new Database("SarifLogBsoa", "BSOA.Demo.Model");
            Table table;

            table = new Table("Artifact");
            table.Columns.Add(Schema.Column.Ref("Description", "Message"));
            table.Columns.Add(Schema.Column.Ref("Location", "ArtifactLocation"));
            table.Columns.Add(Schema.Column.Simple("ParentIndex", "int", "-1"));
            table.Columns.Add(Schema.Column.Simple("Offset", "int", "0"));
            table.Columns.Add(Schema.Column.Simple("Length", "int", "-1"));
            table.Columns.Add(Schema.Column.Simple("MimeType", "string"));
            table.Columns.Add(Schema.Column.Ref("Contents", "ArtifactContent"));
            table.Columns.Add(Schema.Column.Simple("Encoding", "string"));
            table.Columns.Add(Schema.Column.Simple("SourceLanguage", "string"));
            table.Columns.Add(Schema.Column.DateTime("LastModifiedTimeUtc", "DateTime.MinValue"));
            db.Tables.Add(table);

            table = new Table("ArtifactContent");
            table.Columns.Add(Schema.Column.Simple("Text", "string"));
            table.Columns.Add(Schema.Column.Simple("Binary", "string"));
            db.Tables.Add(table);

            table = new Table("ArtifactLocation");
            table.Columns.Add(Schema.Column.Simple("Uri", "Uri"));
            table.Columns.Add(Schema.Column.Simple("UriBaseId", "string"));
            table.Columns.Add(Schema.Column.Simple("Index", "int", "-1"));
            table.Columns.Add(Schema.Column.Ref("Description", "Message"));
            db.Tables.Add(table);

            table = new Table("Location");
            table.Columns.Add(Schema.Column.Simple("Id", "int", "-1"));
            table.Columns.Add(Schema.Column.Ref("PhysicalLocation", "PhysicalLocation"));
            table.Columns.Add(Schema.Column.RefList("LogicalLocations", "LogicalLocation"));
            table.Columns.Add(Schema.Column.Ref("Message", "Message"));
            table.Columns.Add(Schema.Column.RefList("Annotations", "Region"));
            db.Tables.Add(table);

            table = new Table("LogicalLocation");
            table.Columns.Add(Schema.Column.Simple("Name", "string"));
            table.Columns.Add(Schema.Column.Simple("Index", "int", "-1"));
            table.Columns.Add(Schema.Column.Simple("FullyQualifiedName", "string"));
            table.Columns.Add(Schema.Column.Simple("DecoratedName", "string"));
            table.Columns.Add(Schema.Column.Simple("ParentIndex", "int", "-1"));
            table.Columns.Add(Schema.Column.Simple("Kind", "string"));
            db.Tables.Add(table);

            table = new Table("Message");
            table.Columns.Add(Schema.Column.Simple("Text", "string"));
            table.Columns.Add(Schema.Column.Simple("Markdown", "string"));
            table.Columns.Add(Schema.Column.Simple("Id", "string"));
            db.Tables.Add(table);

            table = new Table("PhysicalLocation");
            table.Columns.Add(Schema.Column.Ref("ArtifactLocation", "ArtifactLocation"));
            table.Columns.Add(Schema.Column.Ref("Region", "Region"));
            table.Columns.Add(Schema.Column.Ref("ContextRegion", "Region"));
            db.Tables.Add(table);

            table = new Table("Region");
            table.Columns.Add(Schema.Column.Simple("StartLine", "int", "0"));
            table.Columns.Add(Schema.Column.Simple("StartColumn", "int", "0"));
            table.Columns.Add(Schema.Column.Simple("EndLine", "int", "0"));
            table.Columns.Add(Schema.Column.Simple("EndColumn", "int", "0"));
            table.Columns.Add(Schema.Column.Simple("ByteOffset", "int", "-1"));
            table.Columns.Add(Schema.Column.Simple("ByteLength", "int", "0"));
            table.Columns.Add(Schema.Column.Simple("CharOffset", "int", "-1"));
            table.Columns.Add(Schema.Column.Simple("CharLength", "int", "0"));
            table.Columns.Add(Schema.Column.Ref("Snippet", "ArtifactContent"));
            table.Columns.Add(Schema.Column.Ref("Message", "Message"));
            table.Columns.Add(Schema.Column.Simple("SourceLanguage", "string"));
            db.Tables.Add(table);

            table = new Table("Result");
            table.Columns.Add(Schema.Column.Enum("BaselineState", "Microsoft.CodeAnalysis.Sarif.BaselineState", "int", "Microsoft.CodeAnalysis.Sarif.BaselineState.None"));
            table.Columns.Add(Schema.Column.Simple("RuleId", "string"));
            table.Columns.Add(Schema.Column.Simple("RuleIndex", "int", "-1"));
            table.Columns.Add(Schema.Column.Ref("Message", "Message"));
            table.Columns.Add(Schema.Column.RefList("Locations", "Location"));
            table.Columns.Add(Schema.Column.Simple("Guid", "string"));
            db.Tables.Add(table);

            table = new Table("Run");
            table.Columns.Add(Schema.Column.Ref("Tool", "Tool"));
            table.Columns.Add(Schema.Column.RefList("Artifacts", "Artifact"));
            table.Columns.Add(Schema.Column.RefList("Results", "Result"));
            db.Tables.Add(table);

            table = new Table("Tool");
            table.Columns.Add(Schema.Column.Ref("Driver", "ToolComponent"));
            table.Columns.Add(Schema.Column.RefList("Extensions", "ToolComponent"));
            db.Tables.Add(table);

            table = new Table("ToolComponent");
            table.Columns.Add(Schema.Column.Simple("Name", "string"));
            db.Tables.Add(table);

            AsJson.Save(@"..\..\..\BsoaDemo.schema.json", db, verbose: true);
            return db;
        }
    }
}
