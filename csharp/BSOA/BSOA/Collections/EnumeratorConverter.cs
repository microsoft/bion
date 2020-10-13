using System;
using System.Collections;
using System.Collections.Generic;

namespace BSOA.Collections
{
    /// <summary>
    ///  EnumeratorConverter adds a type converter (index to object model instance)
    ///  on top of an inner enumerator efficiently.
    /// </summary>
    /// <typeparam name="TInner">Inner enumerator type (index)</typeparam>
    /// <typeparam name="TOuter">Exposed enumerator type (object model instance)</typeparam>
    public sealed class EnumeratorConverter<TOuter, TInner> : IEnumerator<TOuter>
    {
        private Func<TInner, TOuter> _toInstance;
        private IEnumerator<TInner> _inner;

        public EnumeratorConverter(IEnumerator<TInner> list, Func<TInner, TOuter> toInstance)
        {
            _toInstance = toInstance;
            _inner = list;
        }

        public TOuter Current => _toInstance(_inner.Current);
        object IEnumerator.Current => _toInstance(_inner.Current);

        public bool MoveNext()
        {
            return _inner.MoveNext();
        }

        public void Reset()
        {
            _inner.Reset();
        }

        public void Dispose()
        {
            _inner?.Dispose();
            _inner = null;
        }
    }
}
