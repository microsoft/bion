using System;
using System.Collections.Generic;
using System.IO;

namespace Bion
{
    public struct ContainerEntry
    {
        public long EndByteOffset;
        public long ByteLength;
        public int ParentIndex;

        public long StartByteOffset => EndByteOffset - ByteLength;

        public static ContainerEntry Empty = new ContainerEntry(-1, -1);

        public ContainerEntry(long startByteOffset, long endByteOffset)
        {
            this.EndByteOffset = endByteOffset;
            this.ByteLength = endByteOffset - startByteOffset;
            this.ParentIndex = -1;
        }
    }

    internal class IndexEntryEndOffsetComparer : IComparer<ContainerEntry>
    {
        public static IndexEntryEndOffsetComparer Instance = new IndexEntryEndOffsetComparer();

        public int Compare(ContainerEntry x, ContainerEntry y)
        {
            return x.EndByteOffset.CompareTo(y.EndByteOffset);
        }
    }

    public class ContainerIndex : IDisposable
    {
        public const int ContainerLengthCutoff = 1024;

        private List<ContainerEntry> _index;
        private Stream _writeToStream;
        private Stack<long> _currentStack;
        private long _lastEnd;

        public int Count => _index.Count;

        private ContainerIndex()
        {
            _index = new List<ContainerEntry>();
            _currentStack = new Stack<long>();
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

        public void Start(long containerStartOffset)
        {
            _currentStack.Push(containerStartOffset);
        }

        public void End(long containerEndOffset)
        {
            ContainerEntry entry = new ContainerEntry(_currentStack.Pop(), containerEndOffset);

            if (entry.ByteLength >= ContainerLengthCutoff || containerEndOffset - _lastEnd >= ContainerLengthCutoff)
            {
                _index.Add(entry);
                _lastEnd = containerEndOffset;
            }
        }

        public ContainerEntry NearestIndexedContainer(long position)
        {
            // Find the first container which ends after this position
            int containerIndex = _index.BinarySearch(new ContainerEntry(position, position), IndexEntryEndOffsetComparer.Instance);
            if (containerIndex < 0) containerIndex = ~containerIndex;

            return (containerIndex >= _index.Count ? ContainerEntry.Empty : _index[containerIndex]);
        }

        public ContainerEntry Parent(ContainerEntry entry)
        {
            if (entry.ParentIndex < 0 || entry.ParentIndex >= _index.Count) { return ContainerEntry.Empty; }
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

                _index.Add(new ContainerEntry(endPosition - byteLength, endPosition));
                lastEndPosition = endPosition;
            }

            // Reconstruct the hierarchy
            for (int i = _index.Count - 2; i >= 0; --i)
            {
                ContainerEntry current = _index[i];

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

            foreach (ContainerEntry entry in _index)
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
