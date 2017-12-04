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

using System;
namespace CommonLib.Collections
{
    public class DynamicArray<T>
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
