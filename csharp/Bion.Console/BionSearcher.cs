using Bion.Json;
using Bion.Text;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Bion.Console
{
    public class BionSearcher : IDisposable
    {
        private WordCompressor _compressor;
        private SearchIndexReader _searchIndexReader;
        private ContainerIndex _containerIndex;
        private BionReader _bionReader;

        private int _runDepth;
        private long[] _termPositions;

        public BionSearcher(string bionFilePath, int runDepth)
        {
            _compressor = WordCompressor.OpenRead(Path.ChangeExtension(bionFilePath, ".wdx"));
            _containerIndex = ContainerIndex.OpenRead(Path.ChangeExtension(bionFilePath, ".cdx"));
            _searchIndexReader = new SearchIndexReader(Path.ChangeExtension(bionFilePath, ".idx"));
            _bionReader = new BionReader(File.OpenRead(bionFilePath), containerIndex: _containerIndex, compressor: _compressor);

            _runDepth = runDepth;
            _termPositions = new long[256];
        }

        public ISearchResult Find(String8 term)
        {
            int termIndex;
            if (!_compressor.TryGetWordIndex(term, out termIndex))
            {
                return EmptyResult.Instance;
            }

            return _searchIndexReader.Find(termIndex);
        }

        public int Write(JsonTextWriter writer, ISearchResult wordMatches, int skip = 0, int take = -1)
        {
            int matchCount = 0;

            ContainerEntry lastRun = ContainerEntry.Empty;
            ContainerEntry lastResult = ContainerEntry.Empty;

            while (!wordMatches.Done)
            {
                int count = wordMatches.Page(ref _termPositions);
                for (int i = 0; i < count; ++i)
                {
                    long position = _termPositions[i];

                    // If this position isn't in the last run's results array, find the run which contains it
                    if(!lastRun.Contains(position))
                    {
                        lastRun = FindContainerAtDepth(position, _runDepth);

                        // TODO: Write run
                    }

                    ContainerEntry result = FindContainerAtDepth(position, _runDepth + 2);
                    if(!lastResult.Equals(result))
                    {
                        matchCount++;
                        if (matchCount <= skip) { continue; }
                        
                        _bionReader.Seek(result.StartByteOffset);
                        JsonBionConverter.BionToJson(_bionReader, writer);

                        if (take >= 0 && matchCount >= skip + take) { return matchCount; }
                    }
                }
            }

            return matchCount;
        }

        private ContainerEntry FindContainerAtDepth(long position, int depth)
        {
            ContainerEntry nearestContainer = _containerIndex.NearestIndexedContainer(position);
            int nearestDepth = _containerIndex.Depth(nearestContainer);

            // If the container we want is indexed, just return it
            if (nearestDepth >= depth)
            {
                return _containerIndex.AncestorAtDepth(nearestContainer, depth);
            }

            // If not, seek to the closest peer we know of
            ContainerEntry closest = _containerIndex[_containerIndex.IndexOfFirstEndingAfter(position) - 1];
            int closestDepth = _containerIndex.Depth(closest);

            // Skip in the document until we find the right container
            _bionReader.Seek(closest.EndByteOffset);
            long currentPosition = _bionReader.BytesRead;
            long nextPosition = currentPosition;

            while (nextPosition < position)
            {
                if (_bionReader.Depth + closestDepth == depth)
                {
                    _bionReader.Skip();
                }
                else
                {
                    _bionReader.Read();
                }

                currentPosition = nextPosition;
                nextPosition = _bionReader.BytesRead;
            }

            return new ContainerEntry(currentPosition, nextPosition - 1, -1);
        }

        public void Dispose()
        {
            _compressor?.Dispose();
            _compressor = null;

            _searchIndexReader?.Dispose();
            _searchIndexReader = null;

            _containerIndex?.Dispose();
            _containerIndex = null;

            _bionReader?.Dispose();
            _bionReader = null;
        }
    }
}
