using BSOA.Generator.Schema;

namespace BSOA.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            Database db = new Database("RegionDatabase", "BSOA.Demo.Model")
            {
                new Table("Region")
                {
                    Schema.Column.Simple("StartLine", "int", "-1"),
                    Schema.Column.Simple("StartColumn", "int", "-1"),
                    Schema.Column.Simple("EndLine", "int", "-1"),
                    Schema.Column.Simple("EndColumn", "int", "-1"),
                    Schema.Column.Ref("Snippet", "ArtifactContent"),
                },
                new Table("ArtifactContent")
                {
                    Schema.Column.Simple("Text", "string"),
                    Schema.Column.Simple("Binary", "string"),
                }
            };

            CodeGenerator generator = new CodeGenerator()
            {
                OutputFolder = "Model",
            };

            generator.Generate(db);
        }
    }
}
