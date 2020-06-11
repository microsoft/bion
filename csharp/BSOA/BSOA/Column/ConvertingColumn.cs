using BSOA.Converter;
using BSOA.Model;
using System;

namespace BSOA.Column
{
    /// <summary>
    ///  ConvertingColumn provides an IColumn&lt;T&gt; on an inner IColumn&lt;U&gt;
    ///  given functions to convert back and forth.
    /// </summary>
    /// <typeparam name="TOuter">Outer type exposed by the column (DateTime)</typeparam>
    /// <typeparam name="TInner">Inner type actually stored (long)</typeparam>
    public class ConvertingColumn<TOuter, TInner> : WrappingColumn<TOuter, TInner>
    {
        private Func<TOuter, TInner> _toInner;
        private Func<TInner, TOuter> _toOuter;

        public ConvertingColumn(IColumn<TInner> inner, IConverter<TOuter, TInner> converter)
            : this(inner, converter.Convert, converter.Convert)
        { }

        public ConvertingColumn(IColumn<TInner> inner, Func<TOuter, TInner> toInner, Func<TInner, TOuter> toOuter) : base(inner)
        {
            _toInner = toInner;
            _toOuter = toOuter;
        }

        public override TOuter this[int index] 
        {
            get => _toOuter(Inner[index]);
            set => Inner[index] = _toInner(value);
        }
    }
}
