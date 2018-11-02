using Bion.Json;
using Bion.Text;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Bion.Console
{
    public class BionSearcher : IDisposable
    {
        private static String8 Files = String8.CopyExpensive("files");
        private static String8 Results = String8.CopyExpensive("results");

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
                    if (!lastRun.Contains(position))
                    {
                        // Close previous run, if any
                        if (!lastRun.IsEmpty()) { WriteRunSubsetEnd(writer); }

                        // Find containing run
                        lastRun = FindContainerAtDepth(position, _runDepth);

                        // Write run subset
                        _bionReader.Seek(lastRun.StartByteOffset);
                        WriteRunSubsetStart(writer);
                    }

                    // Find and write result
                    ContainerEntry result = FindContainerAtDepth(position, _runDepth + 2);
                    if (!result.IsEmpty() && !lastResult.Equals(result))
                    {
                        matchCount++;
                        if (matchCount <= skip) { continue; }

                        _bionReader.Seek(result.StartByteOffset);
                        JsonBionConverter.BionToJson(_bionReader, writer);

                        if (take >= 0 && matchCount >= skip + take) { break; }
                    }
                }

                if (take >= 0 && matchCount >= skip + take) { break; }
            }

            // Close last run, if any
            if (!lastRun.IsEmpty()) { WriteRunSubsetEnd(writer); }

            return matchCount;
        }

        private void WriteRunSubsetStart(JsonTextWriter writer)
        {
            int depth = _bionReader.Depth;

            // Read and copy everything in the run until the end object except the large collections
            while (_bionReader.Read() && _bionReader.Depth > depth)
            {
                if (_bionReader.TokenType == BionToken.PropertyName)
                {
                    String8 propertyName = _bionReader.CurrentString8();
                    if (propertyName.Equals(Files) || propertyName.Equals(Results))
                    {
                        _bionReader.Skip();
                        continue;
                    }
                }

                JsonBionConverter.WriteToken(_bionReader, writer);
            }

            // Start a results array and leave open
            writer.WritePropertyName("results");
            writer.WriteStartArray();
        }

        private void WriteRunSubsetEnd(JsonTextWriter writer)
        {
            // Close results array
            writer.WriteEndArray();

            // Close the run
            writer.WriteEndObject();
        }

        private ContainerEntry AncestorAtDepth(ContainerEntry entry, int entryDepth, int desiredDepth)
        {
            if (entryDepth < desiredDepth) { return ContainerEntry.Empty; }

            while (entryDepth > desiredDepth)
            {
                entry = _containerIndex.Parent(entry);
                entryDepth--;
            }

            return entry;
        }

        private ContainerEntry FindContainerAtDepth(long position, int desiredDepth)
        {
            // Find the first container ending after position.
            ContainerEntry firstAfter = _containerIndex.FirstEndingAfter(position);
            int firstAfterDepth = _containerIndex.Depth(firstAfter);

            // Find the ancestor at the desired depth. If there is one, and it contains position, we can use it
            ContainerEntry firstAtDepth = AncestorAtDepth(firstAfter, firstAfterDepth, desiredDepth);
            if (firstAtDepth.Contains(position))
            {
                return firstAtDepth;
            }

            // If nothing deep enough was indexed, we must find the nearest point of known depth and walk the BION
            long seekToPosition;
            int seekToDepth;

            // The nearest point is...
            if (firstAfter.StartByteOffset < position)
            {
                // The start of the first container ending after position, if it starts before position
                seekToPosition = firstAfter.StartByteOffset;
                seekToDepth = firstAfterDepth - 1;
            }
            else if (firstAfter.Index > 0)
            {
                // The end of the container before that, if it started after position
                ContainerEntry lastBefore = _containerIndex[firstAfter.Index - 1];
                seekToPosition = lastBefore.EndByteOffset;
                seekToDepth = _containerIndex.Depth(lastBefore) - 1;
            }
            else
            {
                // The start of the document, if the first container was found
                seekToPosition = 0;
                seekToDepth = 0;
            }

            // Seek to that position and read until we find a container at the right depth which contains position
            _bionReader.Seek(seekToPosition);
            long lastContainerStart = -1;
            long lastContainerEnd = -1;

            while (_bionReader.BytesRead < position + 3)
            {
                _bionReader.Read();

                if (_bionReader.Depth + seekToDepth == desiredDepth)
                {
                    lastContainerStart = _bionReader.BytesRead - 1;
                    _bionReader.SkipRest();
                    lastContainerEnd = _bionReader.BytesRead;
                }
            }

            ContainerEntry found = new ContainerEntry(lastContainerStart, lastContainerEnd, -1);
            if (!found.Contains(position)) { return ContainerEntry.Empty; }

            return found;
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
