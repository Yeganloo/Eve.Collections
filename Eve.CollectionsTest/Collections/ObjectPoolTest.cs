using NUnit.Framework;
using Eve.Collections;


namespace Eve.CollectionsTest
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
