using Bion.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Bion.Test.CompareTests
{
    [TestClass]
    public class BionReaderWriterTests
    {
        [TestMethod]
        public void Basics_Uncompressed()
        {
            CompareUncompressed(@"Content\Basics.json");
        }

        [TestMethod]
        public void Medium_Uncompressed()
        {
            CompareUncompressed(@"Content\Medium.json");
        }

        [TestMethod]
        public void Basics_Compressed()
        {
            CompareCompressed(@"Content\Basics.json");
        }

        [TestMethod]
        public void Medium_Compressed()
        {
            CompareCompressed(@"Content\Medium.json");
        }

        private static void CompareUncompressed(string jsonFilePath)
        {
            string bionFilePath = Path.ChangeExtension(jsonFilePath, ".bion");
            string comparePath = Path.ChangeExtension(jsonFilePath, "compare.json");

            JsonBionConverter.JsonToBion(jsonFilePath, bionFilePath);
            JsonBionConverter.BionToJson(bionFilePath, comparePath);
            Verify.JsonEqual(jsonFilePath, comparePath);
        }

        private static void CompareCompressed(string jsonFilePath)
        {
            string bionFilePath = Path.ChangeExtension(jsonFilePath, ".bion");
            string dictionaryPath = Path.ChangeExtension(bionFilePath, "dict.bion");
            string comparePath = Path.ChangeExtension(jsonFilePath, "compare.json");
            JsonBionConverter.JsonToBion(jsonFilePath, bionFilePath, dictionaryPath);
            JsonBionConverter.BionToJson(bionFilePath, comparePath, dictionaryPath);
            Verify.JsonEqual(jsonFilePath, comparePath);
        }
    }
}
