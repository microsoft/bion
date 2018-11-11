using Bion.IO;
using Bion.Vector;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Bion.Test.Vector
{
    [TestClass]
    public class IntBlockTests
    {
        [TestMethod]
        public void IntBlock_Scenarios()
        {
            Random r = new Random(6);

            // Zero: 1 byte (noAdjustMarker)
            RoundTrip(Enumerable.Repeat(0, 128).ToArray(), 1);

            // 10: 3 bytes (baseMarker, 1b Base, noAdjustMarker)
            RoundTrip(Enumerable.Repeat(10, 128).ToArray(), 3);

            // 0-127: 3 bytes (slopeMarker, 1b slope, noAdjustMarker)
            RoundTrip(Enumerable.Range(0, 128).ToArray(), 3);

            // 127-0: 8 bytes (baseMarker, 1b base, slopeMarker, 4b slope, noAdjustMarker)
            RoundTrip(Enumerable.Range(0, 128).Reverse().ToArray(), 8);

            // 0-15 random: 65 bytes (adjustMarker, 4 bit x 128 = 64 bytes)
            RoundTrip(Build(128, () => r.Next(16)), 65);

            // 0-63 random: 97 bytes (adjustMarker, 6 bit x 128 = 96 bytes)
            RoundTrip(Build(128, () => r.Next(64)), 97);

            // Short, Zero: 3 bytes (countMarker, 1b Count, noAdjustMarker)
            RoundTrip(Enumerable.Repeat(0, 50).ToArray(), 3);

            // Normal + Short, 97 bytes + 15 bytes (countMarker, 1b Count, adjustMarker, 6 bit x [10 -> 16] = 12 bytes)
            RoundTrip(Build(138, () => r.Next(64)), 97 + 15);           
        }

        private static int[] Build(int count, Func<int> next)
        {
            int[] array = new int[count];
            for (int i = 0; i < array.Length; ++i)
            {
                array[i] = next();
            }
            return array;
        }

        private static void RoundTrip(int[] values, long expectedLength)
        {
            string fileName = "IntBlock.bin";

            using (IntBlockWriter writer = new IntBlockWriter(new BufferedWriter(File.Create(fileName))))
            {
                for (int i = 0; i < values.Length; ++i)
                {
                    writer.Write(values[i]);
                }
            }

            Assert.AreEqual(new FileInfo(fileName).Length, expectedLength);

            using (IntBlockReader reader = new IntBlockReader(new BufferedReader(File.OpenRead(fileName))))
            {
                int[] readValues;
                int index = 0;
                int count = 0;

                do
                {
                    count = reader.Next(out readValues);
                    for (int i = 0; i < count; ++i)
                    {
                        Assert.AreEqual(values[index++], readValues[i]);
                    }
                } while (count == IntBlockPlan.BlockSize);

                Assert.AreEqual(values.Length, index);
            }
        }
    }
}
