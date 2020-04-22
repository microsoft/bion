namespace BSOA.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputFilePath = (args.Length > 0 ? args[0] : @"C:\Download\Demo\V2\Inputs\CodeAsData.sarif");
            string workingFolderPath = @"C:\Download\Demo\V2";

            // Tiny demo file
            inputFilePath = @"C:\Download\Demo\V2\Inputs\elfie-arriba.sarif";

            Benchmarker benchmarker = new Benchmarker(inputFilePath, workingFolderPath);
            benchmarker.Run();
        }
    }
}
