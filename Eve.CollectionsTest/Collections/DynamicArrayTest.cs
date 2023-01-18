﻿using Xunit;
using Eve.Collections;
using System.Collections.Generic;

namespace Eve.CollectionsTest
{
    [Collection("Non-Parallel")]
    public class DynamicArrayTest
    {
        private const int Round = 9000000;
        private int rem = 73;

        [Fact]
        public void _ReversSequence_ReadWrite()
        {

            var array = new DynamicArray<object>();
            for (int i = Round - 1; i > -1; i--)
            {
                array[i] = i;
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(i == (int)array[i]);
            }
        }

        [Fact]
        public void _Sequence_ReadWrite()
        {
            var array = new DynamicArray<object>();
            for (int i = 0; i < Round; i++)
            {
                array[i] = i;
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(i == (int)array[i]);
            }
        }

        [Fact]
        public void _CopyConstructor()
        {
            var array = new DynamicArray<object>();
            for (int i = 0; i < Round; i++)
            {
                array[i] = i;
            }
            var array2 = new DynamicArray<object>(array);
            for (int i = 0; i < Round; i++)
            {
                Assert.Equal(array[i], array2[i]);
            }
        }

        [Fact]
        public void _SequenceAdd_ReadWrite()
        {
            var array = new DynamicArray<object>();
            for (int i = 0; i < Round; i++)
            {
                array.Add(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(i == (int)array[i]);
            }
        }

        [Fact]
        public void _Queue_ReadWrite()
        {
            var array = new DynamicArray<object>();
            for (int i = 0; i < Round; i++)
            {
                array.Add(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(i == (int)array.Dequeue());
            }
        }

        [Fact]
        public void _Stack_ReadWrite()
        {
            var array = new DynamicArray<object>();
            for (int i = 0; i < Round; i++)
            {
                array.Add(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(Round - i - 1 == (int)array.Pop());
            }
        }

        [Fact]
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
                Assert.True(i == a[i + 10]);
            }
        }

        [Fact]
        public void RemoveAt()
        {
            DynamicArray<int> array = new DynamicArray<int>(10);
            for (int i = 0; i < 90; i++)
            {
                array.Add(i);
            }
            array.Dequeue();
            array.RemoveAt(rem);
            array.RemoveAt(rem);
            for (int i = 0; i < 87; i++)
            {
                if (i < rem)
                    Assert.True(i + 1 == array[i]);
                else
                    Assert.True(i + 3 == array[i]);
            }
        }

        [Fact]
        //TODO Better test
        public void Insert()
        {
            DynamicArray<int> array = new DynamicArray<int>(10);
            for (int i = 0; i < 90; i++)
            {
                array.Add(i);
            }
            array.Insert(rem, -1);
            for (int i = 0; i < 89; i++)
            {
                if (i < rem)
                    Assert.True(i == array[i]);
                else if (i == rem)
                    Assert.True(-1 == array[i]);
                else
                    Assert.Equal(i - 1, array[i]);
            }
        }

        [Fact]
        public void Contains()
        {
            DynamicArray<int> array = new DynamicArray<int>(10);
            for (int i = 0; i < rem; i++)
            {
                array.Add(i);
            }
            array.Dequeue();
            array.Add(rem);
            Assert.True(array.Contains(rem));
            Assert.Contains(rem, array);
            Assert.DoesNotContain(rem + 1, array);
        }

        [Fact]
        public void RemoveDequeuePop()
        {
            DynamicArray<int> array = new DynamicArray<int>(10);
            for (int i = 0; i < rem; i++)
            {
                array.Add(i);
            }
            array.Dequeue();
            array.Remove(rem - 5);
            array.Pop();
            Assert.DoesNotContain(0, array);
            Assert.DoesNotContain(rem - 5, array);
            Assert.DoesNotContain(rem - 1, array);
        }

        [Fact]
        public void MSList_ReadWrite()
        {
            var array = new List<object>();
            for (int i = 0; i < Round; i++)
            {
                array.Add(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(i == (int)array[i]);
            }
        }
    }
}
