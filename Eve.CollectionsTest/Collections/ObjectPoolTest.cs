using NUnit.Framework;
using CommonLib.Collections;
using System;
using System.Collections.Generic;

namespace CommonLibTest
{
    [TestFixture()]
    public class ObjectPoolTest
    {
        private const int Round = 3000000;

        [Test()]
        public void _()
        {

        }

        [Test()]
        public void _Sequenc_ReadWrite()
        {
            int k;
            var pool = new ObjectPool<object>();
            for (int i = 0; i < Round; i++)
            {
                pool.Catch(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.IsTrue(Round - i - 1 == (int)pool.Get());
            }
        }

    }
}
