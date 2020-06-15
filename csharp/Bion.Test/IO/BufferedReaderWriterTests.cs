// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

using Bion.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test.IO
{
    [TestClass]
    public class BufferedReaderWriterTests
    {
        [TestMethod]
        public void BufferedReaderWriter_Basics()
        {
            Random r = new Random(5);
            byte[] content = new byte[1024];
            r.NextBytes(content);

            byte[] result = new byte[128];

            using (BufferedReader reader = BufferedReader.FromArray(content, 0, content.Length))
            using (BufferedWriter writer = BufferedWriter.ToArray(content))
            {
                while(!reader.EndOfStream)
                {
                    int length = reader.EnsureSpace(150);
                    writer.EnsureSpace(length);

                    Buffer.BlockCopy(reader.Buffer, reader.Index, writer.Buffer, writer.Index, length);

                    reader.Index += length;
                    writer.Index += length;
                }

                result = writer.Buffer;
            }

            Assert.IsTrue(result.Length >= content.Length);
            for(int i = 0; i < content.Length; ++i)
            {
                Assert.AreEqual(content[i], result[i], $"@{i:n0}, expect: {content[i]}, actual: {result[i]}");
            }
        }
    }
}
