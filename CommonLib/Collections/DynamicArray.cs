//
//  Author:
//    Hosein Yeganloo hoseinyeganloo@gmail.com
//
//  Copyright (c) 2017, 
//
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//       the documentation and/or other materials provided with the distribution.
//     * Neither the name of the [ORGANIZATION] nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

namespace CommonLib.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class DynamicArray<T> : IList<T>
    {

        #region Initialize

        public DynamicArray() : this(1024) { }
        public DynamicArray(int bufferSize)
        {
            _BufferSize = bufferSize;
            ExpandFactor = 2;
            Clear();
        }

        public int Length { get; private set; }

        public int Count
        {
            get
            {
                return Length;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        private int _LastX;
        private int _LastY;
        private readonly int _BufferSize;
        private readonly int ExpandFactor;
        private T[][] _Buffer;

        #endregion

        #region Actions

        public T Add(T item)
        {
            if (_LastY == _BufferSize - 1)
            {
                _LastY = 0;
                if (_LastX == _Buffer.Length - 1)
                    Expand(0);
                _Buffer[++_LastX] = new T[_BufferSize];
                _Buffer[_LastX][_LastY] = item;
            }
            else
            {
                _Buffer[_LastX][++_LastY] = item;
            }
            Length++;
            return item;
        }

        private void Expand(int min)
        {
            int ex = _Buffer.Length * ExpandFactor;
            var tmp = new T[ex < min ? min + 1 : ex][];
            Array.Copy(_Buffer, 0, tmp, 0, _Buffer.Length);
            _Buffer = tmp;
        }

        public void Clear()
        {
            _LastX = 0;
            _LastY = -1;
            _Buffer = new T[_BufferSize][];
            _Buffer[0] = new T[_BufferSize];
        }
        //TODO Index of first visit.
        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }
        //TODO Insert into index
        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }
        //TODO Remove at index
        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Add(T item)
        {
            this.Add(item);
        }
        //TODO Contains
        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array.Length + arrayIndex < this.Length)
                throw new System.OverflowException();
            for (int i = 0; i < _LastX; i++)
            {
                Array.Copy(_Buffer[i], 0, array, arrayIndex + (i * _BufferSize), _BufferSize);
            }
            Array.Copy(_Buffer[_LastX], 0, array, arrayIndex + (_LastX * _BufferSize), _LastY + 1);
        }
        //TODO Remove by value
        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DynamicArrayEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public class DynamicArrayEnumerator<TT> : IEnumerator<TT>
        {
            public DynamicArrayEnumerator(DynamicArray<TT> array)
            {
                this.array = array;
            }
            private DynamicArray<TT> array;
            private int index = -1;
            public TT Current
            {
                get
                {
                    return index > -1 ? array[index] : default(TT);
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }

            public void Dispose()
            {
                Reset();
            }

            public bool MoveNext()
            {
                return ++index < array.Length;
            }

            public void Reset()
            {
                index = -1;
            }
        }

        #endregion

        #region Indexing

        public T this[int index]
        {
            get
            {
                if (index >= Length)
                    throw new IndexOutOfRangeException();
                return _Buffer[index / _BufferSize][index % _BufferSize];
            }
            set
            {
                int x = index / _BufferSize;
                int y = index % _BufferSize;
                if (index >= Length)
                {
                    if (x >= _Buffer.Length)
                        Expand(x);
                    while (_LastX < x)
                        _Buffer[++_LastX] = new T[_BufferSize];
                    _LastY = y;
                    Length++;
                }
                _Buffer[x][y] = value;
            }
        }

        #endregion

    }
}
