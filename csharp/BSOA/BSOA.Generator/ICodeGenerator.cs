using BSOA.Generator.Schema;

namespace BSOA.Generator.Generation
{
    public interface ICodeGenerator
    {
        void Generate(Database database, string outputFolder);
    }
}
