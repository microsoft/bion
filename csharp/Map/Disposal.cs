using System;
using System.Collections.Generic;

namespace Map
{
    /// <summary>
    ///  Disposal manages collecting nested classes which all need to be Disposed
    ///  so that a dynamic number of them can safely be created.
    ///  
    ///  Usage:
    ///  using (Disposal disposal = new Disposal())
    ///  using (Stream something = RecursivelyCreateStream(descriptor, disposal))
    ///  {
    ///     ...
    ///  }
    ///  
    ///  // [Disposal will ensure *all* intermediate wrapping streams were disposed]
    /// </summary>
    public class Disposal : IDisposable
    {
        private List<IDisposable> _components;

        public Disposal()
        {
            _components = new List<IDisposable>();
        }

        public T Add<T>(T component) where T : IDisposable
        {
            _components.Add(component);
            return component;
        }

        public void Dispose()
        {
            if (_components != null)
            {
                // Dispose components backwards (so innermost object is Disposed first)
                for (int i = _components.Count - 1; i >= 0; --i)
                {
                    _components[i].Dispose();
                }

                _components = null;
            }
        }
    }
}
