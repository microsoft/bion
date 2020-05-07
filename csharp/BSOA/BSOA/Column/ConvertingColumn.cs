using BSOA.Converter;
using BSOA.IO;
using BSOA.Model;
using System;

namespace BSOA.Column
{
    /// <summary>
    ///  ConvertingColumn provides an IColumn&lt;T&gt; on an inner IColumn&lt;U&gt;
    ///  given functions to convert back and forth.
    /// </summary>
    /// <typeparam name="T">Outer type exposed by the column (DateTime)</typeparam>
    /// <typeparam name="U">Inner type actually stored (long)</typeparam>
    public class ConvertingColumn<T, U> : WrappingColumn<T, U>
    {
        private Func<T, U> _toInner;
        private Func<U, T> _toOuter;

        public ConvertingColumn(IColumn<U> inner, IConverter<T, U> converter)
            : this(inner, converter.Convert, converter.Convert)
        { }

        public ConvertingColumn(IColumn<U> inner, Func<T, U> toInner, Func<U, T> toOuter) : base(inner)
        {
            _toInner = toInner;
            _toOuter = toOuter;
        }

        public override T this[int index] 
        {
            get => _toOuter(Inner[index]);
            set => Inner[index] = _toInner(value);
        }
    }
}
