// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Bion.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test.Text
{
    [TestClass]
    public class String8SetTests
    {
        [TestMethod]
        public void String8Set_Basics()
        {
            String8[] samples = { String8.CopyExpensive("One"), String8.CopyExpensive("Two"), String8.CopyExpensive("Three") };

            String8Set set = new String8Set();

            // Initial state
            Assert.AreEqual(0, set.Count);
            Assert.AreEqual(0, set.LengthBytes);

            // Add
            for(int i = 0; i < samples.Length; ++i)
            {
                set.Add(samples[i]);
            }
            Assert.AreEqual(3, set.Count);
            Assert.AreEqual(11, set.LengthBytes);

            // Indexer
            for(int i = 0; i < samples.Length; ++i)
            {
                Assert.AreEqual(samples[i], set[i]);
            }

            // Enumerate
            int count = 0;
            foreach(String8 value in set)
            {
                Assert.AreEqual(samples[count++], value);
            }

            // IndexOf
            Assert.AreEqual(1, set.IndexOf(samples[1]));
            Assert.AreEqual(-1, set.IndexOf(String8.CopyExpensive("Four")));

            // Remove
            Assert.IsTrue(set.Remove(samples[1]));
            Assert.AreEqual(2, set.Count);
            Assert.AreEqual(8, set.LengthBytes);
            Assert.AreEqual(samples[0], set[0]);
            Assert.AreEqual(samples[2], set[1]);

            // Clear
            set.Clear();
            Assert.AreEqual(0, set.Count);
            Assert.AreEqual(0, set.LengthBytes);
            

        }
    }
}
