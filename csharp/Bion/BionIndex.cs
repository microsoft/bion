using System.Collections.Generic;

namespace Bion
{
    public struct IndexEntry
    {
        public uint ParentIndex;
        public ulong StartByteOffset;
        public ulong ByteLength;
        public object Identifier;
        public uint Count;

        public void Read(BionReader reader)
        {
            reader.Read(BionToken.StartArray);

            reader.Read(BionToken.Integer);
            this.StartByteOffset = (ulong)reader.CurrentInteger();

            reader.Read(BionToken.Integer);
            this.ByteLength = (ulong)reader.CurrentInteger();

            reader.Read();
            if (reader.TokenType == BionToken.String)
            {
                this.Identifier = reader.CurrentString();
            }
            else if (reader.TokenType == BionToken.Integer)
            {
                this.Identifier = (ulong)reader.CurrentInteger();
            }
            else
            {
                throw new BionSyntaxException(reader, "string or integer");
            }

            reader.Read(BionToken.Integer);
            this.Count = (uint)reader.CurrentInteger();

            reader.Read(BionToken.EndArray);
        }

        public void Write(BionWriter writer)
        {
            writer.WriteStartArray();
            writer.WriteValue(this.StartByteOffset);
            writer.WriteValue(this.ByteLength);

            string identifier = this.Identifier as string;
            if (identifier != null)
            {
                writer.WriteValue(identifier);
            }
            else
            {
                writer.WriteValue((ulong)this.Identifier);
            }

            writer.WriteValue(this.Count);
            writer.WriteEndArray();
        }
    }

    public class BionIndex
    {
        private List<IndexEntry> _index;

        public BionIndex()
        {
            _index = new List<IndexEntry>();
        }

        public void Add(IndexEntry entry)
        {
            _index.Add(entry);
        }

        // TODO: Skip or Skip helping method.
        // TODO: Get path by offset and offset for path; or closest BionIndex can get.
        // Are these implemented in BionReader or here?

        public void Read(BionReader reader)
        {
            reader.Read(BionToken.StartArray);

            while(reader.Read())
            {
                if (reader.TokenType == BionToken.EndArray) break;

                IndexEntry entry = new IndexEntry();
                entry.Read(reader);
                _index.Add(entry);
            }

            // TODO: Identify and map parent indices. (Backward walk)
        }
        
        public void Write(BionWriter writer)
        {
            writer.WriteStartArray();

            foreach(IndexEntry entry in _index)
            {
                entry.Write(writer);
            }

            writer.WriteEndArray();
        }
    }
}
