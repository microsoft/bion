using BSOA.Generator.Generation;
using BSOA.Generator.Schema;
using System.IO;

namespace BSOA.Generator
{
    public class CodeGenerator
    {
        public string OutputFolder { get; set; }

        public void Generate(Database database)
        {
            if (Directory.Exists(OutputFolder)) { Directory.Delete(OutputFolder, true); }
            Directory.CreateDirectory(OutputFolder);

            foreach (Table table in database)
            {
                Generate(table, database);
            }
        }

        public void Generate(Table table, Database database)
        {
            string code = EntityModel.Generate(table, database);
            File.WriteAllText(Path.Combine(OutputFolder, $"{table.Name}.cs"), code);
        }
    }
}
