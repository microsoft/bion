using Bion.Vector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Bion.Test.Vector
{
    [TestClass]
    public class ByteVectorTests
    {
        [TestMethod]
        public void ByteVector_IndexOf()
        {
            byte[] sample = Enumerable.Range(0, 250).Select((i) => (byte)i).ToArray();

            Assert.AreEqual(0, ByteVector.IndexOf(0, sample, 0, sample.Length));            // First
            Assert.AreEqual(1, ByteVector.IndexOf(1, sample, 0, sample.Length));            // Second
            Assert.AreEqual(200, ByteVector.IndexOf(200, sample, 0, sample.Length));        // Middle
            Assert.AreEqual(249, ByteVector.IndexOf(249, sample, 0, sample.Length));        // Last

            Assert.AreEqual(-1, ByteVector.IndexOf(0, sample, 1, sample.Length));           // Index respected
            Assert.AreEqual(1, ByteVector.IndexOf(1, sample, 1, sample.Length));            // Index respected
            Assert.AreEqual(248, ByteVector.IndexOf(248, sample, 0, sample.Length - 1));    // Index respected
            Assert.AreEqual(-1, ByteVector.IndexOf(249, sample, 0, sample.Length -1));      // Index respected
        }

        [TestMethod]
        public void ByteVector_GreaterThan()
        {
            byte[] sample = Enumerable.Range(0, 250).Select((i) => (byte)i).ToArray();

            Assert.AreEqual(1, ByteVector.GreaterThan(0, sample, 0, sample.Length));            // First
            Assert.AreEqual(2, ByteVector.GreaterThan(1, sample, 0, sample.Length));            // Second
            Assert.AreEqual(201, ByteVector.GreaterThan(200, sample, 0, sample.Length));        // Middle
            Assert.AreEqual(249, ByteVector.GreaterThan(248, sample, 0, sample.Length));        // Last
            Assert.AreEqual(-1, ByteVector.GreaterThan(249, sample, 0, sample.Length));         // Last

            Assert.AreEqual(5, ByteVector.GreaterThan(0, sample, 5, sample.Length));            // Index respected
            Assert.AreEqual(6, ByteVector.GreaterThan(5, sample, 5, sample.Length));            // Index respected
            Assert.AreEqual(-1, ByteVector.GreaterThan(248, sample, 0, sample.Length - 5));     // Index respected
            Assert.AreEqual(244, ByteVector.GreaterThan(243, sample, 0, sample.Length - 5));    // Index respected
        }

        [TestMethod]
        public void ByteVector_Skip()
        {
            byte[] sample = Enumerable.Repeat((byte)0, 255).ToArray();
            sample[0] = (byte)BionToken.StartArray;
            sample[1] = (byte)BionToken.StartObject;

            sample[10] = (byte)BionToken.StartArray;
            sample[11] = (byte)BionToken.EndArray;

            sample[250] = (byte)BionToken.EndObject;
            sample[251] = (byte)BionToken.EndArray;

            int depth;

            // Inside Object, find end
            depth = 1;
            Assert.AreEqual(250, ByteVector.Skip(sample, 2, sample.Length, ref depth));

            // Inside Object, find parent end (array)
            depth = 2;
            Assert.AreEqual(251, ByteVector.Skip(sample, 2, sample.Length, ref depth));

            // Inside array, find end
            depth = 1;
            Assert.AreEqual(251, ByteVector.Skip(sample, 1, sample.Length, ref depth));

            // Inside nothing, find end (not enough ends)
            depth = 1;
            Assert.AreEqual(255, ByteVector.Skip(sample, 0, sample.Length, ref depth));
        }
    }
}
