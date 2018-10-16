using Bion.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace Bion.Test.Text
{
    [TestClass]
    public class NumberConverterTests
    {
        [TestMethod]
        public void RoundTrip_SevenBit()
        {
            BufferedRoundTrip("SevenBit.bin",
                (writer) =>
                {
                    for (int i = 0; i < 100000; ++i)
                    {
                        NumberConverter.WriteSevenBit((uint)i, writer);
                    }
                },
                (reader) =>
                {
                    int expected = 0;
                    while (!reader.EndOfStream)
                    {
                        int value = (int)NumberConverter.ReadSevenBit(reader);
                        Assert.AreEqual(expected, value);
                        expected++;
                    }

                    Assert.AreEqual(100000, expected);
                }
            );
        }

        [TestMethod]
        public void RoundTrip_SixBit()
        {
            BufferedRoundTrip("SixBit.bin",
                (writer) =>
                {
                    for (int i = 0; i < 100000; ++i)
                    {
                        NumberConverter.WriteSixBit((uint)i, writer);
                    }
                },
                (reader) =>
                {
                    int expected = 0;
                    while (!reader.EndOfStream)
                    {
                        int value = (int)NumberConverter.ReadSixBit(reader);
                        Assert.AreEqual(expected, value);
                        expected++;
                    }

                    Assert.AreEqual(100000, expected);
                }
            );
        }

        private static void BufferedRoundTrip(string fileName, Action<BufferedWriter> write, Action<BufferedReader> read)
        {
            long bytes;

            using (BufferedWriter writer = new BufferedWriter(File.Create(fileName)))
            {
                Assert.AreEqual(0, writer.BytesWritten);
                write(writer);
                bytes = writer.BytesWritten;
            }

            Assert.AreEqual(new FileInfo(fileName).Length, bytes);

            using (BufferedReader reader = new BufferedReader(File.OpenRead(fileName)))
            {
                Assert.IsFalse(reader.EndOfStream);
                read(reader);
                Assert.IsTrue(reader.EndOfStream);
                Assert.AreEqual(bytes, reader.BytesRead);
            }
        }
    }
}
