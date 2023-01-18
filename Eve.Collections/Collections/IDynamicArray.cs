using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eve.Collections
{
    public interface IDynamicArray<T> : IList<T>
    {
        public T Pop();

        public T Dequeue();

        public bool Remove(Func<T, bool> comparer);

        public IDynamicArray<T> Clone();
        public int BufferSize { get; }
    }
}
