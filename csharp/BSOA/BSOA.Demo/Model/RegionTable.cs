using BSOA;
using BSOA.IO;
using Microsoft.CodeAnalysis.Sarif;
using ScaleDemo.SoA;
using System.Collections;
using System.Collections.Generic;

namespace ScaleDemo
{
    public class RegionTable : ITable<Region4>
    {
        protected Dictionary<string, IColumn> Columns { get; private set; }

        internal NumberColumn<int> StartLine;
        internal NumberColumn<int> StartColumn;
        internal NumberColumn<int> EndLine;
        internal NumberColumn<int> EndColumn;

        internal NumberColumn<int> ByteOffset;
        internal NumberColumn<int> ByteLength;
        internal NumberColumn<int> CharOffset;
        internal NumberColumn<int> CharLength;

        public RegionTable()
        {
            Init();
        }

        private void Init()
        {
            this.Columns = new Dictionary<string, IColumn>();

            this.Columns[nameof(StartLine)] = this.StartLine = new NumberColumn<int>(0);
            this.Columns[nameof(StartColumn)] = this.StartColumn = new NumberColumn<int>(0);
            this.Columns[nameof(EndLine)] = this.EndLine = new NumberColumn<int>(0);
            this.Columns[nameof(EndColumn)] = this.EndColumn = new NumberColumn<int>(0);

            this.Columns[nameof(ByteOffset)] = this.ByteOffset = new NumberColumn<int>(-1);
            this.Columns[nameof(ByteLength)] = this.ByteLength = new NumberColumn<int>(0);
            this.Columns[nameof(CharOffset)] = this.CharOffset = new NumberColumn<int>(-1);
            this.Columns[nameof(CharLength)] = this.CharLength = new NumberColumn<int>(0);
        }

        public int Count { get; private set; }
        public Region4 this[int index] => new Region4(this, index);

        internal Region4 Add()
        {
            this.Count++;
            return this[this.Count - 1];
        }

        public void Add(Region region)
        {
            Region4 item = this.Add();

            item.StartLine = region.StartLine;
            item.StartColumn = region.StartColumn;
            item.EndLine = region.EndLine;
            item.EndColumn = region.EndColumn;
            item.ByteOffset = region.ByteOffset;
            item.ByteLength = region.ByteLength;
            item.CharOffset = region.CharOffset;
            item.CharLength = region.CharLength;
        }

        public IEnumerator<Region4> GetEnumerator()
        {
            return new TableEnumerator<Region4>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new TableEnumerator<Region4>(this);
        }

        public void Read(ITreeReader reader)
        {
            Init();

            //reader.ReadObject(this, setters);

            reader.Expect(TreeToken.StartObject);
            reader.Read();

            // Count
            reader.Expect(TreeToken.PropertyName);
            reader.Read();

            reader.Expect(TreeToken.Integer);
            this.Count = reader.ReadAsInt32();
            reader.Read();

            // Columns
            reader.Expect(TreeToken.PropertyName);
            reader.Read();

            reader.Expect(TreeToken.StartObject);
            reader.Read();

            while (reader.TokenType == TreeToken.PropertyName)
            {
                string tableName = reader.ReadAsString();
                reader.Read();

                Columns[tableName].Read(reader);
                reader.Read();
            }

            reader.Expect(TreeToken.EndObject);
            // Leave
        }

        public void Write(ITreeWriter writer)
        {
            writer.WriteStartObject();

            writer.Write(nameof(Count), Count);

            writer.WritePropertyName(nameof(Columns));
            writer.WriteDictionary(Columns);

            writer.WriteEndObject();
        }
    }
}
