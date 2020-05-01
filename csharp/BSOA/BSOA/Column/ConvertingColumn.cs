using BSOA.Converter;
using BSOA.IO;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  ConvertingColumn provides an IColumn&lt;T&gt; on an inner IColumn&lt;U&gt;
    ///  given functions to convert back and forth.
    /// </summary>
    /// <typeparam name="T">Outer type exposed by the column (DateTime)</typeparam>
    /// <typeparam name="U">Inner type actually stored (long)</typeparam>
    public class ConvertingColumn<T, U> : IColumn<T>
    {
        private IColumn<U> _inner;
        private IConverter<T, U> _toInner;
        private IConverter<U, T> _toOuter;

        public ConvertingColumn(IColumn<U> inner, IConverter<T, U> toInner, IConverter<U, T> toOuter)
        {
            _inner = inner;
            _toInner = toInner;
            _toOuter = toOuter;
        }

        public T this[int index] 
        {
            get => _toOuter.Convert(_inner[index]);
            set => _inner[index] = _toInner.Convert(value);
        }

        public int Count => _inner.Count;

        public bool Empty => (Count == 0);

        public void Clear()
        {
            _inner.Clear();
        }

        public void Trim()
        {
            _inner.Trim();
        }

        public void Read(ITreeReader reader)
        {
            _inner.Read(reader);
        }

        public void Write(ITreeWriter writer)
        {
            _inner.Write(writer);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }
    }
}
