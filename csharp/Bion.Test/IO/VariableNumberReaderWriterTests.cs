using Bion.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Bion.Test.Text
{
    [TestClass]
    public class VariableNumberReaderWriterTests
    {
        [TestMethod]
        public void RoundTrip()
        {
            string fileName = "Sample.bin";
            long bytes;

            using (VariableNumberWriter writer = new VariableNumberWriter(File.Create(fileName)))
            {
                for(int i = 1000; i < 100000; ++i)
                {
                    writer.WriteValue((uint)i);
                }

                bytes = writer.BytesWritten;
            }

            using (VariableNumberReader reader = new VariableNumberReader(File.OpenRead(fileName)))
            {
                Assert.IsFalse(reader.EndOfStream);

                for(int i = 1000; i < 100000; ++i)
                {
                    int value = (int)reader.ReadNumber();
                    Assert.AreEqual(i, value);
                }

                Assert.IsTrue(reader.EndOfStream);
                Assert.AreEqual(bytes, reader.BytesRead);
            }
        }
    }
}
