using BSOA.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  EnumColumn implements IColumn for enums on top of a NumberColumn&lt;U&gt;
    ///  Callers must specify the underlying enum storage type
    /// </summary>
    public class EnumColumn<T, U> : IColumn<T> where U : unmanaged, IEquatable<U>
    {
        private NumberColumn<U> _inner;

        public EnumColumn(T defaultValue)
        {
            _inner = new NumberColumn<U>((U)(object)defaultValue);
        }

        public int Count => _inner.Count;
        
        public bool Empty => Count == 0;
        
        public T this[int index]
        {
            get { return (T)(object)_inner[index]; }
            set { _inner[index] = (U)(object)value; }
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public void Trim()
        {
            _inner.Trim();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<T>(this);
        }

        public void Read(ITreeReader reader)
        {
            _inner.Read(reader);
        }

        public void Write(ITreeWriter writer)
        {
            _inner.Write(writer);
        }
    }
}
