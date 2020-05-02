using System;

namespace BSOA.Column
{
    /// <summary>
    ///  EnumColumn implements IColumn for enums on top of a NumberColumn&lt;U&gt;
    ///  Constructors must identify underlying type and provide cast funcs.
    /// </summary>
    /// <example>
    ///  For: public enum DayOfWeek : byte { ... }
    ///  Use: new EnumColumn&lt;DayOfWeek, byte&gt;(DayOfWeek.Sunday, (v) => (byte)v, (v) => (DayOfWeek)v);
    /// </example>
    public class EnumColumn<T, U> : ConvertingColumn<T, U> where U : unmanaged, IEquatable<U>, IComparable<U>
    {
        public EnumColumn(T defaultValue, Func<T, U> toNumber, Func<U, T> toEnum)
            : base(new NumberColumn<U>(toNumber(defaultValue)), toNumber, toEnum)
        { }
    }
}
