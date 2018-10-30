using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test
{
    [TestClass]
    public class ContainerIndexTests
    {
        [TestMethod]
        public void ContainerIndex_Basics()
        {
            int threshold = 20000;

            using (ContainerIndex index = ContainerIndex.OpenWrite("Sample.cdx"))
            {
                // Add three top level containers
                index.Start(threshold - 3);
                index.Start(threshold - 2);
                index.Start(threshold - 1);

                // Add 100 adjacent peers
                for (int i = 1; i <= 100; ++i)
                {
                    int start = i * threshold;
                    index.Start(start);
                    index.End(start + threshold - 1);
                }

                int end = (101 * threshold) - 1;

                // Close the top level parents
                index.End(end + 1);
                index.End(end + 2);
                index.End(end + 3);
            }

            using (ContainerIndex index = ContainerIndex.OpenRead("Sample.cdx"))
            {
                // Verify 103 entries read back
                Assert.AreEqual(103, index.Count);

                for (int i = 1; i < 100; ++i)
                {
                    int start = i * threshold;
                    int end = start + threshold - 1;

                    // Verify positions inside each container return that container
                    Assert.AreEqual(start, index.NearestIndexedContainer(start).StartByteOffset);
                    Assert.AreEqual(start, index.NearestIndexedContainer(start + 1).StartByteOffset);
                    Assert.AreEqual(start, index.NearestIndexedContainer(end - 1).StartByteOffset);
                    Assert.AreEqual(start, index.NearestIndexedContainer(end).StartByteOffset);

                    // Verify the correct three parents are returned
                    ContainerEntry entry = index.NearestIndexedContainer(start);

                    entry = index.Parent(entry);
                    Assert.AreEqual(threshold - 1, entry.StartByteOffset);

                    entry = index.Parent(entry);
                    Assert.AreEqual(threshold - 2, entry.StartByteOffset);

                    entry = index.Parent(entry);
                    Assert.AreEqual(threshold - 3, entry.StartByteOffset);

                    entry = index.Parent(entry);
                    Assert.AreEqual(-1, entry.StartByteOffset);
                }
            }
        }
    }
}
