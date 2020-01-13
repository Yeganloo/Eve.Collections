﻿//
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

    public class ObjectPool<T>
    {
        private object GLock = new object();
        public ObjectPool() : this(() => { return Activator.CreateInstance<T>(); }) { }
        public ObjectPool(Func<T> constructor) { _Constructor = constructor; }

        private Func<T> _Constructor = null;
        private Func<T, T> _Destructor = null;
        private DynamicArray<T> _Pool = new DynamicArray<T>();

        public void Catch(T Object)
        {
            lock (GLock)
            {
                if (_Destructor != null)
                    _Pool.Add(_Destructor(Object));
                else
                    _Pool.Add(Object);
            }
        }

        public T Get()
        {
            if (_Pool.Length > 0)
                lock (GLock)
                {
                    return _Pool.Pop();
                }
            return _Constructor();
            
        }

        public int Count
        {
            get
            {
                return _Pool.Length;
            }
        }

    }
}
