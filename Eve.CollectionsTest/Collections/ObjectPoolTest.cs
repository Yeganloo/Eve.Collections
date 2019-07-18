using Xunit;
using Eve.Collections;


namespace Eve.CollectionsTest
{
    public class ObjectPoolTest
    {
        private const int Round = 3000000;

        [Fact]
        public void _()
        {

        }

        [Fact]
        public void _Sequenc_ReadWrite()
        {
            var pool = new ObjectPool<object>();
            for (int i = 0; i < Round; i++)
            {
                pool.Catch(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(Round - i - 1 == (int)pool.Get());
            }
        }

    }
}
