using Xunit;
using Eve.Collections;


namespace Eve.CollectionsTest
{
    [Collection("Non-Parallel")]
    public class NamedListTest
    {
        private const int Round = 90000;

        [Fact]
        public void _Sequenc_ReadWrite()
        {
            var pool = new NamedList<object>();
            for (int i = 0; i < Round; i++)
            {
                pool.Add(i.ToString(), i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(i == (int)pool[i.ToString()]);
            }
        }

    }
}
