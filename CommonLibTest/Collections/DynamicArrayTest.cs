using NUnit.Framework;
using CommonLib.Collections;
using System;
using System.Collections.Generic;

namespace CommonLibTest
{
    [TestFixture()]
    public class DynamicArrayTest
    {
        private const int Round = 3000000;

        [Test()]
        public void _()
        {

        }

        [Test()]
        public void _ReversSequenc_ReadWrite()
        {
            int k;
            var array = new DynamicArray<object>();
            for (int i = Round - 1; i > -1; i--)
            {
                array[i] = i;
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.IsTrue(i == (int)array[i]);
            }
        }

        [Test()]
        public void _Sequenc_ReadWrite()
        {
            int k;
            var array = new DynamicArray<object>();
            for (int i = 0; i < Round; i++)
            {
                array[i] = i;
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.IsTrue(i == (int)array[i]);
            }
        }

        [Test()]
        public void _SequencAdd_ReadWrite()
        {
            int k;
            var array = new DynamicArray<object>();
            for (int i = 0; i < Round; i++)
            {
                array.Add(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.IsTrue(i == (int)array[i]);
            }
        }

        [Test()]
        public void _Queue_ReadWrite()
        {
            int k;
            var array = new DynamicArray<object>();
            for (int i = 0; i < Round; i++)
            {
                array.Add(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.IsTrue(i == (int)array.Dequeue());
            }
        }

        [Test()]
        public void _Stack_ReadWrite()
        {
            int k;
            var array = new DynamicArray<object>();
            for (int i = 0; i < Round; i++)
            {
                array.Add(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.IsTrue(Round - i - 1 == (int)array.Pop());
            }
        }

        [Test()]
        public void CopyTo()
        {
            int[] a = new int[Round + 10];
            DynamicArray<int> array = new DynamicArray<int>();
            for (int i = 0; i < Round; i++)
            {
                array.Add(i);
            }
            array.CopyTo(a, 10);
            for (int i = 0; i < Round; i++)
            {
                Assert.IsTrue(i == a[i + 10]);
            }
        }

        [Test()]
        public void MSList_ReadWrite()
        {
            int k;
            var array = new List<object>();
            for (int i = 0; i < Round; i++)
            {
                array.Add(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.IsTrue(i == (int)array[i]);
            }
        }
    }
}
