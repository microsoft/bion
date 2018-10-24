using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bion.Test.Text
{
    [TestClass]
    public class SearchIndexTests
    {
        [TestMethod]
        public void SearchIndex_RoundTrip()
        {
            // Try with no merge (single set write and read)
            SearchIndex_RoundTrip(false);

            // Try with merge logic as well
            SearchIndex_RoundTrip(true);
        }

        public void SearchIndex_RoundTrip(bool requireMerge)
        {
            int wordCount = 100;
            int occurrences = 10;
            int indexBufferSize = (requireMerge ? 500 : 1000);

            string path = "SearchIndex.idx";
            using (SearchIndexWriter writer = new SearchIndexWriter(path, 100, indexBufferSize))
            {
                int position = 0;
                for(int occurrence = 0; occurrence < occurrences; ++occurrence)
                {
                    for(uint wordIndex = 0; wordIndex < wordCount; ++wordIndex)
                    {
                        writer.Add(wordIndex, position);
                        position++;
                    }
                }
            }

            using (SearchIndexReader reader = new SearchIndexReader(path))
            {
                long[] positions = new long[occurrences];

                for (uint wordIndex = 0; wordIndex < wordCount; ++wordIndex)
                {
                    // Read matches for word
                    int matchCount = reader.OffsetsForWord(wordIndex, ref positions);

                    // There should be 10 for every word
                    Assert.AreEqual(occurrences, matchCount);

                    // Verify matches are where expected
                    for(int occurrence = 0; occurrence < occurrences; ++occurrence)
                    {
                        long expected = wordIndex + (occurrence * wordCount);
                        Assert.AreEqual(expected, positions[occurrence]);
                    }
                }
            }
        }
    }
}
