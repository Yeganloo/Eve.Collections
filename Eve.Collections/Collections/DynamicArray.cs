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

namespace Eve.Collections
{
  using System;
  using System.Collections;
  using System.Collections.Generic;

  public class DynamicArray<T> : IList<T>
  {
    private object BufferLock = new object();
    #region Initialize

    public DynamicArray() : this(1024) { }
    public DynamicArray(int bufferSize)
    {
      _BufferSize = bufferSize;
      Clear();
    }

    public int Length
    {
      get
      {
        return _Length;
      }
      private set
      {
        _LastX = (_StartIndex + value) / _BufferSize;
        _LastY = value - _StartIndex - (_LastX * _BufferSize);
        _Length = value;
      }
    }

    public int Count
    {
      get
      {
        return _Length;
      }
    }

    public bool IsReadOnly
    {
      get
      {
        return false;
      }
    }

    private int _StartIndex;
    private int _Length;
    private int _LastX;
    private int _LastY;
    private readonly int _BufferSize;
    private T[][] _Buffer;

    #endregion

    #region Actions

    public void Add(T item)
    {
      if (++_LastY == _BufferSize)
      {
        _LastY = 0;
        if (++_LastX == _Buffer.Length)
          Expand(Length + 1);
        _Buffer[_LastX] = new T[_BufferSize];
        _Buffer[_LastX][_LastY] = item;
      }
      else
      {
        _Buffer[_LastX][_LastY] = item;
      }
      _Length++;
    }

    public T Pop()
    {
      var tmp = _Buffer[_LastX][_LastY];
      _Buffer[_LastX][_LastY] = default(T);
      if (_LastY-- == 0)
      {
        _Buffer[_LastX--] = null;
        _LastY = _BufferSize - 1;
      }
      _Length--;
      return tmp;
    }

    public T Dequeue()
    {
      int x = _StartIndex / _BufferSize;
      int y = _StartIndex - (x * _BufferSize);
      var tmp = _Buffer[x][y];
      if (x == _Buffer.Length - _BufferSize)
      {
        _StartIndex = 1;
        var arr = new T[_Buffer.Length - x][];
        Array.Copy(_Buffer, x, arr, 0, _Buffer.Length - x);
        _Buffer = arr;
      }
      else
      {
        _Buffer[x][y] = default(T);
        _StartIndex++;
      }
      _Length--;
      return tmp;
    }

    private void Expand(int min)
    {
      lock (BufferLock)
      {
        int ex = _Buffer.Length + _BufferSize;
        var tmp = new T[ex < min ? min + 1 : ex][];
        Array.Copy(_Buffer, 0, tmp, 0, _Buffer.Length);
        _Buffer = tmp;
      }
    }

    public void Clear()
    {
      _StartIndex = 0;
      _Length = 0;
      _LastX = 0;
      _LastY = -1;
      _Buffer = new T[_BufferSize][];
      _Buffer[0] = new T[_BufferSize];
    }

    public int IndexOf(T item)
    {
      for (int i = _StartIndex; i < Length; i++)
      {
        T k = this[i];
        if (k != null && k.Equals(item))
        {
          return i - _StartIndex;
        }
      }
      return -1;
    }

    public void Insert(int index, T item)
    {
      index += _StartIndex;
      int x = index / _BufferSize;
      int y = index - (x * _BufferSize);

      if (index < Length)
      {
        int i = _LastX;
        Add(_Buffer[_LastX][_LastY]);
        for (; i > x; i--)
        {
          Array.Copy(_Buffer[i], 0, _Buffer[i], 1, i == _LastX ? _LastY - 1 : _BufferSize - 1);
          _Buffer[i][0] = _Buffer[i - 1][_BufferSize - 1];
        }
        Array.Copy(_Buffer[x], y, _Buffer[x], y + 1, _BufferSize - y - 1);
        _Buffer[x][y] = item;
      }
      else
      {
        this[index] = item;
      }

    }

    public void RemoveAt(int index)
    {
      index = index + _StartIndex;
      int x = index / _BufferSize;
      int y = index - (x * _BufferSize);
      Array.Copy(_Buffer[x], y + 1, _Buffer[x], y, _BufferSize - y - 1);

      for (int i = x + 1; i <= _LastX; i++)
      {
        _Buffer[i - 1][_BufferSize - 1] = _Buffer[i][0];
        Array.Copy(_Buffer[i], 1, _Buffer[i], 0, _BufferSize - 1);
      }
      _Buffer[_LastX][_LastY] = default(T);
      Length--;
    }

    void ICollection<T>.Add(T item)
    {
      this.Add(item);
    }

    public bool Contains(T item)
    {
      for (int i = _StartIndex; i < Length + _StartIndex; i++)
      {
        T k = this[i];
        if (k != null && k.Equals(item))
        {
          return true;
        }
      }
      return false;
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

    public bool Remove(T item)
    {
      for (int i = _StartIndex; i < Length + _StartIndex; i++)
      {
        T k = this[i];
        if (k != null && k.Equals(item))
        {
          RemoveAt(i - _StartIndex);
          return true;
        }
      }
      return false;
    }

    public IEnumerator<T> GetEnumerator()
    {
      return new DynamicArrayEnumerator<T>(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    public DynamicArray<T> Clone()
    {
      var tmp = new DynamicArray<T>(_BufferSize);
      lock (BufferLock)
      {
        Array.Copy(_Buffer[0], 0, tmp._Buffer[0], 0, _BufferSize);
        for (int i = 1; i < _BufferSize; i++)
        {
          if (_Buffer[i] != null)
          {
            tmp._Buffer[i] = new T[_BufferSize];
            Array.Copy(_Buffer[i], 0, tmp._Buffer[i], 0, _BufferSize);
          }
        }
      }
      return tmp;
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
        index += _StartIndex;
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
          _Length = index + 1;
        }
        _Buffer[x][y] = value;
      }
    }

    #endregion

  }
}
