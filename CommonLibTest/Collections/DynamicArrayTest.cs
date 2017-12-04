using NUnit.Framework;
using CommonLib.Collections;
using System;
using System.Collections.Generic;

namespace CommonLibTest
{
    [TestFixture()]
    public class DynamicArrayTest
    {
        private const int Round = 50000000;

        [Test()]
        public void _()
        {

        }

        [Test()]
        public void _Sequenc_ReadWrite()
        {
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
        public void MSList_ReadWrite()
        {
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
