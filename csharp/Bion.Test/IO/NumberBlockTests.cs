// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.IO;

using Bion.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test.Text
{
    [TestClass]
    public class NumberBlockTests
    {
        [TestMethod]
        public void NumberBlock_RoundTrip()
        {
            string path = "NumberBlockTest.bin";
            int blockSize = 16;
            int count = blockSize;
            Random r;

            long bytesWritten = 0;

            r = new Random(5);
            using (BufferedWriter bufferedWriter = new BufferedWriter(File.Create(path)))
            using (NumberBlockWriter writer = new NumberBlockWriter(bufferedWriter, blockSize))
            {
                for (int i = 0; i < count; ++i)
                {
                    writer.Write(r.Next(100000));
                }

                bytesWritten = bufferedWriter.BytesWritten;
            }

            r = new Random(5);
            using (BufferedReader bufferedReader = new BufferedReader(File.OpenRead(path)))
            using (NumberBlockReader reader = new NumberBlockReader(bufferedReader, blockSize))
            {
                int countRead = 0;

                int[] block;
                while (reader.ReadBlock(out block))
                {
                    for (int i = 0; i < blockSize; ++i)
                    {
                        int value = block[i];
                        Assert.AreEqual(r.Next(100000), value);
                    }

                    countRead += blockSize;
                }

                Assert.AreEqual(count, countRead);
                Assert.AreEqual(bytesWritten, bufferedReader.BytesRead);
                Assert.IsTrue(bufferedReader.EndOfStream);
            }
        }
    }
}
