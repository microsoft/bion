using BSOA.Generator.Generation;
using BSOA.Generator.Schema;
using BSOA.Json;

using System.Collections.Generic;

namespace BSOA.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database("SarifLogBsoa", "BSOA.Demo.Model")
            {
                new Table("Artifact")
                {
                    Schema.Column.Ref("Description", "Message"),
                    Schema.Column.Ref("Location", "ArtifactLocation"),
                    Schema.Column.Simple("ParentIndex", "int", "-1"),
                    Schema.Column.Simple("Offset", "int", "0"),
                    Schema.Column.Simple("Length", "int", "-1"),
                    Schema.Column.Simple("MimeType", "string"),
                    Schema.Column.Ref("Contents", "ArtifactContent"),
                    Schema.Column.Simple("Encoding", "string"),
                    Schema.Column.Simple("SourceLanguage", "string"),
                    Schema.Column.DateTime("LastModifiedTimeUtc", "DateTime.MinValue"),
                },
                new Table("ArtifactContent")
                {
                    Schema.Column.Simple("Text", "string"),
                    Schema.Column.Simple("Binary", "string"),
                },
                new Table("ArtifactLocation")
                {
                    Schema.Column.Simple("Uri", "Uri"),
                    Schema.Column.Simple("UriBaseId", "string"),
                    Schema.Column.Simple("Index", "int", "-1"),
                    Schema.Column.Ref("Description", "Message"),
                },
                new Table("Location")
                {
                    Schema.Column.Simple("Id", "int", "-1"),
                    Schema.Column.Ref("PhysicalLocation", "PhysicalLocation"),
                    Schema.Column.RefList("LogicalLocations", "LogicalLocation"),
                    Schema.Column.Ref("Message", "Message"),
                    Schema.Column.RefList("Annotations", "Region"),
                },
                new Table("LogicalLocation")
                {
                    Schema.Column.Simple("Name", "string"),
                    Schema.Column.Simple("Index", "int", "-1"),
                    Schema.Column.Simple("FullyQualifiedName", "string"),
                    Schema.Column.Simple("DecoratedName", "string"),
                    Schema.Column.Simple("ParentIndex", "int", "-1"),
                    Schema.Column.Simple("Kind", "string"),
                },
                new Table("Message")
                {
                    Schema.Column.Simple("Text", "string"),
                    Schema.Column.Simple("Markdown", "string"),
                    Schema.Column.Simple("Id", "string"),
                },
                new Table("PhysicalLocation")
                {
                    Schema.Column.Ref("ArtifactLocation", "ArtifactLocation"),
                    Schema.Column.Ref("Region", "Region"),
                    Schema.Column.Ref("ContextRegion", "Region"),
                },
                new Table("Region")
                {
                    Schema.Column.Simple("StartLine", "int", "0"),
                    Schema.Column.Simple("StartColumn", "int", "0"),
                    Schema.Column.Simple("EndLine", "int", "0"),
                    Schema.Column.Simple("EndColumn", "int", "0"),
                    Schema.Column.Simple("ByteOffset", "int", "-1"),
                    Schema.Column.Simple("ByteLength", "int", "0"),
                    Schema.Column.Simple("CharOffset", "int", "-1"),
                    Schema.Column.Simple("CharLength", "int", "0"),
                    Schema.Column.Ref("Snippet", "ArtifactContent"),
                    Schema.Column.Ref("Message", "Message"),
                    Schema.Column.Simple("SourceLanguage", "string"),
                },
                new Table("Result")
                {
                    Schema.Column.Enum("BaselineState", "Microsoft.CodeAnalysis.Sarif.BaselineState", "int", "Microsoft.CodeAnalysis.Sarif.BaselineState.None"),
                    Schema.Column.Simple("RuleId", "string"),
                    Schema.Column.Simple("RuleIndex", "int", "-1"),
                    Schema.Column.Ref("Message", "Message"),
                    Schema.Column.RefList("Locations", "Location"),
                    Schema.Column.Simple("Guid", "string"),
                },
                new Table("Run")
                {
                    Schema.Column.Ref("Tool", "Tool"),
                    Schema.Column.RefList("Artifacts", "Artifact"),
                    Schema.Column.RefList("Results", "Result"),
                },
                new Table("Tool")
                {
                    Schema.Column.Ref("Driver", "ToolComponent"),
                    Schema.Column.RefList("Extensions", "ToolComponent"),
                },
                new Table("ToolComponent")
                {
                    Schema.Column.Simple("Name", "string"),
                }
            };

            AsJson.Save("Sarif.BsoaSchema.json", db, verbose: true);

            List<ICodeGenerator> generators = new List<ICodeGenerator>()
            {
                new DatabaseModel(),
                new TableModel(),
                new EntityModel(@"Templates\\Sarif\\Team.cs")
            };

            string outputFolder = @"..\..\..\..\BSOA.Demo\Model";
            foreach (ICodeGenerator generator in generators)
            {
                generator.Generate(db, outputFolder);
            }
          }
    }
}
