using System;
using System.Collections.Generic;
using System.IO;

namespace Bion
{
    public struct IndexEntry
    {
        public long EndByteOffset;
        public long ByteLength;
        public int ParentIndex;

        public long StartByteOffset => EndByteOffset - ByteLength;

        public static IndexEntry Empty = new IndexEntry(-1, 0);

        public IndexEntry(long endByteOffset, long byteLength)
        {
            this.EndByteOffset = endByteOffset;
            this.ByteLength = byteLength;
            this.ParentIndex = -1;
        }
    }

    internal class IndexEntryEndOffsetComparer : IComparer<IndexEntry>
    {
        public static IndexEntryEndOffsetComparer Instance = new IndexEntryEndOffsetComparer();

        public int Compare(IndexEntry x, IndexEntry y)
        {
            return x.EndByteOffset.CompareTo(y.EndByteOffset);
        }
    }

    public class ContainerIndex : IDisposable
    {
        private List<IndexEntry> _index;
        private Stream _writeToStream;
        public int Count => _index.Count;

        private ContainerIndex()
        {
            _index = new List<IndexEntry>();
        }

        public static ContainerIndex OpenWrite(string indexPath)
        {
            return new ContainerIndex() { _writeToStream = File.Create(indexPath) };
        }

        public static ContainerIndex OpenRead(string indexPath)
        {
            ContainerIndex index = new ContainerIndex();
            index.Read(File.OpenRead(indexPath));
            return index;
        }

        public void Add(long startByteOffset, long endByteOffset)
        {
            _index.Add(new IndexEntry(endByteOffset, endByteOffset - startByteOffset));
        }

        public IndexEntry NearestIndexedContainer(long position)
        {
            // Find the first container which ends after this position
            int containerIndex = _index.BinarySearch(new IndexEntry(position, 0), IndexEntryEndOffsetComparer.Instance);
            if (containerIndex < 0) containerIndex = ~containerIndex;

            return (containerIndex >= _index.Count ? IndexEntry.Empty : _index[containerIndex]);
        }

        public IndexEntry Parent(IndexEntry entry)
        {
            if (entry.ParentIndex < 0 || entry.ParentIndex >= _index.Count) { return IndexEntry.Empty; }
            return _index[entry.ParentIndex];
        }

        public void Read(Stream stream)
        {
            using (BionReader reader = new BionReader(stream))
            {
                Read(reader);
            }
        }

        public void Read(BionReader reader)
        {
            reader.Read(BionToken.StartArray);

            // Read the Container Index
            long lastEndPosition = 0;
            while (reader.Read())
            {
                if (reader.TokenType == BionToken.EndArray) { break; }

                reader.Read(BionToken.Integer);
                long endPosition = lastEndPosition + reader.CurrentInteger();

                reader.Read(BionToken.Integer);
                long byteLength = reader.CurrentInteger();

                reader.Read(BionToken.EndArray);

                _index.Add(new IndexEntry(endPosition, byteLength));
                lastEndPosition = endPosition;
            }

            // Reconstruct the hierarchy
            for (int i = _index.Count - 2; i >= 0; --i)
            {
                IndexEntry current = _index[i];

                // Find the parent - the first container which starts before this one
                int parentIndex = i + 1;
                while (parentIndex != -1)
                {
                    if (_index[parentIndex].StartByteOffset < current.StartByteOffset)
                    {
                        current.ParentIndex = parentIndex;
                        _index[i] = current;
                        break;
                    }

                    parentIndex = _index[parentIndex].ParentIndex;
                }
            }
        }

        public void Write(Stream stream)
        {
            using (BionWriter writer = new BionWriter(stream))
            {
                Write(writer);
            }
        }

        public void Write(BionWriter writer)
        {
            writer.WriteStartArray();

            long lastEndPosition = 0;

            foreach (IndexEntry entry in _index)
            {
                writer.WriteStartArray();
                writer.WriteValue(entry.EndByteOffset - lastEndPosition);
                writer.WriteValue(entry.ByteLength);
                writer.WriteEndArray();

                lastEndPosition = entry.EndByteOffset;
            }

            writer.WriteEndArray();
        }

        public void Dispose()
        {
            if (_writeToStream != null)
            {
                Write(_writeToStream);
                _writeToStream = null;
            }
        }
    }
}
