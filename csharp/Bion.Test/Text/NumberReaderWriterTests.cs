using Bion.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Bion.Test.Text
{
    [TestClass]
    public class NumberReaderWriterTests
    {
        [TestMethod]
        public void RoundTrip()
        {
            string fileName = "Sample.bin";
            using (NumberWriter writer = new NumberWriter(File.OpenWrite(fileName)))
            {
                for(int i = 1000; i < 100000; ++i)
                {
                    writer.WriteValue((uint)i);
                }
            }

            using (NumberReader reader = new NumberReader(File.OpenRead(fileName)))
            {
                for(int i = 1000; i < 100000; ++i)
                {
                    int value = (int)reader.ReadNumber();
                    Assert.AreEqual(i, value);
                }
            }
        }
    }
}
