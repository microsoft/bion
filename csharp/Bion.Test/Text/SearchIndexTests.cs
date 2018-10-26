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
            int increment = 16;

            string path = "SearchIndex.idx";
            using (SearchIndexWriter writer = new SearchIndexWriter(path, 100, indexBufferSize))
            {
                int position = 0;
                for(int occurrence = 0; occurrence < occurrences; ++occurrence)
                {
                    for(int wordIndex = 0; wordIndex < wordCount; ++wordIndex)
                    {
                        writer.Add(wordIndex, position);
                        position += increment;
                    }
                }
            }

            using (SearchIndexReader reader = new SearchIndexReader(path))
            {
                long[] positions = new long[occurrences];

                for (int wordIndex = 0; wordIndex < wordCount; ++wordIndex)
                {
                    // Read matches for word
                    int matchCount = reader.OffsetsForWord(wordIndex, ref positions);

                    // There should be 10 for every word
                    Assert.AreEqual(occurrences, matchCount);

                    // Verify matches are where expected
                    for(int occurrence = 0; occurrence < occurrences; ++occurrence)
                    {
                        long expected = increment * (wordIndex + (occurrence * wordCount));
                        Assert.AreEqual(expected, positions[occurrence]);
                    }
                }
            }
        }
    }
}
