using System;
using System.Collections;
using System.Collections.Generic;

namespace Eve.Collections
{
    public class CopyOnWriteArray<T> : IDynamicArray<T>
    {
        object glock = new object();
        IDynamicArray<T> array;

        public CopyOnWriteArray(int capacity = 1024)
        {
            array = new DynamicArray<T>(capacity);
        }
        public CopyOnWriteArray(IDynamicArray<T> array)
        {
            this.array = array;
        }

        public T this[int index]
        {
            get => array[index];
            set
            {
                lock (glock)
                {
                    var tmp = array.Clone();
                    tmp[index] = value;
                    array = tmp;
                }
            }
        }

        public int Count => array.Count;

        public bool IsReadOnly => false;

        public int BufferSize => array.BufferSize;

        public void Add(T item)
        {
            lock (glock)
            {
                var tmp = array.Clone();
                tmp.Add(item);
                array = tmp;
            };
        }

        public void Clear()
        {
            var tmp = new DynamicArray<T>(array.BufferSize);
            lock (glock)
            {
                array = tmp;
            }
        }

        public IDynamicArray<T> Clone() => new CopyOnWriteArray<T>(array.Clone());

        public bool Contains(T item) => array.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            array.CopyTo(array, arrayIndex);
        }

        public T Dequeue() => array.Dequeue();

        public IEnumerator<T> GetEnumerator() => array.GetEnumerator();

        public int IndexOf(T item) => array.IndexOf(item);

        public void Insert(int index, T item)
        {
            lock (glock)
            {
                var tmp = array.Clone();
                tmp.Insert(index, item);
                array = tmp;
            }
        }

        public T Pop() => array.Pop();

        public bool Remove(T item)
        {
            lock (glock)
            {
                var tmp = array.Clone();
                var done = tmp.Remove(item);
                array = tmp;
                return done;
            }
        }

        public bool Remove(Func<T, bool> comparer)
        {
            lock (glock)
            {
                var tmp = array.Clone();
                var done = tmp.Remove(comparer);
                array = tmp;
                return done;
            }
        }

        public void RemoveAt(int index)
        {
            lock (glock)
            {
                var tmp = array.Clone();
                tmp.RemoveAt(index);
                array = tmp;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => array.GetEnumerator();
    }
}