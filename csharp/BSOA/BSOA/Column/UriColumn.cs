using BSOA.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Column
{
    /// <summary>
    ///  UriColumn implements IColumn for Uri on top of a StringColumn
    /// </summary>
    public class UriColumn : IColumn<Uri>
    {
        private StringColumn _inner;

        public UriColumn()
        {
            _inner = new StringColumn();
        }

        public int Count => _inner.Count;

        public bool Empty => Count == 0;

        public Uri this[int index]
        {
            get
            {
                string uriText = _inner[index];
                return (uriText == null ? null : new Uri(uriText, UriKind.RelativeOrAbsolute));
            }

            set { _inner[index] = value?.OriginalString; }
        }

        public void Clear()
        {
            _inner.Clear();
        }

        public void Trim()
        {
            _inner.Trim();
        }

        public IEnumerator<Uri> GetEnumerator()
        {
            return new ListEnumerator<Uri>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ListEnumerator<Uri>(this);
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
