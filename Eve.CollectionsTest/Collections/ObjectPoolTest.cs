using Xunit;
using Xunit.Abstractions;
using System;
using Eve.Collections;


namespace Eve.CollectionsTest
{
    [Collection("Non-Parallel")]
    public class ObjectPoolTest
    {
        private const int Round = 90000000;
        private readonly ITestOutputHelper output;

        public ObjectPoolTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void _Sequence_ReadWrite()
        {
            var d = DateTime.UtcNow;
            var pool = new ObjectPool<object>();
            for (int i = 0; i < Round; i++)
            {
                pool.Catch(i);
            }
            for (int i = 0; i < Round; i++)
            {
                Assert.True(i == (int)pool.Get());
            }
            output.WriteLine("Time {0}", DateTime.UtcNow.Subtract(d).TotalMilliseconds);
        }

    }
}
