using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Bion.Test
{
    [TestClass]
    public class BionRoundTripBasics
    {
        [TestMethod]
        public void Integers()
        {
            RoundTrip(0);       // Inline
            RoundTrip(15);      // Inline limit
            RoundTrip(16);      // Non-Inline min
            RoundTrip(127);     // 1b Max
            RoundTrip(128);     // 2b Min
            RoundTrip(14383);   // 2b Max
            RoundTrip(14384);   // 3b Min

            RoundTrip(short.MaxValue);
            RoundTrip(int.MaxValue);
            RoundTrip(long.MaxValue);

            RoundTrip(-1);      // Negative (1b)
            RoundTrip(-127);    // 1b Max
            RoundTrip(-128);    // 2b Min
            RoundTrip(-14383);  // 2b Max
            RoundTrip(-14384);  // 3b Min

            RoundTrip(short.MinValue);
            RoundTrip(int.MinValue);
            RoundTrip(long.MinValue);
        }

        private static void RoundTrip(long value)
        {
            byte[] buffer = new byte[20];

            using (MemoryStream stream = new MemoryStream(buffer))
            {
                using (BionWriter writer = new BionWriter(stream))
                {
                    writer.CloseStream = false;
                    writer.WriteValue(value);
                }

                long length = stream.Position;
                stream.Seek(0, SeekOrigin.Begin);

                using (BionReader reader = new BionReader(stream))
                {
                    reader.CloseStream = false;

                    Assert.IsTrue(reader.Read());
                    Assert.AreEqual(BionToken.Integer, reader.TokenType);

                    long valueRead = reader.CurrentInteger();
                    Assert.AreEqual(value, valueRead);

                    Assert.AreEqual(length, reader.BytesRead);
                }
            }
        }
    }
}
