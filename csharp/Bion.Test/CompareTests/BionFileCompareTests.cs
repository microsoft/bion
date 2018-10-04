using Bion.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Bion.Test.CompareTests
{
    [TestClass]
    public class BionFileCompareTests
    {
        [TestMethod]
        public void Basics()
        {
            Compare(@"CompareTests\Basics.json");
        }

        private static void Compare(string jsonFilePath)
        {
            string bionFilePath = Path.ChangeExtension(jsonFilePath, ".bion");
            JsonBionConverter.JsonToBion(jsonFilePath, bionFilePath);
            JsonBionComparer.Compare(jsonFilePath, bionFilePath);
        }
    }
}
