using BSOA.IO;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  RefListColumn provides a reference from an item in one table to a set
    ///  of items in another table. It stores the integer indices of the references.
    /// </summary>
    public class RefListColumn : IColumn<MutableSlice<int>>
    {
        private VariableLengthColumn<int> _inner;
        public string ReferencedTableName { get; }

        public int Count => _inner.Count;
        public bool Empty => Count == 0;

        public RefListColumn(string referencedTableName)
        {
            _inner = new VariableLengthColumn<int>();
            ReferencedTableName = referencedTableName;
        }

        public MutableSlice<int> this[int index] 
        {
            get => new MutableSlice<int>(_inner, index);
            set => _inner[index] = value.Slice;
        }

        public IEnumerator<MutableSlice<int>> GetEnumerator()
        {
            return new ListEnumerator<MutableSlice<int>>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<MutableSlice<int>>(this);
        }

        public void Trim()
        {
            _inner.Trim();
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public void Write(ITreeWriter writer)
        {
            _inner.Write(writer);
        }

        public void Read(ITreeReader reader)
        {
            _inner.Read(reader);
        }
    }
}