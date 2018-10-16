using Bion.Json;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;

namespace Bion.Test
{
    [TestClass]
    public class BionLookupTests
    {
        [TestMethod]
        public void BionLookup_Basics()
        {
            using (BionLookup d = BionLookup.OpenWrite(new MemoryStream()))
            {
                short index;

                // Adding item
                Assert.IsTrue(d.TryLookup("Name", out index));
                Assert.AreEqual(0, index);

                // Existing item
                Assert.IsTrue(d.TryLookup("Name", out index));
                Assert.AreEqual(0, index);

                // Value too long
                Assert.IsFalse(d.TryLookup("1234567890_1234567890_1234567890_", out index));
                Assert.AreEqual(-1, index);

                // Add
                Assert.IsTrue(d.TryLookup("IsEnabled", out index));
                Assert.AreEqual(1, index);

                // Existing item
                Assert.IsTrue(d.TryLookup("Name", out index));
                Assert.AreEqual(0, index);

                // Lookup
                Assert.AreEqual("Name", d.PropertyName(0));
                Assert.AreEqual("IsEnabled", d.PropertyName(1));
            }
        }

        //[TestMethod]
        //public void BionLookup_ThroughReaderWriter()
        //{
        //    string jsonFilePath = @"CompareTests\Basics.json";
        //    string bionFilePath = "Basics.WithLookup.bion";
        //    string bionLookupFilePath = "Basics.WithLookup.Lookup.bion";

        //    using (JsonTextReader reader = new JsonTextReader(new StreamReader(jsonFilePath)))
        //    using (BionWriter writer = new BionWriter(new FileStream(bionFilePath, FileMode.Create), BionLookup.OpenWrite(bionLookupFilePath)))
        //    {
        //        JsonBionConverter.JsonToBion(reader, writer);
        //    }

        //    // Debuggability: Convert lookup to JSON
        //    JsonBionConverter.BionToJson(bionLookupFilePath, Path.ChangeExtension(bionLookupFilePath, ".json"));

        //    using (JsonTextReader jsonReader = new JsonTextReader(new StreamReader(jsonFilePath)))
        //    using (BionReader bionReader = new BionReader(new FileStream(bionFilePath, FileMode.Open), BionLookup.OpenRead(bionLookupFilePath)))
        //    {
        //        JsonBionComparer.Compare(jsonReader, bionReader);
        //    }
        //}
    }
}
