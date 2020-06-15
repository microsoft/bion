// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

using Bion.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test.Core
{
    [TestClass]
    public class HashingTests
    {
        [TestMethod]
        public void Hashing_Unique()
        {
            HashSet<ulong> hashes = new HashSet<ulong>();

            // Verify hashing ulongs is unique
            for(int i = 0; i < 100000; ++i)
            {
                Assert.IsTrue(hashes.Add(Hashing.Murmur2((ulong)i, 0)));
            }
            hashes.Clear();

            // Verify hashing bytes is unique
            Random r = new Random(5);
            byte[] bytes = new byte[60];
            for(int i = 0; i < 100000; ++i)
            {
                r.NextBytes(bytes);
                Assert.IsTrue(hashes.Add(Hashing.Murmur2(bytes, 0, bytes.Length, 0)));
            }
        }
    }
}
