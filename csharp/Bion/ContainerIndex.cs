using System;
using System.Collections.Generic;
using System.IO;

namespace Bion
{
    public struct ContainerEntry : IEquatable<ContainerEntry>
    {
        public long EndByteOffset;
        public long ByteLength;
        public int Index;
        public int ParentIndex;

        public long StartByteOffset => EndByteOffset - ByteLength;

        public static ContainerEntry Empty = new ContainerEntry(-1);

        public ContainerEntry(long position) : this(position, position, -1)
        { }

        public ContainerEntry(long startByteOffset, long endByteOffset, int index)
        {
            this.EndByteOffset = endByteOffset;
            this.ByteLength = endByteOffset - startByteOffset;
            this.Index = index;
            this.ParentIndex = -1;
        }

        public bool Equals(ContainerEntry other)
        {
            return this.EndByteOffset == other.EndByteOffset;
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
        public const int ContainerLengthCutoff = 2048;

        private List<ContainerEntry> _index;
        private Stream _writeToStream;
        private Stack<long> _currentStack;

        private long _lastIndexedEnd;
        private int _mustIndexDepth;

        public int Count => _index.Count;
        public ContainerEntry this[int index] => _index[index];

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
            ContainerEntry entry = new ContainerEntry(_currentStack.Pop(), containerEndOffset, _index.Count);
            int depth = _currentStack.Count;

            // Index this container if it's long enough, it's far enough away from the last indexed container, or if we indexed a descendant of it
            if (entry.ByteLength >= ContainerLengthCutoff 
                || containerEndOffset - _lastIndexedEnd >= ContainerLengthCutoff
                || _mustIndexDepth == depth)
            {
                _index.Add(entry);
                _lastIndexedEnd = containerEndOffset;
                _mustIndexDepth = depth - 1;
            }
        }

        public int IndexOfFirstEndingAfter(long position)
        {
            // Find the first container which ends at or after this position
            int containerIndex = _index.BinarySearch(new ContainerEntry(position), IndexEntryEndOffsetComparer.Instance);
            if (containerIndex < 0) { containerIndex = ~containerIndex; }

            return containerIndex;
        }

        public ContainerEntry NearestIndexedContainer(long position)
        {
            // We want the first container which ends after the position and starts before it.
            
            // All containers ending before position can't contain it.
            // Find the first container ending after position.
            int indexAfter = IndexOfFirstEndingAfter(position);

            // If position was after the last container end, return empty
            if (indexAfter >= _index.Count) { return ContainerEntry.Empty; }

            // Find the first ancestor of the container which starts before position
            ContainerEntry candidate = this[indexAfter];
            while(candidate.StartByteOffset > position)
            {
                candidate = Parent(candidate);
            }

            // Since all ancestors of indexed containers are indexed, this must be the closest indexed ancestor.
            return candidate;
        }

        public ContainerEntry LastChildBefore(ContainerEntry parent, long position)
        {
            // Find the container before the first one ending after position.
            int indexBefore = IndexOfFirstEndingAfter(position) - 1;
            if (indexBefore <= 0) return ContainerEntry.Empty;

            // Find the first ancestor of the container which has the desired parent
            ContainerEntry candidate = this[indexBefore];
            while(candidate.ParentIndex != parent.Index && candidate.ParentIndex >= 0)
            {
                candidate = Parent(candidate);
            }

            return candidate;
        }

        public ContainerEntry Parent(ContainerEntry entry)
        {
            if (entry.ParentIndex < 0 || entry.ParentIndex >= _index.Count) { return ContainerEntry.Empty; }
            return _index[entry.ParentIndex];
        }

        public ContainerEntry AncestorAtDepth(ContainerEntry entry, int depth)
        {
            int depthCounted = 0;
            ContainerEntry ancestor = entry;
            while(ancestor.Index != -1)
            {
                depthCounted++;
                ancestor = Parent(ancestor);
            }

            int currentDepth = depthCounted;
            ancestor = entry;
            while(currentDepth != depth)
            {
                ancestor = Parent(ancestor);
                currentDepth--;
            }

            return ancestor;
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
            _index.Clear();
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

                _index.Add(new ContainerEntry(endPosition - byteLength, endPosition, _index.Count));
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
