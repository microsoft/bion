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

            // Database Class
            File.WriteAllText(Path.Combine(OutputFolder, $"{database.Name}.cs"), DatabaseModel.Generate(database));

            foreach (Table table in database)
            {
                Generate(table, database);
            }
        }

        public void Generate(Table table, Database database)
        {
            // Entity Class
            File.WriteAllText(Path.Combine(OutputFolder, $"{table.Name}.cs"), EntityModel.Generate(table, database));

            // Table Class
            File.WriteAllText(Path.Combine(OutputFolder, $"{table.Name}Table.cs"), TableModel.Generate(table, database));
        }
    }
}
