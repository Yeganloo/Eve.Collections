using Xunit;
using Eve.Collections;


namespace Eve.CollectionsTest
{
    [Collection("Non-Parallel")]
    public class ObjectPoolTest
    {
        private const int Round = 9000000;

        [Fact]
        public void _Sequence_ReadWrite()
        {
            var pool = new ObjectPool<object>();
            for (int i = 0; i < Round; i++)
            {
                pool.Catch(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(i == (int)pool.Get());
            }
        }

    }
}
